using System;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace chia.dotnet
{
    public record PoolConfig
    {
        public string AuthenticationPublicKey { get; init; }
        public string LauncherId { get; init; }
        public string OwnerPublicKey { get; init; }
        public string P2SingletonPuzzleHash { get; init; }
        public string PayoutInstructions { get; init; }
        public string PoolUrl { get; init; }
        public string TargetPuzzleHash { get; init; }
    }

    [JsonConverter(typeof(PoolPointConverter))]
    public record PoolPoint
    {
        public double TimeFound { get; init; }
        public ulong Difficulty { get; init; }
        public DateTime DateTimeFound => TimeFound.ToDateTime();
    }

    public record PoolStateInfo
    {
        public int AuthenticationTokenTimeout { get; init; }
        public ulong CurrentDifficulty { get; init; }
        public ulong CurrentPoints { get; init; }
        public double NextFarmerUpdate { get; init; }
        public double NextPoolInfoUpdate { get; init; }
        public string P2SingletonPuzzleHash { get; init; }
        [JsonProperty("points_acknowledged_24h")]
        public ICollection<PoolPoint> PointsAcknowledged24h { get; init; }
        public ulong PointsAcknowledgedSinceStart { get; init; }
        [JsonProperty("points_found_24h")]
        public ICollection<PoolPoint> PointsFound24h { get; init; }
        public ulong PointsFoundSinceStart { get; init; }
        public PoolConfig PoolConfig { get; init; }
        public ICollection<ErrorResponse> PoolErrors24h { get; init; }
        public DateTime NextFarmerUpdateDateTime => NextFarmerUpdate.ToDateTime();
        public DateTime NextPoolInfoUpdateDateTime => NextFarmerUpdate.ToDateTime();
    }

    /// <summary>
    /// `PoolState` is a type that is serialized to the blockchain to track the state of the user's pool singleton
    /// `target_puzzle_hash` is either the pool address, or the self-pooling address that pool rewards will be paid to.
    /// `target_puzzle_hash` is NOT the p2_singleton puzzle that block rewards are sent to.
    /// The `p2_singleton` address is the initial address, and the `target_puzzle_hash` is the final destination.
    /// `relative_lock_height` is zero when in SELF_POOLING state
    /// </summary>
    public record PoolState
    {
        public byte Version { get; init; }
        public byte State { get; init; }
        public string TargetPuzzleHash { get; init; }
        public string OwnerPubkey { get; init; }
        public string PoolUrl { get; init; }
        public uint RelativeLockHeight { get; init; }
    }
}
