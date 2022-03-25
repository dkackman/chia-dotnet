namespace chia.dotnet
{
    public record BlockCountMetrics
    {
        public int CompactBlock { get; init; }
        public int UncompactBlocks { get; init; }
        public int HintCount { get; init; }
    }
}
