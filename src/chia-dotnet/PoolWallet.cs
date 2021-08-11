using System.Collections.Generic;
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
        /// <returns>The resulting <see cref="TransactionRecord"/></returns>
        public async Task<TransactionRecord> JoinPool(string targetPuzzlehash, string poolUrl, uint relativeLockHeight, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.wallet_id = WalletId;
            data.target_puzzlehash = targetPuzzlehash;
            data.pool_url = poolUrl;
            data.relative_lock_height = relativeLockHeight;

            return await WalletProxy.SendMessage<TransactionRecord>("pw_join_pool", data, "transaction", cancellationToken);
        }

        /// <summary>
        /// Leaving a pool requires two state transitions.
        /// First we transition to PoolSingletonState.LEAVING_POOL
        /// Then we transition to FARMING_TO_POOL or SELF_POOLING
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>The resulting <see cref="TransactionRecord"/></returns>
        public async Task<TransactionRecord> SelfPool(CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.wallet_id = WalletId;

            return await WalletProxy.SendMessage<TransactionRecord>("pw_self_pool", data, "transaction", cancellationToken);
        }

        /// <summary>
        /// Perform a sweep of the p2_singleton rewards controlled by the pool wallet singleton
        /// </summary>
        /// <param name="fee">Transaction fee (in units of mojos)</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>Wallet state and transaction</returns>
        public async Task<(PoolWalletInfo State, TransactionRecord Transaction)> AbsorbRewards(ulong fee, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.wallet_id = WalletId;
            data.fee = fee;

            var response = await WalletProxy.SendMessage("pw_absorb_rewards", cancellationToken);

            return (
                Converters.ToObject<PoolWalletInfo>(response.state),
                Converters.ToObject<TransactionRecord>(response.transaction)
                );
        }

        /// <summary>
        /// Perform a sweep of the p2_singleton rewards controlled by the pool wallet singleton
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>Wallet state and list of unconfirmed transactions</returns>
        // TODO - what is this type
        public async Task<(dynamic State, IEnumerable<dynamic> UnconfirmedTransactions)> Status(CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.wallet_id = WalletId;

            var response = await WalletProxy.SendMessage("pw_status", data, cancellationToken);

            return (response.state, response.unconfirmed_transactions);
        }
    }
}
