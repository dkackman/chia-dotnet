using System.Collections.Generic;

namespace chia.dotnet
{
    public record HarvesterInfo
    {
        public HarvesterConnection Connection { get; init; } = new();
        public ICollection<PlotInfo> FailedToOpenFileNames { get; init; } = new List<PlotInfo>();
        public ICollection<PlotInfo> NotFoundFileNames { get; init; } = new List<PlotInfo>();
        public ICollection<PlotInfo> Plots { get; init; } = new List<PlotInfo>();
    }
}
