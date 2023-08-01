namespace chia.dotnet
{
    public record TransactionTypeFilter
    {
        public string AssetId { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
        public uint FirstSeenHeight { get; init; }
        public string SenderPuzzleHash { get; init; } = string.Empty;
    }
}
