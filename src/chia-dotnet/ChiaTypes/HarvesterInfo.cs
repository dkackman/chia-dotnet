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
        public IEnumerable<PlotInfo> FailedToOpenFileNames { get; init; }
        public IEnumerable<PlotInfo> NotFoundFileNames { get; init; }
        public IEnumerable<PlotInfo> Plots { get; init; }
    }
}
