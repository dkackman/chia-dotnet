using System.Collections.Generic;

namespace chia.dotnet
{
    public record TransactionTypeFilter
    {
        public IEnumerable<byte> Values { get; init; } = new List<byte>();

        public FilterMode Mode { get; init; } = FilterMode.Exlude;
    }
}
