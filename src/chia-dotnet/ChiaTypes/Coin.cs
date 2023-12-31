using chia.dotnet.bech32;
using Newtonsoft.Json;
using System.Numerics;

namespace chia.dotnet
{
    /// <summary>
    /// This structure is used in the body for the reward and fees genesis coins.
    /// </summary>
    public record Coin
    {
        public string ParentCoinInfo { get; init; } = string.Empty;
        public string PuzzleHash { get; init; } = string.Empty;
        public BigInteger Amount { get; init; }

        /// <summary>
        /// The <see cref="Amount"/> as a hex string
        /// </summary>
        [JsonIgnore]
        public string AmountHex
        {
            get
            {
                var amountHexString = Amount.ToString("X");
                return (amountHexString.Length % 2 == 0 ? "" : "0") + amountHexString;
            }
        }

        /// <summary>
        /// SHA256 hash of <see cref="ParentCoinInfo"/>, <see cref="PuzzleHash"/>, and <see cref="AmountHex"/>
        /// </summary>
        [JsonIgnore]
        public string Name
        {
            get
            {
                var theBytes = HexBytes.FromHex(ParentCoinInfo) + HexBytes.FromHex(PuzzleHash) + HexBytes.FromHex(AmountHex);
                return theBytes.Sha256().ToString().ToUpperInvariant();
            }
        }
    }
}
