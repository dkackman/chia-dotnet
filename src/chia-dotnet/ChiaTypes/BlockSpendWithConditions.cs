using System.Collections.Generic;

namespace chia.dotnet
{
    public record BlockSpendWithConditions
    {
        public CoinSpend CoinSpend { get; init; } = new();
        public IEnumerable<ConditionWithArgs> Conditions { get; init; } = new List<ConditionWithArgs>();
    }
}
