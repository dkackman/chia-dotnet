using System.Collections.Generic;

namespace chia.dotnet
{
    public record HarvesterConnection
    {
        public string Host { get; init; }
        public string NodeId { get; init; }
        public int Port { get; init; }
    }

    public record HarvesterInfo
    {
        public HarvesterConnection Connection { get; init; }
        public List<PlotInfo> FailedToOpenFileNames { get; init; }
        public List<PlotInfo> NotFoundFileNames { get; init; }
        public List<PlotInfo> Plots { get; init; }
    }
}
