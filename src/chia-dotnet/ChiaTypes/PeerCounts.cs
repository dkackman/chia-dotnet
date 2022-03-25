using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace chia.dotnet
{
    public record PeerCounts
    {
        [JsonProperty("total_last_5_days")]
        public int TotalLast5Days { get; init; }
        public int ReliableNodes { get; init; }
        [JsonProperty("ipv4_last_5_days")]
        public int Ipv4Last5Days { get; init; }
        [JsonProperty("ipv6_last_5_days")]
        public int Ipv6Last5Days { get; init; }
        public IDictionary<string, int> Versions { get; init; } = new Dictionary<string, int>();
    }
}
