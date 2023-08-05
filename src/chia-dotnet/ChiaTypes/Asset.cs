namespace chia.dotnet
{
    public record AssetInfo
    {
        public string Asset { get; init; } = string.Empty;
        public string Address { get; init; } = string.Empty;
        public ulong Amount { get; init; }
    }

    public record FungibleAsset
    {
        public string Asset { get; init; } = string.Empty;
        public ulong Amount { get; init; }
    }

    public record RoyaltyAsset
    {
        public string Asset { get; init; } = string.Empty;
        public string RoyaltyAddress { get; init; } = string.Empty;
        public ushort RoyaltyPercentage { get; init; }
    }
}
