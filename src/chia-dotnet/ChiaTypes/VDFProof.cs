namespace chia.dotnet
{
    public record VDFProof
    {
        public byte WitnessType { get; init; }
        public string Witness { get; init; } = string.Empty;
        public bool NormalizedToIdentity { get; init; }
    }
}
