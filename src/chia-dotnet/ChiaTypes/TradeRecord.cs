using System;
using System.Collections.Generic;

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
        public SpendBundle SpendBundle { get; init; }
        /// <summary>
        /// this is full trade
        /// </summary>
        public SpendBundle TxSpendBundle { get; init; }
        public ICollection<Coin> Additions { get; init; }
        public ICollection<Coin> Removals { get; init; }
        public string TradeId { get; init; }
        /// <summary>
        /// TradeStatus, enum not streamable
        /// </summary>
        public string Status { get; init; }
        public ICollection<SendPeer> SentTo { get; init; }

        public DateTime? AcceptedAtDateTime => AcceptedAtTime.ToDateTime();
        public DateTime CreatedAtDateTime => CreatedAtTime.ToDateTime();
    }
}
