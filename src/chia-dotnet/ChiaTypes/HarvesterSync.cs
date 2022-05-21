namespace chia.dotnet
{
    public record HarvesterSync
    {
        public bool Initial { get; init; }
        public uint PlotFilesProcessed { get; init; }
        public uint PlotFilesTotal { get; init; }
    }
}
