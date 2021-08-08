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
        public VDFInfo challenge_chain_sp_vdf { get; init; }
        public string challenge_chain_sp_signature { get; init; }
        public VDFInfo reward_chain_sp_vdf { get; init; }
        public string reward_chain_sp_signature { get; init; }
    }

    public record EndOfSubSlotBundle
    {
        public ChallengeChainSubSlot challenge_chain { get; init; }
        public InfusedChallengeChainSubSlot infused_challenge_chain { get; init; }
        public RewardChainSubSlot reward_chain { get; init; }
        public SubSlotProofs proofs { get; init; }
    }

    public record UnfinishedHeaderBlock
    {
        public IEnumerable<EndOfSubSlotBundle> FinishedSubSlots { get; init; }
        public RewardChainBlockUnfinished RewardChainBlock { get; init; }
        public VDFProof ChallengeChainSpProof { get; init; }
        public VDFProof RewardChainSpProof { get; init; }
        public Foliage Foliage { get; init; }
        public FoliageTransactionBlock FoliageTransactionBlock { get; init; }
        public string TransactionsFilter { get; init; }
    }
}
