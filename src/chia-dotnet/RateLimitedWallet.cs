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

        /// <summary>
        /// Sets user info for the wallet
        /// </summary>
        /// <param name="interval">The limit interval</param>
        /// <param name="limit">The limit amount</param>
        /// <param name="origin">Origin infomration about the wallet</param> 
        /// <param name="adminPubkey">The wallet admin pubkey</param> 
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>An awaitable <see cref="Task"/></returns>
        public async Task SetUserInfo(ulong interval, ulong limit, (string parent_coin_info, string puzzle_hash, ulong amount) origin, string adminPubkey, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.wallet_id = WalletId;
            data.interval = interval;
            data.limit = limit;
            data.origin = origin;
            data.admin_pubkey = adminPubkey;

            _ = await WalletProxy.SendMessage("rl_set_user_info", data, cancellationToken);
        }

        /// <summary>
        /// Sends a clawback transaction
        /// </summary>
        /// <param name="fee">Fee amount (in units of mojos)</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>Information about the transaction</returns>
        public async Task<TransactionRecord> SendClawbackTransaction(ulong fee, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.wallet_id = WalletId;
            data.fee = fee;

            return await WalletProxy.SendMessage<TransactionRecord>("send_clawback_transaction", data, "transaction", cancellationToken);
        }

        /// <summary>
        /// Add rate limited funds to the wallet
        /// </summary>
        /// <param name="amount">The amount to add (in units of mojos)</param>
        /// <param name="fee">Transaction fee (in units of mojos)</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>An awaitable <see cref="Task"/></returns>
        public async Task AddFunds(ulong amount, ulong fee, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.wallet_id = WalletId;
            data.amount = amount;
            data.fee = fee;

            _ = await WalletProxy.SendMessage("add_rate_limited_funds", data, cancellationToken);
        }
    }
}
