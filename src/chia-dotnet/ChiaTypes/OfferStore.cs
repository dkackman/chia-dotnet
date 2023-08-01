using System.Collections.Generic;

namespace chia.dotnet
{
    public record OfferStore
    {
        public string StoreID { get; init; } = string.Empty;

        public IEnumerable<KeyValuePair<string, string>> Inclusions { get; init; } = new Dictionary<string, string>();
    }
}
