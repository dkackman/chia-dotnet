using System.Collections.Generic;

namespace chia.dotnet
{
    public record OfferSummary
    {
        public IDictionary<string, ulong> Offered { get; init; } = new Dictionary<string, ulong>();
        public IDictionary<string, ulong> Requested { get; init; } = new Dictionary<string, ulong>();
    }
}
