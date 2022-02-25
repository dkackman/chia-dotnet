namespace chia.dotnet
{
    public record CATInfo
    {
        public string AssetId { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
        public string Symbol { get; init; } = string.Empty;
    }
}
