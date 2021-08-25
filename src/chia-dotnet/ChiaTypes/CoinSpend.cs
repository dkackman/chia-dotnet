namespace chia.dotnet
{
    /// <summary>
    /// This is a rather disparate data structure that validates coin transfers. It's generally populated
    /// with data from different sources, since burned coins are identified by name, so it is built up
    /// more often that it is streamed.
    /// </summary>
    public record CoinSpend
    {
        public Coin Coin { get; init; } = new();
        public string PuzzleReveal { get; init; } = string.Empty;
        public string Solution { get; init; } = string.Empty;
    }
}
