using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace chia.dotnet
{
    public record HarvesterInfo
    {
        public HarvesterConnection Connection { get; init; } = new();
        public IEnumerable<string> FailedToOpenFileNames { get; init; } = new List<string>();
        public IEnumerable<string> NoKeyFilenames { get; init; } = new List<string>();
        public IEnumerable<PlotInfo> Plots { get; init; } = new List<PlotInfo>();
        public IEnumerable<string> Duplicates { get; init; } = new List<string>();
        public long TotalPlotSize { get; init; }
        public HarvesterSync Syncing { get; init; } = new();
        public double LastSyncTime { get; init; }
        [JsonIgnore]
        public DateTime LastSyncDateTime => LastSyncTime.ToDateTime();
    }
}
