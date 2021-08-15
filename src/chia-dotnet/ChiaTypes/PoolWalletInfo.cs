namespace chia.dotnet
{
    /// <summary>
    /// Internal Pool Wallet state, not destined for the blockchain. This can be completely derived with
    /// the Singleton's CoinSpends list, or with the information from the WalletPoolStore.
    /// </summary>
    public record PoolWalletInfo
    {
        public PoolState Current { get; init; }
        public PoolState target { get; init; }
        public Coin LauncherCoin { get; init; }
        public string LauncherId { get; init; }
        public string P2SingletonPuzzleHash { get; init; }
        /// <summary>
        /// Inner puzzle in current singleton, not revealed yet
        /// </summary>
        public string CurrentInner { get; init; }
        public string TipSingletonCoinId { get; init; }
        /// <summary>
        /// Block height that current PoolState is from
        /// </summary>
        public uint SingletonBlockHeight { get; init; }
    }
}
