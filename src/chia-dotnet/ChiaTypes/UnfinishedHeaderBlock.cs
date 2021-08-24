using System.Collections.Generic;
using System.Numerics;

namespace chia.dotnet
{
    public record RewardChainBlockUnfinished
    {
        public BigInteger TotalIters { get; init; }
        public byte SignagePointIndex { get; init; }
        public string PosSsCcChallengeHash { get; init; } = string.Empty;
        public ProofOfSpace ProofOfSpace { get; init; } = new();
        /// <summary>
        /// Not present for first sp in slot
        /// </summary>
        public VDFInfo? ChallengeChainSpVdf { get; init; }
        public string ChallengeChainSpSignature { get; init; } = string.Empty;
        /// <summary>
        /// Not present for first sp in slot
        /// </summary>
        public VDFInfo? RewardChainSpVdf { get; init; }
        public string RewardChainSpSignature { get; init; } = string.Empty;
    }

    public record EndOfSubSlotBundle
    {
        public ChallengeChainSubSlot ChallengeChain { get; init; } = new();
        public InfusedChallengeChainSubSlot? InfusedChallengeChain { get; init; }
        public RewardChainSubSlot RewardChain { get; init; } = new();
        public SubSlotProofs Proofs { get; init; } = new();
    }

    /// <summary>
    /// Same as a FullBlock but without TransactionInfo and Generator, used by light clients
    /// </summary>
    public record UnfinishedHeaderBlock
    {
        /// <summary>
        /// If first sb
        /// </summary>
        public ICollection<EndOfSubSlotBundle> FinishedSubSlots { get; init; } = new List<EndOfSubSlotBundle>();
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
