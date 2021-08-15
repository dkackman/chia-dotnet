namespace chia.dotnet
{
    /// <summary>
    /// This type doesn't exist in the chia code but is generated and passed around as a dicitonary
    /// (not to be ocnfused with <see cref="SignagePoint"/>)
    /// </summary>
    public record FarmerSignagePoint
    {
        public string ChallengeChainSp { get; init; }
        public string ChallengeHash { get; init; }
        public ulong Difficulty { get; init; }
        public string RewardChainSp { get; init; }
        public byte SignagePointIndex { get; init; }
        public ulong SubSlotIters { get; init; }
    }
}
