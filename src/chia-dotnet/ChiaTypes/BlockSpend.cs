using System;

namespace chia.dotnet
{
    public record BlockSpend
    {
        public string PuzzleReveal { get; init; } = string.Empty;
        public string Solution { get; init; } = string.Empty;
        public BlockCoinSpend Coin { get; init; } = new();
    }

    public record BlockCoinSpend
    {
        public Int64 Amount { get; init; }
        public string ParentCoinInfo { get; init; } = string.Empty;
        public string PuzzleHash { get; init; } = string.Empty;
    }
}
