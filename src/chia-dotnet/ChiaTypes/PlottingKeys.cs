namespace chia.dotnet
{
    public record PlottingKeys
    {
        public string FarmerPublicKey { get; init; } = string.Empty;
        public string PoolPublicKey { get; init; } = string.Empty;
    }
}
