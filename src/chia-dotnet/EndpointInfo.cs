using System;

namespace chia.dotnet
{
    /// <summary>
    /// Information about how to connect and authenticate with the RPC endpoint
    /// </summary>
    public record EndpointInfo
    {
        /// <summary>
        /// The secure websocket (wss) Uri of the RPC endpoint
        /// </summary>
        public Uri Uri { get; init; } = new("http://localhost");

        /// <summary>
        /// The full filesystem path to the public certificate used to authenticate with the Websocket (.crt)
        /// </summary>
        public string CertPath { get; init; } = string.Empty;

        /// <summary>
        /// The full filesystem path to the base64 encoded RSA private key to authneticate with the websocket (.key)
        /// </summary>
        public string KeyPath { get; init; } = string.Empty;
    }
}
