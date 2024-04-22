namespace chia.dotnet.protocol;

public record CoinState
{
    public Coin Coin { get; init; } = new();
    public uint? SpentHeight { get; init; }
    public uint? CreatedHeight { get; init; }
}
