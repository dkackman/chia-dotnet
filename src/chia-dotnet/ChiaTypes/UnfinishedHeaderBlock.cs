using System.Collections.Generic;

namespace chia.dotnet
{
    /// <summary>
    /// Same as a FullBlock but without TransactionInfo and Generator, used by light clients
    /// </summary>
    public record UnfinishedHeaderBlock
    {
        /// <summary>
        /// If first sb
        /// </summary>
        public IEnumerable<EndOfSubSlotBundle> FinishedSubSlots { get; init; } = [];
        /// <summary>
        /// Reward chain trunk data
        /// </summary>
        public RewardChainBlockUnfinished RewardChainBlock { get; init; } = new();
        /// <summary>
        /// If not first sp in sub-slot
        /// </summary>
        public VDFProof? ChallengeChainSpProof { get; init; }
        /// <summary>
        ///  If not first sp in sub-slot
        /// </summary>
        public VDFProof? RewardChainSpProof { get; init; }
        /// <summary>
        /// Reward chain foliage data
        /// </summary>
        public Foliage Foliage { get; init; } = new();
        /// <summary>
        /// Reward chain foliage data (tx block)
        /// </summary>
        public FoliageTransactionBlock? FoliageTransactionBlock { get; init; }
        /// <summary>
        /// Filter for block transactions
        /// </summary>
        public string TransactionsFilter { get; init; } = string.Empty;
    }
}
