using System.Collections.Generic;
using Newtonsoft.Json;

namespace chia.dotnet
{
    public record Spend
    {
        public string CoinId { get; init; } = string.Empty;
        public string PuzzleHash { get; init; } = string.Empty;
        public uint? HeightRelative { get; init; }
        public uint? SecondsRelative { get; init; }
        public uint? BeforeHeightRelative { get; init; }
        public uint? BeforeSecondsRelative { get; init; }
        public uint? BirthHeight { get; init; }
        public uint? BirthSeconds { get; init; }
        public int Flags { get; init; }

        [JsonConverter(typeof(CoinConverter))]
        public IEnumerable<Coin> CreateCoin { get; init; } = [];

        [JsonConverter(typeof(NameValuePairConverter))]
        public IEnumerable<NameValuePair> AggSigMe { get; init; } = [];
    }
}
