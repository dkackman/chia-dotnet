namespace chia.dotnet
{
    public record DataLayerOffer
    {
        public string TradeId { get; init; } = string.Empty;
        public string Offer { get; init; } = string.Empty;
        public StoreProofs Maker { get; init; } = new();
        public OfferStore Taker { get; init; } = new();
    }
}
