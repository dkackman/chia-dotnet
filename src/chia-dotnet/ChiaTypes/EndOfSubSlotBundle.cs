namespace chia.dotnet
{
    public record EndOfSubSlotBundle
    {
        public ChallengeChainSubSlot ChallengeChain { get; init; } = new();
        public InfusedChallengeChainSubSlot? InfusedChallengeChain { get; init; }
        public RewardChainSubSlot RewardChain { get; init; } = new();
        public SubSlotProofs Proofs { get; init; } = new();
    }
}
