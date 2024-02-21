using System.Collections.Generic;

namespace chia.dotnet
{
    public record AmountFilter
    {
        public IEnumerable<ulong> Values { get; init; } = [];

        public FilterMode Mode { get; init; } = FilterMode.Exlude;
    }
}
