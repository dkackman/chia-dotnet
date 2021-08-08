using System.Collections.Generic;

namespace chia.dotnet
{
    public record TradeRecord
    {
        public uint ConfirmedAtIndex { get; init; }
        public ulong? AcceptedAtTime { get; init; }
        public ulong CreatedAtTime { get; init; }
        public bool MyOffer { get; init; }
        public uint Sent { get; init; }
        public SpendBundle SpendBundle { get; init; }
        public SpendBundle TxSpendBundle { get; init; }
        public ICollection<Coin> Additions { get; init; }
        public ICollection<Coin> Removals { get; init; }
        public string TradeId { get; init; }
        public string Status { get; init; }
        public ICollection<SendPeer> SentTo { get; init; }
    }
}
