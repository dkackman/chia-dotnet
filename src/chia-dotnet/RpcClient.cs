using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.WebSockets;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("chia-dotnet.tests")]

namespace chia.dotnet
{
    public class RpcClient : IDisposable
    {
        private readonly ClientWebSocket _webSocket = new();
        private readonly CancellationTokenSource _receiveCancellationTokenSource = new();
        private readonly ConcurrentDictionary<string, Message> _pendingMessages = new();
        private readonly ConcurrentDictionary<string, Message> _pendingResponses = new();
        private readonly EndpointInfo _endpoint;

        private bool disposedValue;

        public RpcClient(EndpointInfo endpoint, string serviceName)
        {
            _endpoint = endpoint ?? throw new ArgumentNullException(nameof(endpoint));

            if (string.IsNullOrEmpty(value: serviceName))
            {
                throw new ArgumentNullException(nameof(serviceName));
            }

            ServiceName = serviceName;

            _webSocket.Options.RemoteCertificateValidationCallback += ValidateServerCertificate;
        }

        public string ServiceName { get; private set; }

        public async Task Connect(CancellationToken cancellationToken)
        {
            _webSocket.Options.ClientCertificates = CertLoader.GetCerts(_endpoint.CertPath, _endpoint.KeyPath);

            await _webSocket.ConnectAsync(_endpoint.Uri, cancellationToken);
            _ = Task.Factory.StartNew(ReceiveLoop, _receiveCancellationTokenSource.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
        }

        public async Task Close(CancellationToken cancellationToken)
        {
            _receiveCancellationTokenSource.Cancel();
            await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "bye", cancellationToken);
        }

        /// <summary>
        /// Posts a <see cref="Message"/> to the websocket but does not wait for a response
        /// </summary>
        /// <param name="message">The <see cref="Message"/> to post</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>A <see cref="Task"/></returns>
        public async Task PostMessage(Message message, CancellationToken cancellationToken)
        {
            var json = message.ToJson();
            await _webSocket.SendAsync(Encoding.UTF8.GetBytes(json), WebSocketMessageType.Text, true, cancellationToken);
        }

        /// <summary>
        /// Sends a <see cref="Message"/> to the websocket but and waits for a response
        /// </summary>
        /// <param name="message">The <see cref="Message"/> to post</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>The response message</returns>
        public async Task<Message> SendMessage(Message message, CancellationToken cancellationToken)
        {
            // capture the message to be sent
            if (!_pendingMessages.TryAdd(message.Request_Id, message))
            {
                throw new InvalidOperationException($"Message with id of {message.Request_Id} has already been sent");
            }

            try
            {
                var json = message.ToJson();
                await _webSocket.SendAsync(Encoding.UTF8.GetBytes(json), WebSocketMessageType.Text, true, cancellationToken);
            }
            catch
            {
                _ = _pendingMessages.TryRemove(message.Request_Id, out _);
                throw;
            }

            // wait here until a response shows up or we get cancelled
            Message response;
            while (!_pendingResponses.TryRemove(message.Request_Id, out response) && !cancellationToken.IsCancellationRequested)
            {
                await Task.Yield();
            }

            // the receive loop cleans up but make sure we do so on cancellation too
            if (_pendingMessages.ContainsKey(message.Request_Id))
            {
                _ = _pendingMessages.TryRemove(message.Request_Id, out _);
            }

            return response;
        }

        /// <summary>
        /// Event rasied when a message is received from the endpoint that was either not in response to a send
        /// or was a response from a posted message (i.e. we didn't register to receive the response
        /// </summary>
        public event EventHandler<Message> BroadcastMessageReceived;

        protected virtual void OnBroadcastMessageReceived(Message message)
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
                    result = await _webSocket.ReceiveAsync(buffer, _receiveCancellationTokenSource.Token);
                    ms.Write(buffer.Array, buffer.Offset, result.Count);
                } while (!result.EndOfMessage);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    break;
                }

                _ = ms.Seek(0, SeekOrigin.Begin);
                using var reader = new StreamReader(ms, Encoding.UTF8);
                var response = await reader.ReadToEndAsync();
                var message = Message.FromJson(response);

                // if we have a message pending with this id capture the response and remove it from the pending dictionary                
                if (_pendingMessages.TryRemove(message.Request_Id, out _))
                {
                    _pendingResponses[message.Request_Id] = message;
                }
                else
                {
                    OnBroadcastMessageReceived(message);
                }
            } while (!_receiveCancellationTokenSource.IsCancellationRequested);
        }

        private static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            // uncomment these checks to change remote cert validaiton requirements

            // require remote ca to be trusted on this machine
            //if ((sslPolicyErrors & SslPolicyErrors.RemoteCertificateChainErrors) == SslPolicyErrors.RemoteCertificateChainErrors) 
            //    return false;

            // require server name to be validated in the cert
            //if ((sslPolicyErrors & SslPolicyErrors.RemoteCertificateNameMismatch) == SslPolicyErrors.RemoteCertificateNameMismatch)
            //    return false;

            return !((sslPolicyErrors & SslPolicyErrors.RemoteCertificateNotAvailable) == SslPolicyErrors.RemoteCertificateNotAvailable);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _receiveCancellationTokenSource.Cancel();
                    _webSocket.Dispose();
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
    }
}
