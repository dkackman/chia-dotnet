﻿using System.Collections.Generic;
using System.Numerics;
using System.Dynamic;
using System.Threading;
using System.Threading.Tasks;

namespace chia.dotnet
{
    /// <summary>
    /// Wraps a Pool Wallet
    /// </summary>
    public sealed class PoolWallet : Wallet
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="walletId">The wallet_id to wrap</param>
        /// <param name="walletProxy">Wallet RPC proxy to use for communication</param>
        public PoolWallet(uint walletId, WalletProxy walletProxy)
            : base(walletId, walletProxy)
        {
        }

        /// <summary>
        /// Join the wallet to a pool
        /// </summary>
        /// <param name="targetPuzzlehash">Puzzle hash</param>
        /// <param name="poolUrl">Url of the pool to join</param>
        /// <param name="relativeLockHeight">Relative lock height</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>A transaction</returns>
        public async Task<dynamic> JoinPool(string targetPuzzlehash, string poolUrl, uint relativeLockHeight, CancellationToken cancellationToken)
        {
            dynamic data = new ExpandoObject();
            data.wallet_id = WalletId;
            data.target_puzzlehash = targetPuzzlehash;
            data.pool_url = poolUrl;
            data.relative_lock_height = relativeLockHeight;

            var response = await WalletProxy.SendMessage("pw_join_pool", data, cancellationToken);

            return response.Data.transaction;
        }

        /// <summary>
        /// Leaving a pool requires two state transitions.
        /// First we transition to PoolSingletonState.LEAVING_POOL
        /// Then we transition to FARMING_TO_POOL or SELF_POOLING
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>A transaction</returns>
        public async Task<dynamic> SelfPool(CancellationToken cancellationToken)
        {
            var response = await WalletProxy.SendMessage("pw_self_pool", cancellationToken);

            return response.Data.transaction;
        }

        /// <summary>
        /// Perform a sweep of the p2_singleton rewards controlled by the pool wallet singleton
        /// </summary>
        /// <param name="fee">Transaction fee</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>Wallet state and transaction</returns>
        public async Task<(dynamic State, dynamic Transaction)> AbsorbRewards(BigInteger fee, CancellationToken cancellationToken)
        {
            dynamic data = new ExpandoObject();
            data.fee = fee;

            var response = await WalletProxy.SendMessage("pw_absorb_rewards", cancellationToken);

            return (response.Data.state, response.Data.transaction);
        }

        /// <summary>
        /// Perform a sweep of the p2_singleton rewards controlled by the pool wallet singleton
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>Wallet state and list of unconfirmed transactions</returns>
        public async Task<(dynamic State, IEnumerable<dynamic> Transaction)> Status(CancellationToken cancellationToken)
        {
            var response = await WalletProxy.SendMessage("pw_status", cancellationToken);

            return (response.Data.state, response.Data.unconfirmed_transactions);
        }
    }
}