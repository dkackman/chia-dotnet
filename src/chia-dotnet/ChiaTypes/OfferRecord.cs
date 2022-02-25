namespace chia.dotnet
{
    /// <summary>
    /// The in memory representation of an offer and its record of trade
    /// </summary>
    public record OfferRecord
    {
        /// <summary>
        /// The bech32 encoded value of the offer
        /// </summary>
        public string Offer { get; init; } = string.Empty;

        /// <summary>
        /// Trade record assocaited with the offer
        /// </summary>
        public TradeRecord TradeRecord { get; init; } = new TradeRecord();
    }
}
