using Newtonsoft.Json;
using System;
using System.Linq;
using System.Security.Cryptography;

namespace chia.dotnet
{
    /// <summary>
    /// This structure is used in the body for the reward and fees genesis coins.
    /// </summary>
    public record Coin
    {
        public string ParentCoinInfo { get; init; } = string.Empty;
        public string PuzzleHash { get; init; } = string.Empty;
        public ulong Amount { get; init; }
        [JsonIgnore]
        public string Name
        {
            get
            {
                // convert values to bytes
                var parent_id_bytes = Convert.FromHexString(ParentCoinInfo);
                var puzzle_hash_bytes = Convert.FromHexString(PuzzleHash);
                var amount_hex_string = Amount.ToString("X");
                // add leading zero if needed
                amount_hex_string = (amount_hex_string.Length % 2 == 0 ? "" : "0") + amount_hex_string;
                var amount_bytes = Convert.FromHexString(amount_hex_string);

                // concat all to one bytes array
                var bytes = parent_id_bytes.Concat(puzzle_hash_bytes).Concat(amount_bytes).ToArray();

                using var hash = SHA256.Create();
                var coin_id_bytes = hash.ComputeHash(bytes);

                return Convert.ToHexString(coin_id_bytes);
            }
        }
    }
}
