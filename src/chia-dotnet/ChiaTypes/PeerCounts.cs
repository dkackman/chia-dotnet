using System.Collections.Generic;

namespace chia.dotnet
{
    public record PeerCounts
    {
        public int TotalLast5Days { get; init; }
        public int ReliableNodes { get; init; }
        public int Ipv4Last5Days { get; init; }
        public int Ipv6Last5Days { get; init; }
        public IDictionary<string, int> Version { get; init; } = new Dictionary<string, int>();
    }
}
