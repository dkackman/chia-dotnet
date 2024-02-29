using System.Collections.Generic;

namespace chia.dotnet
{
    public record HashOnlyProof
    {
        public string KeyClvmHash { get; init; } = string.Empty;
        public string ValueClvmHash { get; init; } = string.Empty;
        public string NodeHash { get; init; } = string.Empty;
        public IEnumerable<Layer> layers { get; init; } = [];
    }
}
