namespace chia.dotnet
{
    public record SubSlotProofs
    {
        public VDFProof ChallengeChainSlotProof { get; init; } = new();
        public VDFProof? InfusedChallengeChainSlotProof { get; init; }
        public VDFProof RewardChainSlotProof { get; init; } = new();
    }
}
