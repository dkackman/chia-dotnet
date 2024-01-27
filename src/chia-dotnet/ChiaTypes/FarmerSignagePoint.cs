namespace chia.dotnet
{
    /// <summary>
    /// This type doesn't exist in the chia code but is generated and passed around as a dictionary
    /// (not to be confused with <see cref="SignagePoint"/>)
    /// </summary>
    public record FarmerSignagePoint
    {
        public string ChallengeChainSp { get; init; } = string.Empty;
        public string ChallengeHash { get; init; } = string.Empty;
        public ulong Difficulty { get; init; }
        public string RewardChainSp { get; init; } = string.Empty;
        public byte SignagePointIndex { get; init; }
        public ulong SubSlotIters { get; init; }
    }
}
