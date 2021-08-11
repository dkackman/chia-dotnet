using System;
using System.Collections.Generic;

using Newtonsoft.Json;

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
        public string CurrentInner { get; init; }  //# Inner puzzle in current singleton, not revealed yet
        public string TipSingletonCoinId { get; init; }
        public uint SingletonBlockHeight { get; init; }    //# Block height that current PoolState is from
    }
}
