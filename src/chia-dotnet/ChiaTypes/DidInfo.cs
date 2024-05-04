using System.Collections.Generic;

namespace chia.dotnet
{
    public record DIDInfo
    {
        public string DidId { get; init; } = string.Empty;
        public string LatestCoin { get; init; } = string.Empty;
        public string P2Address { get; init; } = string.Empty;
        public string PublicKey { get; init; } = string.Empty;
        public string RecoveryListHash { get; init; } = string.Empty;
        public int NumVerifications { get; init; }
        public IDictionary<string, string> Metadata { get; init; } = new Dictionary<string, string>();
        public string LauncherId { get; init; } = string.Empty;
        public string FullPuzzle { get; init; } = string.Empty;
        public IEnumerable<object> Solution { get; init; } = [];
        public IEnumerable<string> Hints { get; init; } = [];
    }
}
