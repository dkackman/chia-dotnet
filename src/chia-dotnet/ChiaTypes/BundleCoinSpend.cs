using System.Collections.Generic;

namespace chia.dotnet
{
    public record BundleCoinSpend
    {
        public CoinSpend CoinSpend { get; init; } = new();
        public IEnumerable<Coin> Additions { get; init; } = [];
        public ulong? Cost { get; init; }
        public bool EligibleForDedup { get; init; }
        public bool EligibleForFastForward { get; init; }

    }
}
