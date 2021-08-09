namespace chia.dotnet
{
    public record EndOfSlotBundle
    {
        public ChallengeChainSubSlot ChallengeChain { get; init; }
        public InfusedChallengeChainSubSlot InfusedChallengeChain { get; init; }
        public RewardChainSubSlot RewardChain { get; init; }
        public SubSlotProofs Proofs { get; init; }
    }
}
