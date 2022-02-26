using System;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace chia.dotnet
{
    // TODO - look in trade_record.py and figure out how these get serialized.
    /*
             formatted["summary"] = {
            "offered": offered,
            "requested": requested,
        }
        formatted["pending"] = offer.get_pending_amounts()
     */
    /// <summary>
    /// Used for storing transaction data and status in wallets.
    /// </summary>
    public record TradeRecord
    {
        public uint ConfirmedAtIndex { get; init; }
        public ulong? AcceptedAtTime { get; init; }
        public ulong CreatedAtTime { get; init; }
        public bool IsMyOffer { get; init; }
        public uint Sent { get; init; }
        /// <summary>
        /// Bech32 encoded value of the offer
        /// </summary>
        public string Offer { get; init; } = string.Empty;
        public string? TakenOffer { get; init; }
        public ICollection<Coin> CoinsOfInterest { get; init; } = new List<Coin>();
        public string TradeId { get; init; } = string.Empty;
        public TradeStatus Status { get; init; }
        public ICollection<SendPeer> SentTo { get; init; } = new List<SendPeer>();
        [JsonIgnore]
        public DateTime? AcceptedAtDateTime => AcceptedAtTime.ToDateTime();
        [JsonIgnore]
        public DateTime CreatedAtDateTime => CreatedAtTime.ToDateTime();
    }
}
