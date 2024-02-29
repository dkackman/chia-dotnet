using System.Collections.Generic;

namespace chia.dotnet
{
    public record ProofResultInclusions
    {
        public string StoreId { get; init; } = string.Empty;
        public IEnumerable<KeyValueHashes> Inclusions { get; init; } = [];
    }

}
