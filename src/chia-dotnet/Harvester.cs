using System.Collections.Generic;

namespace chia.dotnet
{
    public record HarvesterConnection
    {
        public string Host { get; init; }
        public string NodeId { get; init; }
        public int Port { get; init; }
    }

    public record Harvester
    {
        public HarvesterConnection Connection { get; init; }
        public List<PlotFile> FailedToOpenFileNames { get; init; }
        public List<PlotFile> NotFoundFileNames { get; init; }
        public List<PlotFile> Plots { get; init; }
    }
}
