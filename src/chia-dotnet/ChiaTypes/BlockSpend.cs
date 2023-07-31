using System;

namespace chia.dotnet;

public record BlockSpend
{
    public string Puzzle_Reveal { get; set; }
    public string Solution { get; set; }
    public BlockCoinSpend Coin { get; set; }
}

public record BlockCoinSpend
{
    public Int64 Amount { get; set; }
    public string Parent_Coin_Info { get; set; }
    public string Puzzle_Hash { get; set; }
}
