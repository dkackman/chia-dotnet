namespace chia.dotnet
{
    public record VCRecord
    {
        public string? CoinId { get; init; }
        public VerifiedCredential VC { get; init; } = new();
        public uint ConfirmedAtHeight { get; init; }
    }
}
