using System.Numerics;
using System.Collections.Generic;

namespace chia.dotnet
{
    public record RewardChainBlockUnfinished
    {
        public BigInteger TotalIters { get; init; }
        public byte SignagePointIndex { get; init; }
        public string PosSsCcChallengeHash { get; init; }
        public ProofOfSpace ProofOfSpace { get; init; }
        /// <summary>
        /// Not present for first sp in slot
        /// </summary>
        public VDFInfo ChallengeChainSpVdf { get; init; }
        public string ChallengeChainSpSignature { get; init; }
        /// <summary>
        /// Not present for first sp in slot
        /// </summary>
        public VDFInfo RewardChainSpVdf { get; init; }
        public string RewardChainSpSignature { get; init; }
    }

    public record EndOfSubSlotBundle
    {
        public ChallengeChainSubSlot ChallengeChain { get; init; }
        public InfusedChallengeChainSubSlot InfusedChallengeChain { get; init; }
        public RewardChainSubSlot RewardChain { get; init; }
        public SubSlotProofs Proofs { get; init; }
    }

    /// <summary>
    /// Same as a FullBlock but without TransactionInfo and Generator, used by light clients
    /// </summary>
    public record UnfinishedHeaderBlock
    {
        /// <summary>
        /// If first sb
        /// </summary>
        public ICollection<EndOfSubSlotBundle> FinishedSubSlots { get; init; }
        /// <summary>
        /// Reward chain trunk data
        /// </summary>
        public RewardChainBlockUnfinished RewardChainBlock { get; init; }
        /// <summary>
        /// If not first sp in sub-slot
        /// </summary>
        public VDFProof ChallengeChainSpProof { get; init; }
        /// <summary>
        ///  If not first sp in sub-slot
        /// </summary>
        public VDFProof RewardChainSpProof { get; init; }
        /// <summary>
        /// Reward chain foliage data
        /// </summary>
        public Foliage Foliage { get; init; }
        /// <summary>
        /// Reward chain foliage data (tx block)
        /// </summary>
        public FoliageTransactionBlock FoliageTransactionBlock { get; init; }
        /// <summary>
        /// Filter for block transactions
        /// </summary>
        public string TransactionsFilter { get; init; }
    }
}
