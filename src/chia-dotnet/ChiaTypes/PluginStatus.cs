using System.Collections.Generic;

namespace chia.dotnet
{
    public record PluginStatus
    {
        public IDictionary<string, IDictionary<string, object>> Uploasders { get; init; } = new Dictionary<string, IDictionary<string, object>>();
        public IDictionary<string, IDictionary<string, object>> Downloaders { get; init; } = new Dictionary<string, IDictionary<string, object>>();
    }
}
