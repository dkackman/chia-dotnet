using System;

namespace chia.dotnet
{
    /// <summary>
    /// These are values that correspond to a CoinName that are used
    /// in keeping track of the unspent database.
    /// </summary>
    public record CoinRecord
    {
        public Coin Coin { get; init; }
        public uint ConfirmedBlockIndex { get; init; }
        public uint SpentBlockIndex { get; init; }
        public bool Spent { get; init; }
        public bool Coinbase { get; init; }
        public ulong Timestamp { get; init; }

        public DateTime? DateTimestamp => Timestamp.ToDateTime();
    }
}
