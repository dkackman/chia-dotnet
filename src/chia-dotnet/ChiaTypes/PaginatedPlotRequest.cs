using System.Collections.Generic;

namespace chia.dotnet
{
    public record PaginatedPlotRequest
    {
        public string NodeId { get; init; } = string.Empty;
        public int Page { get; init; }
        public int PageCount { get; init; }
        public int TotalCount { get; init; }
        public IEnumerable<PlotInfo> Plots { get; init; } = new List<PlotInfo>();
    }
}
