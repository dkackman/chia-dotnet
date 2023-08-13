namespace chia.dotnet
{
    /// <summary>
    /// This structure is used in the body for the reward and fees genesis coins.
    /// </summary>
    public record Coin
    {
        public string ParentCoinInfo { get; init; } = string.Empty;
        public string PuzzleHash { get; init; } = string.Empty;
        public ulong Amount { get; init; }
    }
}
