using System.Collections.Generic;

namespace chia.dotnet
{
    public record PlotInfoRequestData
    {
        public string NodeId { get; init; } = string.Empty;
        public int Page { get; init; }
        public int PageSize { get; init; }
        public IEnumerable<FilterItem> Filter { get; init; } = new List<FilterItem>();
        public string SortKey { get; init; } = "filename";
        public bool Reverse { get; init; }
    }
}
