namespace chia.dotnet
{
    public record PlotQueueEntry
    {
        public int Delay { get; set; }
        public bool Deleted { get; set; }
        public string Error { get; set; }
        public string Id { get; set; }
        public string Log { get; set; }
        public string LogNew { get; set; }
        public bool Parallel { get; set; }
        public string Queue { get; set; }
        public KValues Size { get; set; }
        public string State { get; set; }
    }
}
