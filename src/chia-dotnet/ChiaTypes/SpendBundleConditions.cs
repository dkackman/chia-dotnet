using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace chia.dotnet
{
    public record SpendBundleConditions
    {
        public IEnumerable<Spend> Spends { get; init; } = [];
        public ulong ReserveFee { get; init; }
        public uint HeightAbsolute { get; init; }
        public uint SecondsAbsolute { get; init; }
        public uint? BeforeHeightAbsolute { get; init; }
        public uint? BeforeSecondsAbsolute { get; init; }

        [JsonConverter(typeof(NameValuePairConverter))]
        public IEnumerable<NameValuePair> AggSigUnsafe { get; init; } = [];
        public ulong Cost { get; init; }
        public ulong RemovalAmount { get; init; }
        public ulong AdditionAmount { get; init; }
    }

}
