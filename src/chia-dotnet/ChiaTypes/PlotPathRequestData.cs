using System.Collections.Generic;

namespace chia.dotnet
{
    public record PlotPathRequestData
    {
        public string NodeId { get; init; } = string.Empty;
        public int Page { get; init; }
        public int PageSize { get; init; }
        public IEnumerable<string> Filter { get; init; } = new List<string>();
        public bool Reverse { get; init; }
    }
}
