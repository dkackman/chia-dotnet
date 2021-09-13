using System;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace chia.dotnet
{
    /// <summary>
    /// This type does not exist in the chia python, but is returned as a dicitonary for the UI to show pool state.
    /// Not to be confused with <see cref="PoolState"/>
    /// </summary>
    public record PoolStateInfo
    {
        public int? AuthenticationTokenTimeout { get; init; }
        public ulong? CurrentDifficulty { get; init; }
        public ulong CurrentPoints { get; init; }
        public double NextFarmerUpdate { get; init; }
        public double NextPoolInfoUpdate { get; init; }
        public string P2SingletonPuzzleHash { get; init; } = string.Empty;
        [JsonProperty("points_acknowledged_24h")]
        public ICollection<PoolPoint> PointsAcknowledged24h { get; init; } = new List<PoolPoint>();
        public ulong PointsAcknowledgedSinceStart { get; init; }
        [JsonProperty("points_found_24h")]
        public ICollection<PoolPoint> PointsFound24h { get; init; } = new List<PoolPoint>();
        public ulong PointsFoundSinceStart { get; init; }
        public PoolWalletConfig PoolConfig { get; init; } = new();
        public ICollection<ErrorResponse> PoolErrors24h { get; init; } = new List<ErrorResponse>();
        [JsonIgnore]
        public DateTime NextFarmerUpdateDateTime => NextFarmerUpdate.ToDateTime();
        [JsonIgnore]
        public DateTime NextPoolInfoUpdateDateTime => NextFarmerUpdate.ToDateTime();
    }
}
