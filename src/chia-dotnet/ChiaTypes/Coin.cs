using Newtonsoft.Json;
using System;
using System.Security.Cryptography;
using System.Text;

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
                var inputString = ParentCoinInfo + PuzzleHash + Amount.ToString();
                var inputBytes = Encoding.UTF8.GetBytes(inputString);
                using var sha256 = SHA256.Create();
                // Compute the hash value from the input bytes
                var hashBytes = sha256.ComputeHash(inputBytes);

                // Convert the hash bytes to a hexadecimal string
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }
    }
}
