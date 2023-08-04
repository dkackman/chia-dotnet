using System;
using Newtonsoft.Json;

namespace chia.dotnet
{
    public record Root
    {
        public string Hash { get; init; } = string.Empty;
        public bool Confirmed { get; init; }
        public ulong Timestamp { get; init; }
        [JsonIgnore]
        public DateTime DateTimestamp => Timestamp.ToDateTime();
    }

    public record RootHistory
    {
        public string RootHash { get; init; } = string.Empty;
        public bool Confirmed { get; init; }
        public ulong Timestamp { get; init; }
        [JsonIgnore]
        public DateTime DateTimestamp => Timestamp.ToDateTime();
    }

    public record RootHash
    {
        public string Id { get; init; } = string.Empty;
        public string Hash { get; init; } = string.Empty;
        public bool Confirmed { get; init; }
        public ulong Timestamp { get; init; }
        [JsonIgnore]
        public DateTime DateTimestamp => Timestamp.ToDateTime();
    }
}
