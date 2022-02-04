using System.Collections.Generic;

namespace chia.dotnet
{
    public record HarvesterInfo
    {
        public HarvesterConnection Connection { get; init; } = new();
        public ICollection<string> FailedToOpenFileNames { get; init; } = new List<string>();
        public ICollection<string> NotFoundFileNames { get; init; } = new List<string>();
        public ICollection<PlotInfo> Plots { get; init; } = new List<PlotInfo>();
    }
}
