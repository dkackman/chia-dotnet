namespace chia.dotnet
{
    /// <summary>
    /// This structure is used in the body for the reward and fees genesis coins.
    /// </summary>
    public record Coin
    {
        public string ParentCoinInfo { get; init; }
        public string PuzzleHash { get; init; }
        public ulong Amount { get; init; }
    }
}
