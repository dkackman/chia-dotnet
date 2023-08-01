namespace chia.dotnet
{
    /// <summary>
    /// `PoolState` is a type that is serialized to the blockchain to track the state of the user's pool singleton
    /// `target_puzzle_hash` is either the pool address, or the self-pooling address that pool rewards will be paid to.
    /// `target_puzzle_hash` is NOT the p2_singleton puzzle that block rewards are sent to.
    /// The `p2_singleton` address is the initial address, and the `target_puzzle_hash` is the final destination.
    /// `relative_lock_height` is zero when in SELF_POOLING state
    /// </summary>
    public record PoolState
    {
        private readonly string? poolUrl;

        public byte Version { get; init; } = PoolInfo.POOL_PROTOCOL_VERSION;

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

        // the blockchain doesn't like it when the trailing slash is encoded into the nft (won't be able to decode and find wallet info)
        public string? PoolUrl { get => poolUrl?.TrimEnd('/'); init => poolUrl = value; }

        public uint RelativeLockHeight { get; init; }
    }
}
