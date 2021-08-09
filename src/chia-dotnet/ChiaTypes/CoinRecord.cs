using System;

namespace chia.dotnet
{
    /// <summary>
    /// This structure is used in the body for the reward and fees genesis coins.
    /// </summary>
    public record Coin
    {
        public string ParentCoinInfo { get; init; }
        public string PuzzleHash { get; init; }
        public ulong Amount { get; init; }
    }

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
