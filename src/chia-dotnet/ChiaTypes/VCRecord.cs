namespace chia.dotnet
{
    public record VCRecord
    {
        public VerifiedCredential VC { get; init; } = new();
        public uint ConfirmedAtHeight { get; init; }
    }
}
