using System;

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
        /// Trade record associated with the offer
        /// </summary>
        public TradeRecord Trade { get; init; } = new TradeRecord();

        /// <summary>
        /// Trade record associated with the offer
        /// </summary>
        /// <remarks>Deprecated. Use <see cref="Trade"/></remarks>
        [Obsolete("This property is obsolete. Use Trade instead.", false)]
        public TradeRecord TradeRecord => Trade;
    }
}
