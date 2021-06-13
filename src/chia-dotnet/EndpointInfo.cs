using System;

namespace chia.dotnet
{
    /// <summary>
    /// Information about how to connect and authenticate with the RPC endpoint
    /// </summary>
    public record EndpointInfo
    {
        public Uri Uri { get; init; }
        public string CertPath { get; init; }
        public string KeyPath { get; init; }
    }
}
