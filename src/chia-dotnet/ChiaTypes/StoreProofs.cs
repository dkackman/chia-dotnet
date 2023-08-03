using System.Collections.Generic;

namespace chia.dotnet
{
    public record StoreProofs
    {
        public string StoreId { get; init; } = string.Empty;
        public IEnumerable<Proof> Proofs { get; init; } = new List<Proof>();
    }
}
