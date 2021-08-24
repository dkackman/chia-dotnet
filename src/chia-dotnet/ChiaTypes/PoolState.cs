using System;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace chia.dotnet
{
    /// <summary>
    /// From the user's point of view, a pool group can be in these states:
    /// `SELF_POOLING`: The singleton exists on the blockchain, and we are farming
    ///     block rewards to a wallet address controlled by the user
    /// 
    /// `LEAVING_POOL`: The singleton exists, and we have entered the "escaping" state, which
    ///     means we are waiting for a number of blocks = `relative_lock_height` to pass, so we can leave.
    /// 
    /// `FARMING_TO_POOL`: The singleton exists, and it is assigned to a pool.
    /// 
    /// `CLAIMING_SELF_POOLED_REWARDS`: We have submitted a transaction to sweep our
    ///     self-pooled funds.
    /// </summary>
    public enum PoolSingletonState
    {
        /// <summary>
        /// The singleton exists on the blockchain, and we are farming
        ///    block rewards to a wallet address controlled by the user
        /// </summary>
        SELF_POOLING = 1,
        /// <summary>
        /// The singleton exists, and we have entered the "escaping" state, which
        /// means we are waiting for a number of blocks = `relative_lock_height` to pass, so we can leave.
        /// </summary>
        LEAVING_POOL = 2,
        /// <summary>
        /// The singleton exists, and it is assigned to a pool.
        /// </summary>
        FARMING_TO_POOL = 3
    }

    /// <summary>
    /// This is what goes into the user's config file, to communicate between the wallet and the farmer processes.
    /// </summary>
    public record PoolWalletConfig
    {
        public string LauncherId { get; init; } = string.Empty;
        public string PoolUrl { get; init; } = string.Empty;
        public string PayoutInstructions { get; init; } = string.Empty;
        public string TargetPuzzleHash { get; init; } = string.Empty;
        public string P2SingletonPuzzleHash { get; init; } = string.Empty;
        public string OwnerPublicKey { get; init; } = string.Empty;
        public string AuthenticationPublicKey { get; init; } = string.Empty;
    }

    [JsonConverter(typeof(PoolPointConverter))]
    public record PoolPoint
    {
        public double TimeFound { get; init; }
        public ulong Difficulty { get; init; }
        [JsonIgnore]
        public DateTime DateTimeFound => TimeFound.ToDateTime();
    }

    /// <summary>
    /// This type does not exist in the chia python, but is returned as a dicitonary for the UI to show pool state.
    /// Not to be confused with <see cref="PoolState"/>
    /// </summary>
    public record PoolStateInfo
    {
        public int AuthenticationTokenTimeout { get; init; }
        public ulong CurrentDifficulty { get; init; }
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
        /// <summary>
        ///  PoolSingletonState
        /// </summary>
        public PoolSingletonState State { get; init; }
        /// <summary>
        /// A puzzle_hash we pay to
        /// When self-farming, this is a main wallet address
        /// When farming-to-pool, the pool sends this to the farmer during pool protocol setup
        /// </summary>
        public string TargetPuzzleHash { get; init; } = string.Empty;
        /// <summary>
        /// owner_pubkey is set by the wallet, once
        /// </summary>
        public string OwnerPubkey { get; init; } = string.Empty;
        public string? PoolUrl { get; init; }
        public uint RelativeLockHeight { get; init; }
    }
}
