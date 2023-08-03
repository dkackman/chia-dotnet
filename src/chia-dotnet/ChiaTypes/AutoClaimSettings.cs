namespace chia.dotnet
{
    public record AutoClaimSettings
    {
        public bool Enabled { get; init; }
        public ulong TxFee { get; init; }
        public ulong MinAmount { get; init; }
        public ushort BatchSize { get; init; }
    }
}
