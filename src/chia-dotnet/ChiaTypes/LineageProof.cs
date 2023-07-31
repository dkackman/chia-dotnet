namespace chia.dotnet
{
    public record LineageProof
    {
        public string ParentName { get; init; } = string.Empty;
        public string InnerPuzzleHash { get; init; } = string.Empty;
        public ulong Amount { get; init; }
    }
}
