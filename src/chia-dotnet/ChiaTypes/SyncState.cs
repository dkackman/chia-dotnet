namespace chia.dotnet
{
    public record SyncState
    {
        public bool SyncMode { get; init; }
        public ulong SyncProgressHeight { get; init; }
        public ulong SyncTipHeight { get; init; }
        public bool Synced { get; init; }
    }
}
