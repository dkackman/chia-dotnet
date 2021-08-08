using System;
using System.Numerics;
using System.Collections.Generic;

namespace chia.dotnet
{
    /*
    This structure is used in the body for the reward and fees genesis coins.
    */
    public record Coin
    {
        public string ParentCoinInfo { get; init; }
        public string PuzzleHash { get; init; }
        public ulong Amount { get; init; }
    }

    /*
    These are values that correspond to a CoinName that are used
    in keeping track of the unspent database.
    */
    public record CoinRecord
    {
        public Coin Coin { get; init; }
        public uint ConfirmedBlockIndex { get; init; }
        public uint SpentBlockIndex { get; init; }
        public bool Spent { get; init; }
        public bool Coinbase { get; init; }
        public ulong Timestamp { get; init; }
    }
}
