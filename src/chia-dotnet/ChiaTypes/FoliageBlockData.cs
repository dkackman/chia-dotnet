namespace chia.dotnet
{
    /// <summary>
    /// Part of the block that is signed by the plot key
    /// </summary>
    public record FoliageBlockData
    {
        public string UnfinishedRewardBlockHash { get; init; } = string.Empty;
        public PoolTarget PoolTarget { get; init; } = new();
        /// <summary>
        /// Iff ProofOfSpace has a pool pk
        /// </summary>
        public string? PoolSignature { get; init; }
        public string FarmerRewardPuzzleHash { get; init; } = string.Empty;
        /// <summary>
        /// Used for future updates. Can be any 32 byte value initially
        /// </summary>
        public string ExtensionData { get; init; } = string.Empty;
    }
}
