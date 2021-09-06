namespace chia.dotnet
{
    public record ProofOfSpace
    {
        public string Challenge { get; init; } = string.Empty;
        /// <summary>
        /// Only one of these two should be present
        /// </summary>
        public string? PublicPoolKey { get; init; }
        public string? PoolContractPuzzleHash { get; init; }
        public string PlotPublicKey { get; init; } = string.Empty;
        public KValues Size { get; init; }
        public string Proof { get; init; } = string.Empty;
    }
}
