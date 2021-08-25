namespace chia.dotnet
{
    public record ChallengeChainSubSlot
    {
        public VDFInfo ChallengeChainEndOfSlotVdf { get; init; } = new();
        /// <summary>
        /// Only at the end of a slot
        /// </summary>
        public string? InfusedChallengeChainSubSlotHash { get; init; }
        /// <summary>
        /// Only once per sub-epoch, and one sub-epoch delayed
        /// </summary>
        public string? SubepochSummaryHash { get; init; }
        /// <summary>
        /// Only at the end of epoch, sub-epoch, and slot
        /// </summary>
        public ulong? NewSubSlotIters { get; init; }
        /// <summary>
        /// Only at the end of epoch, sub-epoch, and slot
        /// </summary>
        public ulong? NewDifficulty { get; init; }
    }
}
