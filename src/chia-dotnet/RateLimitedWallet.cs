using System;
using System.Dynamic;
using System.Threading;
using System.Threading.Tasks;

namespace chia.dotnet
{
    /// <summary>
    /// Wraps a CAT wallet
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

        /// <summary>
        /// Validates that <see cref="Wallet.WalletId"/> is a <see cref="WalletType.RATE_LIMITED"/>
        /// </summary>
        /// <returns>True if the wallet is Rate Limited</returns>
        public override async Task Validate(CancellationToken cancellationToken = default)
        {
            await Validate(WalletType.RATE_LIMITED, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Sets user info for the wallet
        /// </summary>
        /// <param name="interval">The limit interval</param>
        /// <param name="limit">The limit amount</param>
        /// <param name="parentCoinInfo">Origin parent coin</param> 
        /// <param name="puzzleHash">Origin puzzle hash</param> 
        /// <param name="amount">Origin amount</param> 
        /// <param name="adminPubkey">The wallet admin pubkey</param> 
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>An awaitable <see cref="Task"/></returns>
        public async Task SetUserInfo(ulong interval, ulong limit, string parentCoinInfo, string puzzleHash, ulong amount, string adminPubkey, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(parentCoinInfo))
            {
                throw new ArgumentNullException(nameof(parentCoinInfo));
            }

            if (string.IsNullOrEmpty(puzzleHash))
            {
                throw new ArgumentNullException(nameof(puzzleHash));
            }

            if (string.IsNullOrEmpty(adminPubkey))
            {
                throw new ArgumentNullException(nameof(adminPubkey));
            }

            dynamic origin = new ExpandoObject();
            origin.parent_coin_info = parentCoinInfo;
            origin.puzzle_hash = puzzleHash;
            origin.amount = amount;

            dynamic data = CreateWalletDataObject();
            data.interval = interval;
            data.limit = limit;
            data.origin = origin;
            data.admin_pubkey = adminPubkey;

            _ = await WalletProxy.SendMessage("rl_set_user_info", data, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Sends a clawback transaction
        /// </summary>
        /// <param name="fee">Fee amount (in units of mojos)</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>The <see cref="TransactionRecord"/></returns>
        public async Task<TransactionRecord> SendClawbackTransaction(ulong fee, CancellationToken cancellationToken = default)
        {
            dynamic data = CreateWalletDataObject();
            data.fee = fee;

            return await WalletProxy.SendMessage<TransactionRecord>("send_clawback_transaction", data, "transaction", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Add rate limited funds to the wallet
        /// </summary>
        /// <param name="amount">The amount to add (in units of mojos)</param>
        /// <param name="fee">Transaction fee (in units of mojos)</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>An awaitable <see cref="Task"/></returns>
        public async Task AddFunds(ulong amount, ulong fee, CancellationToken cancellationToken = default)
        {
            dynamic data = CreateWalletDataObject();
            data.amount = amount;
            data.fee = fee;

            _ = await WalletProxy.SendMessage("add_rate_limited_funds", data, cancellationToken).ConfigureAwait(false);
        }
    }
}
