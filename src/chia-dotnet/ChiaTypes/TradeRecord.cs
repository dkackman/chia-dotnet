using System;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace chia.dotnet
{
    /// <summary>
    /// Used for storing transaction data and status in wallets.
    /// </summary>
    public record TradeRecord
    {
        public uint ConfirmedAtIndex { get; init; }
        public ulong? AcceptedAtTime { get; init; }
        public ulong CreatedAtTime { get; init; }
        public bool MyOffer { get; init; }
        public uint Sent { get; init; }
        /// <summary>
        /// This in not complete spendbundle
        /// </summary>
        public SpendBundle SpendBundle { get; init; } = new();
        /// <summary>
        /// this is full trade
        /// </summary>
        public SpendBundle? TxSpendBundle { get; init; }
        public ICollection<Coin> Additions { get; init; } = new List<Coin>();
        public ICollection<Coin> Removals { get; init; } = new List<Coin>();
        public string TradeId { get; init; } = string.Empty;
        /// <summary>
        /// TradeStatus, enum not streamable
        /// </summary>
        public uint Status { get; init; }
        [JsonIgnore]
        public TradeStatus TradeStatus => (TradeStatus)Status;
        public ICollection<SendPeer> SentTo { get; init; } = new List<SendPeer>();
        [JsonIgnore]
        public DateTime? AcceptedAtDateTime => AcceptedAtTime.ToDateTime();
        [JsonIgnore]
        public DateTime CreatedAtDateTime => CreatedAtTime.ToDateTime();
    }
}
