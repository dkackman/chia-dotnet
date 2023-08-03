using System.Collections.Generic;

namespace chia.dotnet
{
    public record AmountWithPuzzlehash
    {
        public IEnumerable<TransactionType> Values { get; init; } = new List<TransactionType>();

        public FilterMode Mode { get; init; } = FilterMode.Exlude;
    }
}
