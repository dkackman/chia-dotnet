using System.Collections.Generic;

namespace chia.dotnet
{
    /// <summary>
    /// Information that goes along with each transaction block
    /// </summary>
    public record TransactionsInfo
    {
        /// <summary>
        /// sha256 of the block generator in this block
        /// </summary>
        public string GeneratorRoot { get; init; } = string.Empty;
        /// <summary>
        /// sha256 of the concatenation of the generator ref list entries
        /// </summary>
        public string GeneratorRefsRoot { get; init; } = string.Empty;
        public string AggregatedSignature { get; init; } = string.Empty;
        /// <summary>
        /// This only includes user fees, not block rewards
        /// </summary>
        public ulong Fees { get; init; }
        /// <summary>
        /// This is the total cost of this block, including CLVM cost, cost of program size and conditions
        /// </summary>
        public ulong Cost { get; init; }
        /// <summary>
        /// These can be in any order
        /// </summary>
        public IEnumerable<Coin> RewardClaimsIncorporated { get; init; } = [];
    }
}
