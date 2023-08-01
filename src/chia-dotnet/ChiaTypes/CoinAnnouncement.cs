namespace chia.dotnet
{
    public record CoinAnnouncement
    {
        public string CoinId { get; init; } = string.Empty;
        public string Message { get; init; } = string.Empty;
        public string MorphBytes { get; init; } = string.Empty;
    }
}
