using System;
using System.Dynamic;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace chia.dotnet
{
    /// <summary>
    /// Class that handles core communication with the rpc endpoint using http(s)
    /// </summary>
    public class HttpRpcClient : IRpcClient
    {
        private readonly HttpClient _httpClient;

        private bool disposedValue;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="endpoint">Details of the service endpoint</param>        
        public HttpRpcClient(EndpointInfo endpoint)
        {
            Endpoint = endpoint ?? throw new ArgumentNullException(nameof(endpoint));

            var handler = new SocketsHttpHandler();
            handler.SslOptions.ClientCertificates = endpoint.GetCert();
            handler.SslOptions.RemoteCertificateValidationCallback += ValidateServerCertificate;

            _httpClient = new(handler, true)
            {
                BaseAddress = Endpoint.Uri
            };
        }

        /// <summary>
        /// Details of the RPC service endpoint
        /// </summary>
        public EndpointInfo Endpoint { get; init; }

        /// <summary>
        /// Posts a <see cref="Message"/> to the <see cref="Endpoint"/> but does not wait for a response
        /// </summary>
        /// <param name="message">The message to send</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <remarks>Awaiting this method waits for the message to be sent only. It doesn't await a response.</remarks>
        /// <returns>Awaitable <see cref="Task"/></returns>
        public async Task PostMessage(Message message, CancellationToken cancellationToken = default)
        {
            if (disposedValue)
            {
                throw new ObjectDisposedException(nameof(HttpRpcClient));
            }

            // need to use our json to ensure we get the snake case resolver
            // (don't change to extension method syntax as it won't bind to the dynamic 'Data' object)            
            var json = Converters.ToJson(message.Data);

            using var content = new StringContent(json, Encoding.UTF8, "application/json");
            using var response = await _httpClient.PostAsync(message.Command, content, cancellationToken).ConfigureAwait(false);
            _ = response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Sends a <see cref="Message"/> to the endpoint and waits for a response
        /// </summary>
        /// <param name="message">The message to send</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <remarks>Awaiting this method will block until a response is received from the rpc endpoint or the A token to allow the call to be cancelled is cancelled</remarks>
        /// <returns>The response message</returns>
        /// <exception cref="ResponseException">Throws when <see cref="Message.IsSuccessfulResponse"/> is False</exception>
        public async Task<dynamic> SendMessage(Message message, CancellationToken cancellationToken = default)
        {
            if (disposedValue)
            {
                throw new ObjectDisposedException(nameof(HttpRpcClient));
            }

            // need to use our json to ensure we get the snake case resolver
            // (don't change to extension method syntax as it won't bind to the dynamic 'Data' object)
            var json = Converters.ToJson(message.Data);

            using var content = new StringContent(json, Encoding.UTF8, "application/json");
            using var response = await _httpClient
                .PostAsync(message.Command, content, cancellationToken)
                .ConfigureAwait(false);
            using var responseContent = response
                .EnsureSuccessStatusCode()
                .Content;

            var responseJson = await responseContent
                .ReadAsStringAsync(cancellationToken)
                .ConfigureAwait(false);

            var responseData = responseJson.ToObject<dynamic>();

            return responseData?.success == false ? throw new ResponseException(message, responseData?.error?.ToString()) : responseData ?? new ExpandoObject();
        }

        private static bool ValidateServerCertificate(object sender, X509Certificate? certificate, X509Chain? chain, SslPolicyErrors sslPolicyErrors)
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
