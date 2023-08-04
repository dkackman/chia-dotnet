namespace chia.dotnet
{
    public record DataLayerSyncStatus
    {
        public string RootHash { get; init; } = string.Empty;
        public int Generation { get; init; }
        public string TargetRootHash { get; init; } = string.Empty;
        public int TargetGeneration { get; init; }

    }
}
