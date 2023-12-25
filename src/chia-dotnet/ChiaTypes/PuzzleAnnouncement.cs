namespace chia.dotnet
{
    public record PuzzleAnnouncement
    {
        public string PuzzleHash { get; init; } = string.Empty;
        public string Message { get; init; } = string.Empty;
        public string? MorphBytes { get; init; }
    }
}
