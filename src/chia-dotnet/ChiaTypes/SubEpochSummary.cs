namespace chia.dotnet
{
    public record SubEpochSummary
    {
        public string PrevSubepochSummaryHash { get; init; } = string.Empty;
        /// <summary>
        /// hash of reward chain at end of last segment
        /// </summary>
        public string RewardChainHash { get; init; } = string.Empty;
        /// <summary>
        ///  How many more blocks than 384*(N-1)
        /// </summary>
        public byte NumBlocksOverflow { get; init; }
        /// <summary>
        /// Only once per epoch (diff adjustment)
        /// </summary>
        public ulong? NewDifficulty { get; init; }
        /// <summary>
        /// Only once per epoch (diff adjustment)
        /// </summary>
        public ulong? NewSubSlotIters { get; init; }
    }
}
