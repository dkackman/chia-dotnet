using System;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Net.Security;
using System.Net.WebSockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;

namespace chia.dotnet.protocol;

public class Peer : IDisposable
{
    private readonly ClientWebSocket _webSocket = new();
    private readonly CancellationTokenSource _receiveCancellationTokenSource = new();
    private readonly ConcurrentDictionary<ushort, ProtocolMessage> _pendingRequests = new();
    private readonly ConcurrentDictionary<ushort, ProtocolMessage> _pendingResponses = new();
    private int _nonce;

    private bool disposedValue;

    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="endpoint">Details of the WebSocket endpoint</param>        
    public Peer(EndpointInfo endpoint)
    {
        Endpoint = endpoint ?? throw new ArgumentNullException(nameof(endpoint));

        _webSocket.Options.RemoteCertificateValidationCallback += ValidateServerCertificate;
    }

    /// <summary>
    /// Details of the RPC service endpoint
    /// </summary>
    public EndpointInfo Endpoint { get; init; }

    /// <summary>
    /// Opens the WebSocket and starts the receive loop
    /// </summary>
    /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
    /// <returns>An awaitable <see cref="Task"/></returns>
    public async Task Connect(CancellationToken cancellationToken = default)
    {
        ObjectDisposedException.ThrowIf(disposedValue, this);

        if (_webSocket.State is WebSocketState.Connecting or WebSocketState.Open)
        {
            throw new InvalidOperationException("Websocket connection is already open");
        }

        _webSocket.Options.ClientCertificates = Endpoint.GetCert();

        await _webSocket.ConnectAsync(Endpoint.Uri, cancellationToken).ConfigureAwait(false);
        _ = Task.Factory.StartNew(ReceiveLoop, _receiveCancellationTokenSource.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default).ConfigureAwait(false);
    }

    /// <summary>
    /// Cancels the receive loop and closes the WebSocket
    /// </summary>
    /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
    /// <returns>Awaitable <see cref="Task"/></returns>
    public async Task Close(CancellationToken cancellationToken = default)
    {
        ObjectDisposedException.ThrowIf(disposedValue, this);

        _receiveCancellationTokenSource.Cancel();
        await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "bye", cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Posts a <see cref="Message"/> to the WebSocket but does not wait for a response
    /// </summary>
    /// <param name="message">The message to post</param>
    /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
    /// <remarks>Awaiting this method waits for the message to be sent only. It doesn't await a response.</remarks>
    /// <returns>Awaitable <see cref="Task"/></returns>
    public virtual async Task PostMessage(ProtocolMessage message, CancellationToken cancellationToken = default)
    {
        ObjectDisposedException.ThrowIf(disposedValue, this);

        var json = message.ToJson();
        await _webSocket.SendAsync(Encoding.UTF8.GetBytes(json), WebSocketMessageType.Text, true, cancellationToken).ConfigureAwait(false);
    }

    /// <summary>
    /// Sends a <see cref="Message"/> to the endpoint and waits for a response
    /// </summary>
    /// <param name="message">The message to send</param>
    /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
    /// <remarks>Awaiting this method will block until a response is received from the <see cref="WebSocket"/> or the A token to allow the call to be cancelled is cancelled</remarks>
    /// <returns>The response message</returns>
    /// <exception cref="ResponseException">Throws when <see cref="Message.IsSuccessfulResponse"/> is False</exception>
    public async Task<byte[]> SendMessage(ProtocolMessage message, CancellationToken cancellationToken = default)
    {
        ObjectDisposedException.ThrowIf(disposedValue, this);

        // Atomically increment _nonce and assign it to message.Id
        message.Id = (ushort)Interlocked.Increment(ref _nonce);

        // capture the message to be sent
        if (!_pendingRequests.TryAdd(message.Id, message))
        {
            throw new InvalidOperationException($"A message with an id of {message.Id} has already been sent");
        }

        try
        {
            await PostMessage(message, cancellationToken).ConfigureAwait(false);
        }
        catch (Exception e)
        {
            Debug.WriteLine(e.Message);
            _ = _pendingRequests.TryRemove(message.Id, out _);
            throw;
        }

        // wait here until a response shows up or we get cancelled
        ProtocolMessage response;
        while (!_pendingResponses.TryRemove(message.Id, out response!) && !cancellationToken.IsCancellationRequested)
        {
            await Task.Delay(10, cancellationToken).ConfigureAwait(false);
        }

        // the receive loop cleans up but make sure we do so on cancellation too
        if (_pendingRequests.ContainsKey(message.Id))
        {
            _ = _pendingRequests.TryRemove(message.Id, out _);
        }

        return response?.Data ?? throw new ProtocolException(message, "The WebSocket did not respond");
    }

    /// <summary>
    /// Event raised when a message is received from the endpoint that was either not in response to a request
    /// or was a response from a posted message (i.e. we didn't register to receive the response)
    /// Pooling state_changed messages come through this event
    /// </summary>
    public event EventHandler<ProtocolMessage>? BroadcastMessageReceived;

    /// <summary>
    /// Raises the <see cref="BroadcastMessageReceived"/> event
    /// </summary>
    /// <param name="message">The message to broadcast</param>
    protected virtual void OnBroadcastMessageReceived(ProtocolMessage message)
    {
        BroadcastMessageReceived?.Invoke(this, message);
    }

    private async Task ReceiveLoop()
    {
        var buffer = new ArraySegment<byte>(new byte[2048]);
        do
        {
            using var ms = new MemoryStream();

            WebSocketReceiveResult result;
            do
            {
                result = await _webSocket.ReceiveAsync(buffer, _receiveCancellationTokenSource.Token).ConfigureAwait(false);
#pragma warning disable CS8604 // Possible null reference argument.
                ms.Write(buffer.Array, buffer.Offset, result.Count);
#pragma warning restore CS8604 // Possible null reference argument.
            } while (!result.EndOfMessage);

            if (result.MessageType == WebSocketMessageType.Close)
            {
                break;
            }

            _ = ms.Seek(0, SeekOrigin.Begin);
            using var reader = new StreamReader(ms, Encoding.UTF8);
            var response = await reader.ReadToEndAsync().ConfigureAwait(false);
            var message = response.ToObject<ProtocolMessage>() ?? new ProtocolMessage();

            // if we have a message pending with this id, capture the response and remove the request from the pending dictionary                
            if (_pendingRequests.TryRemove(message.Id, out _))
            {
                _pendingResponses[message.Id] = message;
            }
            else
            {
                OnBroadcastMessageReceived(message);
            }
        } while (!_receiveCancellationTokenSource.IsCancellationRequested);
    }

    private static bool ValidateServerCertificate(object sender, X509Certificate? certificate, X509Chain? chain, SslPolicyErrors sslPolicyErrors)
    {
        // uncomment these checks to change remote cert validation requirements

        // require remote ca to be trusted on this machine
        //if ((sslPolicyErrors & SslPolicyErrors.RemoteCertificateChainErrors) == SslPolicyErrors.RemoteCertificateChainErrors) 
        //    return false;

        // require server name to be validated in the cert
        //if ((sslPolicyErrors & SslPolicyErrors.RemoteCertificateNameMismatch) == SslPolicyErrors.RemoteCertificateNameMismatch)
        //    return false;

        return !((sslPolicyErrors & SslPolicyErrors.RemoteCertificateNotAvailable) == SslPolicyErrors.RemoteCertificateNotAvailable);
    }

    /// <summary>
    /// Called when the instance is being disposed or finalized
    /// </summary>
    /// <param name="disposing">Invoke from <see cref="IDisposable.Dispose"/></param>
    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                _receiveCancellationTokenSource.Cancel();
                _pendingRequests.Clear();
                _pendingResponses.Clear();
                _webSocket.Dispose();
                _receiveCancellationTokenSource.Dispose();
            }

            disposedValue = true;
        }
    }

    /// <summary>
    /// <see cref="IDisposable.Dispose"/>
    /// </summary>
    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    public async Task SendHandshake(string networkId, NodeType nodeType)
    {
        var handshake = new Handshake()
        {
            NetworkId = networkId,
            NodeType = nodeType,
            ProtocolVersion = "0.0.34",
            SoftwareVersion = "0.0.0",
            Capabilities = [
                (1, "1"),
                (2, "1"),
                (3, "1")
            ]
        };

    }
}
