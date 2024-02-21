using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace chia.dotnet
{
    public record HarvesterInfo
    {
        public HarvesterConnection Connection { get; init; } = new();
        public IEnumerable<string> FailedToOpenFileNames { get; init; } = [];
        public IEnumerable<string> NoKeyFilenames { get; init; } = [];
        public IEnumerable<PlotInfo> Plots { get; init; } = [];
        public IEnumerable<string> Duplicates { get; init; } = [];
        public long TotalPlotSize { get; init; }
        public HarvesterSync Syncing { get; init; } = new();
        public double LastSyncTime { get; init; }
        [JsonIgnore]
        public DateTime LastSyncDateTime => LastSyncTime.ToDateTime();
    }
}
