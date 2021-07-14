using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Numerics;
using System.Dynamic;
using System.Collections.Generic;

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
        /// Login to the wallet
        /// </summary>
        /// <remarks>Always login before interacting with the wallet. Logged in state is kept on the serve so might have changed</remarks>
        /// <returns>an awaitable <see cref="Task"/></returns>
        public async Task<uint> Login(CancellationToken cancellationToken = default)
        {
            var fingerprints = await WalletProxy.GetPublicKeys(cancellationToken);

            // not 100% sure this applies in all cases but wallets seem to come back in id order
            // haven't figured out a different or better way to get a fingerprint from an id
            var fingerprint = fingerprints.First();

            return await WalletProxy.LogIn(fingerprint, true, cancellationToken);
        }

        /// <summary>
        /// Get the balance of a specific wallet
        /// </summary>      
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>The wallet balance (in units of mojos)</returns>
        public async Task<(BigInteger ConfirmedWalletBalance, BigInteger UnconfirmedWalletBalance, BigInteger SpendableBalance, BigInteger PendingChange, BigInteger MaxSendAmount, int UnspentCoinCount, int PendingCoinRemovalCount)>
            GetBalance(CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.wallet_id = WalletId;

            var response = await WalletProxy.SendMessage("get_wallet_balance", data, cancellationToken);

            return (response.Data.wallet_balance.confirmed_wallet_balance,
                    response.Data.wallet_balance.unconfirmed_wallet_balance,
                    response.Data.wallet_balance.spendable_balance,
                    response.Data.wallet_balance.pending_change,
                    response.Data.wallet_balance.max_send_amount,
                    response.Data.wallet_balance.unspent_coin_count,
                    response.Data.wallet_balance.pending_coin_removal_count
                );
        }

        /// <summary>
        /// Get the list of transactions
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>A list of transactions</returns>
        public async Task<IEnumerable<dynamic>> GetTransactions(CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.wallet_id = WalletId;

            var response = await WalletProxy.SendMessage("get_transactions", data, cancellationToken);

            return response.Data.transactions;
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

            return response.Data.address;
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

            return response.Data.count;
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
        /// <returns>The transaction</returns>
        public async Task<dynamic> SendTransaction(string address, BigInteger amount, BigInteger fee, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.wallet_id = WalletId;
            data.address = address;
            data.amount = amount;
            data.fee = fee;

            var response = await WalletProxy.SendMessage("send_transaction", data, cancellationToken);

            return response.Data.transaction;
        }

        /// <summary>
        /// Sends a transaction
        /// </summary>
        /// <param name="additions">Additions to the block chain</param>
        /// <param name="coins">Coins to include</param>
        /// <param name="fee">Fee amount (in units of mojos)</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>The transaction</returns>
        public async Task<dynamic> SendTransactionMulti(IEnumerable<dynamic> additions, IEnumerable<dynamic> coins, BigInteger fee, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.wallet_id = WalletId;
            data.additions = additions.ToList();
            data.fee = fee;
            if (coins != null) // coins are optional
            {
                data.coins = coins.ToList();
            }

            var response = await WalletProxy.SendMessage("send_transaction_multi", data, cancellationToken);

            return response.Data.transaction;
        }

        /// <summary>
        /// Sends a transaction
        /// </summary>
        /// <param name="additions">Additions to the block chain</param>
        /// <param name="fee">Fee amount</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>The transaction</returns>
        public async Task<dynamic> SendTransactionMulti(IEnumerable<dynamic> additions, BigInteger fee, CancellationToken cancellationToken = default)
        {
            return await SendTransactionMulti(additions, null, fee, cancellationToken);
        }
    }
}
