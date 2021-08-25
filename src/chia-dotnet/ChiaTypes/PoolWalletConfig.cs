namespace chia.dotnet
{
    /// <summary>
    /// This is what goes into the user's config file, to communicate between the wallet and the farmer processes.
    /// </summary>
    public record PoolWalletConfig
    {
        public string LauncherId { get; init; } = string.Empty;
        public string PoolUrl { get; init; } = string.Empty;
        public string PayoutInstructions { get; init; } = string.Empty;
        public string TargetPuzzleHash { get; init; } = string.Empty;
        public string P2SingletonPuzzleHash { get; init; } = string.Empty;
        public string OwnerPublicKey { get; init; } = string.Empty;
        public string AuthenticationPublicKey { get; init; } = string.Empty;
    }
}
