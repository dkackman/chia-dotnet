using System;
using System.Net.Security;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace chia.dotnet
{
    /// <summary>
    /// Base class that handles core communication with the rpc endpoint using http(s)
    /// and synchronizes request and response messages
    /// </summary>
    public class HttpRpcClient : IDisposable, IRpcClient
    {
        private readonly SocketsHttpHandler _httpHandler = new();
        private readonly HttpClient _httpClient;

        private bool disposedValue;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="endpoint">Details of the websocket endpoint</param>        
        public HttpRpcClient(EndpointInfo endpoint)
        {
            Endpoint = endpoint ?? throw new ArgumentNullException(nameof(endpoint));

            _httpHandler.SslOptions.ClientCertificates = CertLoader.GetCerts(Endpoint.CertPath, Endpoint.KeyPath);
            _httpHandler.SslOptions.RemoteCertificateValidationCallback += ValidateServerCertificate;

            _httpClient = new(_httpHandler);
            _httpClient.BaseAddress = Endpoint.Uri;
        }

        /// <summary>
        /// Details of the RPC service endpoint
        /// </summary>
        public EndpointInfo Endpoint { get; init; }

        /// <summary>
        /// Posts a <see cref="Message"/> to the websocket but does not wait for a response
        /// </summary>
        /// <param name="message">The message to send</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <remarks>Awaiting this method waits for the message to be sent only. It doesn't await a response.</remarks>
        /// <returns>Awaitable <see cref="Task"/></returns>
        public async Task PostMessage(Message message, CancellationToken cancellationToken = default)
        {
            if (disposedValue)
            {
                throw new ObjectDisposedException(nameof(WebSocketRpcClient));
            }

            var d = message.Data as IDictionary<string, object>;
            using var response = await _httpClient.PostAsJsonAsync(message.Command, d, cancellationToken);
            _ = response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Sends a <see cref="Message"/> to the endpoint and waits for a response
        /// </summary>
        /// <param name="message">The message to send</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <remarks>Awaiting this method will block until a response is received from the rpc endpoint or the <see cref="CancellationToken"/> is cancelled</remarks>
        /// <returns>The response message</returns>
        /// <exception cref="ResponseException">Throws when <see cref="Message.IsSuccessfulResponse"/> is False</exception>
        public async Task<dynamic> SendMessage(Message message, CancellationToken cancellationToken = default)
        {
            if (disposedValue)
            {
                throw new ObjectDisposedException(nameof(WebSocketRpcClient));
            }

            var d = message.Data as IDictionary<string, object>;
            using var response = await _httpClient.PostAsJsonAsync(message.Command, d, cancellationToken);
            _ = response.EnsureSuccessStatusCode();

            dynamic responseMessage = await response.Deserialize<dynamic>();
            return responseMessage?.success == false ? throw new ResponseException(message, responseMessage?.error?.ToString()) : (dynamic)responseMessage;
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
                    _httpClient.Dispose();
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
