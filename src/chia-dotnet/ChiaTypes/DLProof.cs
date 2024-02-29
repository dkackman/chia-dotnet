namespace chia.dotnet
{
    public record DLProof
    {
        public string CoinId { get; init; } = string.Empty;
        public string InnerPuzzleHash { get; init; } = string.Empty;
        public StoreProofs StoreProofs { get; init; } = new();
    }
}
