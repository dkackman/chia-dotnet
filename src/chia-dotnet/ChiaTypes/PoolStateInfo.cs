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
        public byte? AuthenticationTokenTimeout { get; init; }
        public ulong? CurrentDifficulty { get; init; }
        public ulong CurrentPoints { get; init; }
        public double NextFarmerUpdate { get; init; }
        public double NextPoolInfoUpdate { get; init; }
        [JsonProperty("points_acknowledged_24h")]
        public IEnumerable<PoolPoint> PointsAcknowledged24h { get; init; } = [];
        public ulong PointsAcknowledgedSinceStart { get; init; }
        [JsonProperty("points_found_24h")]
        public IEnumerable<PoolPoint> PointsFound24h { get; init; } = [];
        public ulong PointsFoundSinceStart { get; init; }
        public int PlotCount { get; init; }
        public PoolWalletConfig PoolConfig { get; init; } = new();
        public IEnumerable<ErrorResponse> PoolErrors24h { get; init; } = [];
        [JsonIgnore]
        public DateTime NextFarmerUpdateDateTime => NextFarmerUpdate.ToDateTime();
        [JsonIgnore]
        public DateTime NextPoolInfoUpdateDateTime => NextFarmerUpdate.ToDateTime();
    }
}
