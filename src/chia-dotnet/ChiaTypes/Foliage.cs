namespace chia.dotnet
{
    /// <summary>
    /// The entire foliage block, containing signature and the unsigned back pointer
    /// The hash of this is the "header hash". Note that for unfinished blocks, the prev_block_hash
    /// Is the prev from the signage point, and can be replaced with a more recent block
    /// </summary>
    public record Foliage
    {
        public string PrevBlockHash { get; init; } = string.Empty;
        public string RewardBlockHash { get; init; } = string.Empty;
        public FoliageBlockData FoliageBlockData { get; init; } = new();
        public string FoliageBlockDataSignature { get; init; } = string.Empty;
        public string? FoliageTransactionBlockHash { get; init; }
        public string? FoliageTransactionBlockSignature { get; init; }
    }
}
