namespace chia.dotnet
{
    public record PrivateKeyDetail
    {
        public uint Fingerprint { get; init; }
        public string Sk { get; init; } = string.Empty;
        public string Pk { get; init; } = string.Empty;
        public string FarmerPk { get; init; } = string.Empty;
        public string PoolPk { get; init; } = string.Empty;
        public string Seed { get; init; } = string.Empty;
    }
}
