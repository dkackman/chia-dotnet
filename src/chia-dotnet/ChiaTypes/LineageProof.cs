namespace chia.dotnet
{
    public record LineageProof
    {
        public string? ParentName { get; init; }
        public string? InnerPuzzleHash { get; init; }
        public ulong? Amount { get; init; }
    }
}
