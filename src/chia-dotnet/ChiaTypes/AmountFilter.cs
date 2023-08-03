using System.Collections.Generic;

namespace chia.dotnet
{
    public record AmountFilter
    {
        public IEnumerable<ulong> Values { get; init; } = new List<ulong>();

        public FilterMode Mode { get; init; } = FilterMode.Exlude;
    }
}
