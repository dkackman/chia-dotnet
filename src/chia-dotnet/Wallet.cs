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
    /// <remarks>When not dervied from this represents a <see cref="WalletType.STANDARD_WALLET"/></remarks>
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
        /// Creates a dynamic object and sets its wallet_id property to <see cref="WalletId"/>
        /// </summary>
        /// <returns>A dynamic object</returns>
        protected dynamic CreateWalletDataObject()
        {
            dynamic data = new ExpandoObject();
            data.wallet_id = WalletId;

            return data;
        }

        /// <summary>
        /// Validates that <see cref="WalletId"/> is a <see cref="WalletType.STANDARD_WALLET"/>
        /// </summary>
        /// <returns>True if the wallet is of the expected type</returns>
        /// <remarks>Intended to be overriden by derived classes of specific <see cref="WalletType"/></remarks>
        public virtual async Task Validate(CancellationToken cancellationToken = default)
        {
            await Validate(WalletType.STANDARD_WALLET, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Validates that <see cref="WalletId"/> exists and is of the correct <see cref="WalletType"/>
        /// </summary>
        /// <param name="walletType">The expected type of wallet</param>
        /// <returns>true if the wallet is of the expected type</returns>
        /// <remarks>Throws an exception if the wallet does not exist</remarks>
        protected async Task Validate(WalletType walletType, CancellationToken cancellationToken)
        {
            var info = await GetWalletInfo(cancellationToken).ConfigureAwait(false);
            if (info.Type != walletType)
            {
                throw new InvalidOperationException($"Wallet {WalletId} is of type {info.Type}; not {walletType}");
            }
        }

        /// <summary>
        /// Retrieves information about this wallet
        /// </summary>
        /// <returns><see cref="WalletInfo"/> and the wallet pubkey fingerprint</returns>
        /// <remarks>Throws an exception if the wallet does not exist</remarks>
        public async Task<WalletInfo> GetWalletInfo(CancellationToken cancellationToken = default)
        {
            var wallets = await WalletProxy.GetWallets(true, cancellationToken).ConfigureAwait(false);
            var info = wallets.FirstOrDefault(i => i.Id == WalletId);
            return info is null ? throw new InvalidOperationException($"No wallet with an id of {WalletId} was found") : info;
        }

        /// <summary>
        /// Get the balance of a specific wallet
        /// </summary>      
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>The wallet balance (in units of mojos)</returns>
        public async Task<WalletBalance> GetBalance(CancellationToken cancellationToken = default)
        {
            return await WalletProxy.SendMessage<WalletBalance>("get_wallet_balance", CreateWalletDataObject(), "wallet_balance", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Returns a set of coins that can be used for generating a new transaction.
        /// </summary>
        /// <param name="amount">An amount</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>The list of <see cref="Token"/>s</returns>
        public async Task<IEnumerable<Coin>> SelectCoins(ulong amount, CancellationToken cancellationToken = default)
        {
            dynamic data = CreateWalletDataObject();
            data.amount = amount;

            return await WalletProxy.SendMessage<IEnumerable<Coin>>("select_coins", data, "coins", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Get the list of transactions
        /// </summary>
        /// <param name="toAddress">Restrict results only to this address</param>
        /// <param name="sortKey">Field to sort results by</param>
        /// <param name="reverse">Reverse the sort order of the results</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>A list of <see cref="TransactionRecord"/>s</returns>
        public async Task<IEnumerable<TransactionRecord>> GetTransactions(string? toAddress = null, string? sortKey = null, bool reverse = false, CancellationToken cancellationToken = default)
        {
            var count = await GetTransactionCount(cancellationToken).ConfigureAwait(false);
            return await GetTransactions(0, count, toAddress, sortKey, reverse, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Get the list of transactions
        /// </summary>
        /// <param name="start">the start index of transactions (zero based)</param>
        /// <param name="end">The end index of transactions (max of <see cref="GetTransactionCount(CancellationToken)"/></param>
        /// <param name="toAddress">Restrict results only to this address</param>
        /// <param name="sortKey">Field to sort results by</param>
        /// <param name="reverse">Reverse the sort order of the results</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>A list of <see cref="TransactionRecord"/>s</returns>
        public async Task<IEnumerable<TransactionRecord>> GetTransactions(uint start, uint end, string? toAddress = null, string? sortKey = null, bool reverse = false, CancellationToken cancellationToken = default)
        {
            dynamic data = CreateWalletDataObject();
            data.start = start;
            data.end = end;
            data.to_address = toAddress;
            data.sort_key = sortKey;
            data.reverse = reverse;

            return await WalletProxy.SendMessage<IEnumerable<TransactionRecord>>("get_transactions", data, "transactions", cancellationToken).ConfigureAwait(false);
        }


        /// <summary>
        /// Get the list of spendable coins
        /// </summary>
        /// <param name="minCoinAmount">The minimum coin amount</param>
        /// <param name="maxCoinAmount">The maximum coin amount></param>
        /// <param name="excludedCoinAmounts">Amounts to exlcude</param>
        /// <param name="excludedCoins">Coins to exclude</param>
        /// <param name="excludedCoinIds">Coin ids to exclude</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>Information about spendable coins</returns>
        public async Task<(IEnumerable<CoinRecord> confirmedRecords,
            IEnumerable<CoinRecord> unconfirmedRecords,
            IEnumerable<CoinRecord> unconfirmedAdditions)> GetSpendableCoins(ulong? minCoinAmount, ulong? maxCoinAmount, IEnumerable<ulong>? excludedCoinAmounts = null, IEnumerable<Coin>? excludedCoins = null, IEnumerable<string>? excludedCoinIds = null, CancellationToken cancellationToken = default)
        {
            dynamic data = CreateWalletDataObject();
            if (minCoinAmount is not null)
            {
                data.minCoinAmount = minCoinAmount.Value;
            }
            if (maxCoinAmount is not null)
            {
                data.maxCoinAmount = maxCoinAmount.Value;
            }
            if (excludedCoinAmounts is not null)
            {
                data.excludedCoinAmounts = excludedCoinAmounts.ToList();
            }
            if (excludedCoins is not null)
            {
                data.excludedCoins = excludedCoins.ToList();
            }
            if (excludedCoinIds is not null)
            {
                data.excludedCoinIds = excludedCoinIds.ToList();
            }
            var response = await WalletProxy.SendMessage("get_spendable_coins", data, cancellationToken).ConfigureAwait(false);

            return (
                Converters.ToObject<CoinRecord>(response.confirmed_records),
                Converters.ToObject<CoinRecord>(response.unconfirmed_removals),
                Converters.ToObject<Coin>(response.unconfirmed_additions)
                );
        }

        /// <summary>
        /// Get the last address or create a new one
        /// </summary>
        /// <param name="newAddress">Whether to generate a new address</param> 
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>An address</returns>
        public async Task<string> GetNextAddress(bool newAddress, CancellationToken cancellationToken = default)
        {
            dynamic data = CreateWalletDataObject();
            data.new_address = newAddress;

            var response = await WalletProxy.SendMessage("get_next_address", data, cancellationToken).ConfigureAwait(false);

            return response.address;
        }

        /// <summary>
        /// Get the number of transactions
        /// </summary>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>The number of transactions</returns>
        public async Task<uint> GetTransactionCount(CancellationToken cancellationToken = default)
        {
            var response = await WalletProxy.SendMessage("get_transaction_count", CreateWalletDataObject(), cancellationToken).ConfigureAwait(false);

            return response.count;
        }

        /// <summary>
        /// Delete unconfirmed transactions from the wallet
        /// </summary>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>An awaitable <see cref="Task"/></returns>
        public async Task DeleteUnconfirmedTransactions(CancellationToken cancellationToken = default)
        {
            _ = await WalletProxy.SendMessage("delete_unconfirmed_transactions", CreateWalletDataObject(), cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Sends a transaction
        /// </summary>
        /// <param name="address">The receiving address</param>
        /// <param name="amount">The amount to send (in units of mojos)</param>
        /// <param name="fee">Fee amount (in units of mojos)</param>
        /// <param name="memos">Memos to go along with the transaction</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>The <see cref="TransactionRecord"/></returns>
        public async Task<TransactionRecord> SendTransaction(string address, ulong amount, ulong fee, IEnumerable<string>? memos = null, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(address))
            {
                throw new ArgumentNullException(nameof(address));
            }

            dynamic data = CreateWalletDataObject();
            data.address = address;
            data.amount = amount;
            data.fee = fee;
            if (memos is not null)
            {
                data.memos = memos.ToList();
            }

            return await WalletProxy.SendMessage<TransactionRecord>("send_transaction", data, "transaction", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Sends a transaction
        /// </summary>
        /// <param name="additions">Additions to the block chain</param>
        /// <param name="coins">Coins to include</param>
        /// <param name="fee">Fee amount (in units of mojos)</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>The <see cref="TransactionRecord"/></returns>
        public async Task<TransactionRecord> SendTransactionMulti(IEnumerable<Coin> additions, ulong fee, IEnumerable<Coin>? coins = null, CancellationToken cancellationToken = default)
        {
            if (additions is null)
            {
                throw new ArgumentNullException(nameof(additions));
            }

            dynamic data = CreateWalletDataObject();
            data.additions = additions.ToList();
            data.fee = fee;
            if (coins != null) // coins are optional
            {
                data.coins = coins.ToList();
            }

            return await WalletProxy.SendMessage<TransactionRecord>("send_transaction_multi", data, "transaction", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Sends a transaction
        /// </summary>
        /// <param name="additions">Additions to the block chain</param>
        /// <param name="fee">Fee amount (in units of mojo)</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>The <see cref="TransactionRecord"/></returns>
        public async Task<TransactionRecord> SendTransactionMulti(IEnumerable<Coin> additions, ulong fee, CancellationToken cancellationToken = default)
        {
            return await SendTransactionMulti(additions, fee, cancellationToken).ConfigureAwait(false);
        }
    }
}
