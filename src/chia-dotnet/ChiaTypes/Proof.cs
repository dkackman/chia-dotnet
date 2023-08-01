using System.Collections.Generic;

namespace chia.dotnet
{
    public record Proof
    {
        public string Key { get; init; } = string.Empty;
        public string Value { get; init; } = string.Empty;
        public string NodeHash { get; init; } = string.Empty;
        public IEnumerable<Layer> Layers { get; init; } = new List<Layer>();
    }
}
