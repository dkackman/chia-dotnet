using Newtonsoft.Json;

namespace chia.dotnet
{
    public record SyncState
    {
        public bool SyncMode { get; init; }
        public uint SyncProgressHeight { get; init; }
        public uint SyncTipHeight { get; init; }
        public bool Synced { get; init; }
        [JsonIgnore]
        public double SyncProgressPercent => SyncTipHeight > 0 ? (double)SyncProgressHeight / SyncTipHeight : 0;
    }
}
