using System.Collections.Generic;

namespace chia.dotnet
{
    public record HarvesterConnection
    {
        public string Host { get; init; } = string.Empty;
        public string NodeId { get; init; } = string.Empty;
        public int Port { get; init; }
    }

    public record HarvesterInfo
    {
        public HarvesterConnection Connection { get; init; } = new();
        public ICollection<PlotInfo> FailedToOpenFileNames { get; init; } = new List<PlotInfo>();
        public ICollection<PlotInfo> NotFoundFileNames { get; init; } = new List<PlotInfo>();
        public ICollection<PlotInfo> Plots { get; init; } = new List<PlotInfo>();
    }
}
