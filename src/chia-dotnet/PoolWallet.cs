using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace chia.dotnet
{
    /// <summary>
    /// Wraps a Pool Wallet
    /// </summary>
    /// <remarks>
    /// ctor
    /// </remarks>
    /// <param name="walletId">The wallet_id to wrap</param>
    /// <param name="walletProxy">Wallet RPC proxy to use for communication</param>
    public sealed class PoolWallet(uint walletId, WalletProxy walletProxy) : Wallet(walletId, walletProxy)
    {
        /// <summary>
        /// Validates that <see cref="Wallet.WalletId"/> is a <see cref="WalletType.POOLING_WALLET"/>
        /// </summary>
        /// <returns>True if the wallet is a pooling wallet</returns>
        public override async Task Validate(CancellationToken cancellationToken = default)
        {
            await Validate(WalletType.POOLING_WALLET, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Join the wallet to a pool
        /// </summary>
        /// <param name="targetPuzzlehash">Puzzle hash</param>
        /// <param name="poolUrl">Url of the pool to join</param>
        /// <param name="relativeLockHeight">Relative lock height</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>The resulting <see cref="TransactionRecord"/></returns>
        /// <remarks>See <see cref="WalletProxy.GetPoolInfo(Uri, CancellationToken)"/></remarks>
        public async Task<(TransactionRecord Transaction, TransactionRecord FeeTransaction, ulong TotalFee, IEnumerable<TransactionRecord> Transactions)>
                JoinPool(string targetPuzzlehash, string poolUrl, uint relativeLockHeight, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(targetPuzzlehash))
            {
                throw new ArgumentNullException(nameof(targetPuzzlehash));
            }

            if (string.IsNullOrEmpty(poolUrl))
            {
                throw new ArgumentNullException(nameof(poolUrl));
            }

            dynamic data = CreateWalletDataObject();
            data.target_puzzlehash = targetPuzzlehash;
            data.pool_url = poolUrl.TrimEnd('/'); // the blockchain doesn't like the trailing /
            data.relative_lock_height = relativeLockHeight;

            var response = await WalletProxy.SendMessage("pw_join_pool", data, cancellationToken).ConfigureAwait(false);
            return (
                Converters.ToObject<TransactionRecord>(response.transaction),
                Converters.ToObject<TransactionRecord>(response.fee_transaction),
                (ulong)response.total_fee,
                Converters.ToObject<IEnumerable<TransactionRecord>>(response.transactions)
                );
        }

        /// <summary>
        /// Leaving a pool requires two state transitions.
        /// First we transition to PoolSingletonState.LEAVING_POOL
        /// Then we transition to FARMING_TO_POOL or SELF_POOLING
        /// </summary>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>The resulting <see cref="TransactionRecord"/></returns>
        public async Task<(TransactionRecord Transaction, TransactionRecord FeeTransaction, ulong TotalFee, IEnumerable<TransactionRecord> Transactions)>
            SelfPool(CancellationToken cancellationToken = default)
        {
            var response = await WalletProxy.SendMessage("pw_self_pool", CreateWalletDataObject(), cancellationToken).ConfigureAwait(false);

            return (
                Converters.ToObject<TransactionRecord>(response.transaction),
                Converters.ToObject<TransactionRecord>(response.fee_transaction),
                (ulong)response.total_fee,
                Converters.ToObject<IEnumerable<TransactionRecord>>(response.transactions)
    );
        }

        /// <summary>
        /// Perform a sweep of the p2_singleton rewards controlled by the pool wallet singleton
        /// </summary>
        /// <param name="fee">Fee (in units of mojos)</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>Wallet state and transaction</returns>
        public async Task<(PoolWalletInfo State, TransactionRecord Transaction, TransactionRecord FeeTransaction, IEnumerable<TransactionRecord> Transactions)>
            AbsorbRewards(ulong fee = 0, CancellationToken cancellationToken = default)
        {
            dynamic data = CreateWalletDataObject();
            data.fee = fee;

            var response = await WalletProxy.SendMessage("pw_absorb_rewards", data, cancellationToken).ConfigureAwait(false);

            return (
                Converters.ToObject<PoolWalletInfo>(response.state),
                Converters.ToObject<TransactionRecord>(response.transaction),
                Converters.ToObject<TransactionRecord>(response.fee_transaction),
                Converters.ToObject<IEnumerable<TransactionRecord>>(response.transactions)
                );
        }

        /// <summary>
        /// Perform a sweep of the p2_singleton rewards controlled by the pool wallet singleton
        /// </summary>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>Wallet state and list of unconfirmed transactions</returns>
        public async Task<(PoolWalletInfo State, IEnumerable<TransactionRecord> UnconfirmedTransactions)> Status(CancellationToken cancellationToken = default)
        {
            var response = await WalletProxy.SendMessage("pw_status", CreateWalletDataObject(), cancellationToken).ConfigureAwait(false);

            return (
                Converters.ToObject<PoolWalletInfo>(response.state),
                Converters.ToObject<IEnumerable<TransactionRecord>>(response.unconfirmed_transactions)
            );
        }
    }
}
