namespace chia.dotnet
{
    /// <summary>
    /// This class serves as the main driver for the entire VC puzzle stack. Given the information below, it can sync and
    /// spend VerifiedCredentials in any specified manner.Trying to sync from a spend that this class did not create will
    /// likely result in an error.
    /// </summary>
    public record VerifiedCredential
    {
        public Coin Coin { get; init; } = new();
        public LineageProof SingletonLineageProof { get; init; } = new();
        public VCLineageProof EMLLineageProof { get; init; } = new();
        public string LauncherId { get; init; } = string.Empty;
        public string InnerPuzzleHash { get; init; } = string.Empty;
        public string ProofProvider { get; init; } = string.Empty;
        public string? ProofHash { get; init; }
    }
}
