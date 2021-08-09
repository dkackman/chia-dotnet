namespace chia.dotnet
{
    public record SignagePoint
    {
        public string ChallengeChainSp { get; init; }
        public string ChallengeHash { get; init; }
        public ulong Difficulty { get; init; }
        public string RewardChainSp { get; init; }
        public byte SignagePointIndex { get; init; }
        public ulong SubSlotIters { get; init; }
    }
}
