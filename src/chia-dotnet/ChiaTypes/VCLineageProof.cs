namespace chia.dotnet
{
    /// <summary>
    /// The covenant layer for exigent metadata layers requires to be passed the previous parent's metadata too
    /// </summary>
    public record VCLineageProof : LineageProof
    {
        public string? ParentProofHash { get; init; }
    }
}
