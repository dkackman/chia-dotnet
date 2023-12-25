namespace chia.dotnet
{
    public record Announcement
    {
        public string OriginInfo { get; init; } = string.Empty;
        public string Message { get; init; } = string.Empty;
        public string? MorphBytes { get; init; }
    }
}
