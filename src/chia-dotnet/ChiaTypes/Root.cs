using System;
using Newtonsoft.Json;

namespace chia.dotnet
{
    public enum Status
    {
        PENDING = 1,
        COMMITTED = 2
    }

    public record Root
    {
        public string TreeId { get; init; } = string.Empty;
        public string? NodeHash { get; init; }
        public uint Generation { get; init; }
        public Status Status { get; init; }
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
        public string? Id { get; init; }
        public string Hash { get; init; } = string.Empty;
        public bool Confirmed { get; init; }
        public ulong Timestamp { get; init; }
        [JsonIgnore]
        public DateTime DateTimestamp => Timestamp.ToDateTime();
    }
}
