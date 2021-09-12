using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace chia.dotnet
{
    /// <summary>
    /// Base class representing a specific wallet (i.e. anything with a WalletID)
    /// </summary>
    public class Wallet
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="walletId">The wallet_id to wrap</param>
        /// <param name="walletProxy">Wallet RPC proxy to use for communication</param>
        public Wallet(uint walletId, WalletProxy walletProxy)
        {
            WalletId = walletId;
            WalletProxy = walletProxy ?? throw new ArgumentNullException(nameof(walletProxy));
        }

        /// <summary>
        /// The id of the wallet
        /// </summary>
        public uint WalletId { get; init; }

        /// <summary>
        /// Wallet RPC proxy for communication
        /// </summary>
        public WalletProxy WalletProxy { get; init; }

        /// <summary>
        /// Validtes that <see cref="WalletId"/> is a <see cref="WalletType.STANDARD_WALLET"/>
        /// </summary>
        /// <returns>True if the wallet if of the expected type</returns>
        /// <remarks>Intended to be overriden by derived classes of specific <see cref="WalletType"/></remarks>
        public virtual async Task Validate(CancellationToken cancellationToken = default)
        {
            await Validate(WalletType.STANDARD_WALLET, cancellationToken);
        }

        /// <summary>
        /// Validates that <see cref="WalletId"/> exists and is of <see cref="WalletType"/>
        /// </summary>
        /// <param name="walletType">The expected type of wallet</param>
        /// <returns>true if the wallet is of the expected type</returns>
        /// <remarks>Throws n exception if the wallet does not exist</remarks>
        protected virtual async Task Validate(WalletType walletType, CancellationToken cancellationToken)
        {
            var info = await GetWalletInfo(cancellationToken);
            if (info.Type != walletType)
            {
                throw new InvalidOperationException($"Wallet {WalletId} if of type {info.Type} not {walletType}");
            }
        }

        /// <summary>
        /// Retrieves information about this wallet
        /// </summary>
        /// <returns><see cref="WalletInfo"/> and the wallet pubkey fingerprint</returns>
        public async Task<WalletInfo> GetWalletInfo(CancellationToken cancellationToken = default)
        {
            var wallets = await WalletProxy.GetWallets(cancellationToken);
            var info = wallets.FirstOrDefault(i => i.Id == WalletId);
            return info is null ? throw new InvalidOperationException($"No wallet with an id of {WalletId} was found") : info;
        }

        /// <summary>
        /// Get the balance of a specific wallet
        /// </summary>      
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>The wallet balance (in units of mojos)</returns>
        public async Task<(ulong ConfirmedWalletBalance, ulong UnconfirmedWalletBalance, ulong SpendableBalance, ulong PendingChange, ulong MaxSendAmount, int UnspentCoinCount, int PendingCoinRemovalCount)>
            GetBalance(CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.wallet_id = WalletId;

            var response = await WalletProxy.SendMessage("get_wallet_balance", data, cancellationToken);

            return (
                response.wallet_balance.confirmed_wallet_balance,
                response.wallet_balance.unconfirmed_wallet_balance,
                response.wallet_balance.spendable_balance,
                response.wallet_balance.pending_change,
                response.wallet_balance.max_send_amount,
                response.wallet_balance.unspent_coin_count,
                response.wallet_balance.pending_coin_removal_count
                );
        }

        /// <summary>
        /// Get the list of transactions
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>A list of <see cref="TransactionRecord"/>s</returns>
        public async Task<IEnumerable<TransactionRecord>> GetTransactions(CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.wallet_id = WalletId;

            return await WalletProxy.SendMessage<IEnumerable<TransactionRecord>>("get_transactions", data, "transactions", cancellationToken);
        }

        /// <summary>
        /// Get the last address or create a new one
        /// </summary>
        /// <param name="newAddress">Whether to generate a new address</param> 
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>An address</returns>
        public async Task<string> GetNextAddress(bool newAddress, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.wallet_id = WalletId;
            data.new_address = newAddress;

            var response = await WalletProxy.SendMessage("get_next_address", data, cancellationToken);

            return response.address;
        }

        /// <summary>
        /// Get the number of transactions
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>The number of transactions</returns>
        public async Task<uint> GetTransactionCount(CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.wallet_id = WalletId;

            var response = await WalletProxy.SendMessage("get_transaction_count", data, cancellationToken);

            return response.count;
        }

        /// <summary>
        /// Delete unconfirmed transactions from the wallet
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>An awaitable <see cref="Task"/></returns>
        public async Task DeleteUnconfirmedTransactions(CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.wallet_id = WalletId;

            _ = await WalletProxy.SendMessage("delete_unconfirmed_transactions", data, cancellationToken);
        }

        /// <summary>
        /// Sends a transaction
        /// </summary>
        /// <param name="address">The receiving address</param>
        /// <param name="amount">The amount to send (in units of mojos)</param>
        /// <param name="fee">Fee amount (in units of mojos)</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>The <see cref="TransactionRecord"/></returns>
        public async Task<TransactionRecord> SendTransaction(string address, ulong amount, ulong fee, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(address))
            {
                throw new ArgumentNullException(nameof(address));
            }

            dynamic data = new ExpandoObject();
            data.wallet_id = WalletId;
            data.address = address;
            data.amount = amount;
            data.fee = fee;

            return await WalletProxy.SendMessage<TransactionRecord>("send_transaction", data, "transaction", cancellationToken);
        }

        /// <summary>
        /// Sends a transaction
        /// </summary>
        /// <param name="additions">Additions to the block chain</param>
        /// <param name="coins">Coins to include</param>
        /// <param name="fee">Fee amount (in units of mojos)</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>The <see cref="TransactionRecord"/></returns>
        public async Task<TransactionRecord> SendTransactionMulti(IEnumerable<Coin> additions, IEnumerable<Coin>? coins, ulong fee, CancellationToken cancellationToken = default)
        {
            if (additions is null)
            {
                throw new ArgumentNullException(nameof(additions));
            }

            dynamic data = new ExpandoObject();
            data.wallet_id = WalletId;
            data.additions = additions.ToList();
            data.fee = fee;
            if (coins != null) // coins are optional
            {
                data.coins = coins.ToList();
            }

            return await WalletProxy.SendMessage<TransactionRecord>("send_transaction_multi", data, "transaction", cancellationToken);
        }

        /// <summary>
        /// Sends a transaction
        /// </summary>
        /// <param name="additions">Additions to the block chain</param>
        /// <param name="fee">Fee amount (in units of mojo)</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>The <see cref="TransactionRecord"/></returns>
        public async Task<TransactionRecord> SendTransactionMulti(IEnumerable<Coin> additions, ulong fee, CancellationToken cancellationToken = default)
        {
            return await SendTransactionMulti(additions, null, fee, cancellationToken);
        }
    }
}
