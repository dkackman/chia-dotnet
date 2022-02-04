using System;
using Newtonsoft.Json;

namespace chia.dotnet
{
    /// <summary>
    /// An entry on the plotter queue
    /// </summary>
    public record QueuedPlotInfo
    {
        public int Delay { get; init; }
        public bool Deleted { get; init; }
        public string Error { get; init; } = string.Empty;
        public string Id { get; init; } = string.Empty;
        public string Log { get; init; } = string.Empty;
        public string LogNew { get; init; } = string.Empty;
        public bool Parallel { get; init; }
        public string Queue { get; init; } = string.Empty;
        public KSize Size { get; init; }
        public string State { get; init; } = string.Empty;
        [JsonIgnore]
        public PlotState PlotState => Enum.Parse<PlotState>(State);
    }
}
