namespace chia.dotnet
{
    public record Announcement
    {
        public string OriginInfo { get; init; } = string.Empty;
        public string Message { get; init; } = string.Empty;
        public string MorphBytes { get; init; } = string.Empty;
    }
    public record CoinAnnouncement
    {
        public string CoinId { get; init; } = string.Empty;
        public string Message { get; init; } = string.Empty;
        public string MorphBytes { get; init; } = string.Empty;
    }
    public record PuzzleAnnouncement
    {
        public string PuzzleHash { get; init; } = string.Empty;
        public string Message { get; init; } = string.Empty;
        public string MorphBytes { get; init; } = string.Empty;
    }
}
