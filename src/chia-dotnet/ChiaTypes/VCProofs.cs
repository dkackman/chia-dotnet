using System.Collections.Generic;

namespace chia.dotnet
{
    public record VCProofs
    {
        public IDictionary<string, string> KeyValuePairs { get; init; } = new Dictionary<string, string>();
    }
}
