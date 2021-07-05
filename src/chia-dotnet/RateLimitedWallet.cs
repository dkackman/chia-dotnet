using System;
using System.Collections.Generic;
using System.Numerics;
using System.Dynamic;
using System.Threading;
using System.Threading.Tasks;

namespace chia.dotnet
{
    /// <summary>
    /// Wraps a Coloured Coin wallet
    /// </summary>
    public sealed class RateLimitedWallet : Wallet
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="walletId">The wallet_id to wrap</param>
        /// <param name="walletProxy">Wallet RPC proxy to use for communication</param>
        public RateLimitedWallet(uint walletId, WalletProxy walletProxy)
            : base(walletId, walletProxy)
        {
        }
    }
}
