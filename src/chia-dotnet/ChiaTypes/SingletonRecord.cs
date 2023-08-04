using System;
using Newtonsoft.Json;

namespace chia.dotnet
{
    public record SingletonInfo
    {
        public string Launcher { get; init; } = string.Empty;
        public string LauncRootherId { get; init; } = string.Empty;
    }

    public record SingletonRecord
    {
        public string CoinId { get; init; } = string.Empty;
        public string LauncherId { get; init; } = string.Empty;
        public string Root { get; init; } = string.Empty;
        public string InnerPuzzleHash { get; init; } = string.Empty;
        public bool Confirmed { get; init; }
        public uint ConfirmedAtHeight { get; init; }
        public LineageProof LineageProof { get; init; } = new();
        public int Generation { get; init; }
        public ulong Timestamp { get; init; }
        [JsonIgnore]
        public DateTime DateTimestamp => Timestamp.ToDateTime();
    }
}
