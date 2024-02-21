using System.Collections.Generic;

namespace chia.dotnet
{
    public record Mirror
    {
        public string CoinId { get; init; } = string.Empty;
        public string LauncherId { get; init; } = string.Empty;
        public ulong Amount { get; init; }
        public IEnumerable<string> Urls { get; init; } = [];
        public bool Ours { get; init; } = true;
    }
}
