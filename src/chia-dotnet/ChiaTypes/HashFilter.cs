using System.Collections.Generic;

namespace chia.dotnet
{
    public record HashFilter
    {
        public IEnumerable<string> Values { get; init; } = [];

        public FilterMode Mode { get; init; } = FilterMode.Exlude;
    }
}
