using System;

using Newtonsoft.Json;

namespace chia.dotnet
{
    public record HarvesterSummary
    {
        public HarvesterConnection Connection { get; init; } = new();
        public int FailedToOpenFileNames { get; init; }
        public int NotFoundFileNames { get; init; }
        public int NoKeyFilenames { get; init; }
        public int Duplicates { get; init; }
        public int Plots { get; init; }
        public long TotalPlotSize { get; init; }
        public HarvesterSync Syncing { get; init; } = new();
        public double LastSyncTime { get; init; }
        [JsonIgnore]
        public DateTime LastSyncDateTime => LastSyncTime.ToDateTime();
    }
}
