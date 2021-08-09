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
        public VDFInfo ChallengeChainSpVdf { get; init; }
        public string ChallengeChainSpSignature { get; init; }
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
        public ICollection<EndOfSubSlotBundle> FinishedSubSlots { get; init; }
        public RewardChainBlockUnfinished RewardChainBlock { get; init; }
        public VDFProof ChallengeChainSpProof { get; init; }
        public VDFProof RewardChainSpProof { get; init; }
        public Foliage Foliage { get; init; }
        public FoliageTransactionBlock FoliageTransactionBlock { get; init; }
        public string TransactionsFilter { get; init; }
    }
}
