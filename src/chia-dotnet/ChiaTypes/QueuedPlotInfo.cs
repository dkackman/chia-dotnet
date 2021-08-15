namespace chia.dotnet
{
    /// <summary>
    /// An entry on the plotter queue
    /// </summary>
    public record QueuedPlotInfo
    {
        public int Delay { get; init; }
        public bool Deleted { get; init; }
        public string Error { get; init; }
        public string Id { get; init; }
        public string Log { get; init; }
        public string LogNew { get; init; }
        public bool Parallel { get; init; }
        public string Queue { get; init; }
        public KValues Size { get; init; }
        public string State { get; init; }
    }
}
