using System.Collections.Generic;

namespace chia.dotnet
{
    /// <summary> 
    /// This is a list of coins being spent along with their solution programs, and a single
    /// aggregated signature. This is the object that most closely corresponds to a bitcoin
    /// transaction (although because of non-interactive signature aggregation, the boundaries
    /// between transactions are more flexible than in bitcoin). 
    /// </summary>
    public record SpendBundle
    {
        public string AggregatedSignature { get; init; } = string.Empty;
        public IEnumerable<CoinSpend> CoinSpends { get; init; } = [];
    }
}
