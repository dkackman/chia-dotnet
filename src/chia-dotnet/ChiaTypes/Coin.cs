using Newtonsoft.Json;
using chia.dotnet.bech32;

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
        public string AmountHex
        {
            get
            {
                var amountHexString = Amount.ToString("X");
                return (amountHexString.Length % 2 == 0 ? "" : "0") + amountHexString;
            }
        }

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
