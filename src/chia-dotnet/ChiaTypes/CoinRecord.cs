using System;

using Newtonsoft.Json;

namespace chia.dotnet
{
    /// <summary>
    /// These are values that correspond to a CoinName that are used
    /// in keeping track of the unspent database.
    /// </summary>
    public record CoinRecord
    {
        public Coin Coin { get; init; } = new();
        public uint ConfirmedBlockIndex { get; init; }
        public uint SpentBlockIndex { get; init; }
        public bool Spent { get; init; }
        public bool Coinbase { get; init; }
        /// <summary>
        /// Timestamp of the block at height confirmed_block_index
        /// </summary>
        public ulong Timestamp { get; init; }
        /// <summary>
        /// Timestamp of the block at height confirmed_block_index
        /// </summary>
        [JsonIgnore]
        public DateTime? DateTimestamp => Timestamp.ToDateTime();
    }
}
