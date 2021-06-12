using System;

namespace chia.dotnet
{
    public record EndpointInfo
    {
        public Uri Uri { get; init; }
        public string CertPath { get; init; }
        public string KeyPath { get; init; }
    }
}
