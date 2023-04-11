using System.Collections.Generic;

namespace chia.dotnet
{
    public record OfferSummary
    {
        public string Id { get; init; }
        public IDictionary<string, ulong> Offered { get; init; } = new Dictionary<string, ulong>();
        public IDictionary<string, ulong> Requested { get; init; } = new Dictionary<string, ulong>();
        public ulong Fees { get; init; } = 0;
        public IDictionary<string, object> Infos { get; init; } = new Dictionary<string, object>();
    }
}
