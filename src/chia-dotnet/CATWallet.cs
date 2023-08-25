using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace chia.dotnet
{
    /// <summary>
    /// Wraps a CAT wallet
    /// </summary>
    public sealed class CATWallet : Wallet
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="walletId">The wallet_id to wrap</param>
        /// <param name="walletProxy">Wallet RPC proxy to use for communication</param>
        public CATWallet(uint walletId, WalletProxy walletProxy)
            : base(walletId, walletProxy)
        {
        }

        /// <summary>
        /// Validates that <see cref="Wallet.WalletId"/> is a <see cref="WalletType.CAT"/>
        /// </summary>
        /// <returns>True if the wallet is a CAT wallet</returns>
        public override async Task Validate(CancellationToken cancellationToken = default)
        {
            await Validate(WalletType.CAT, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Get the name of a wallet's CAT
        /// </summary>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>The CAT name</returns>
        public async Task<string> GetName(CancellationToken cancellationToken = default)
        {
            var response = await WalletProxy.SendMessage("cat_get_name", CreateWalletDataObject(), cancellationToken).ConfigureAwait(false);

            return response.name.ToString();
        }

        /// <summary>
        /// Get the asset id of a wallet's CAT
        /// </summary>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>The asset id</returns>
        public async Task<string> GetAssetId(CancellationToken cancellationToken = default)
        {
            var response = await WalletProxy.SendMessage("cat_get_asset_id", CreateWalletDataObject(), cancellationToken).ConfigureAwait(false);

            return response.asset_id?.ToString() ?? throw new InvalidOperationException("Asset Id not found");
        }

        /// <summary>
        /// Set the name of the CAT
        /// </summary>
        /// <param name="name">The new name</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>An awaitable <see cref="Task"/></returns>
        public async Task SetName(string name, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            dynamic data = CreateWalletDataObject();
            data.name = name;

            await WalletProxy.SendMessage("cat_set_name", data, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Spend a CAT
        /// </summary>
        /// <param name="innerAddress">The inner address for the spend</param>
        /// <param name="amount">The amount to put in the wallet (in units of mojos)</param> 
        /// <param name="memos">Optional list of byte string memos to include in the transaction</param>
        /// <param name="minCoinAmount"></param>
        /// <param name="maxCoinAmount"></param>
        /// <param name="excludeCoinAmounts"></param>
        /// <param name="reusePuzhash"></param>
        /// <param name="fee">The fee to create the wallet (in units of mojos)</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>A <see cref="TransactionRecord"/></returns>
        public async Task<TransactionRecord> Spend(string innerAddress,
            ulong amount,
            IEnumerable<string>? memos = null,
            ulong minCoinAmount = 0,
            ulong maxCoinAmount = 0,
            IEnumerable<ulong>? excludeCoinAmounts = null,
            bool? reusePuzhash = null,
            ulong fee = 0,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(innerAddress))
            {
                throw new ArgumentNullException(nameof(innerAddress));
            }

            dynamic data = CreateWalletDataObject();
            data.inner_address = innerAddress;
            data.amount = amount;
            data.min_coin_amount = minCoinAmount;
            data.max_coin_amount = maxCoinAmount;
            data.fee = fee;
            data.reuse_puzhash = reusePuzhash;
            if (memos != null)
            {
                data.memos = memos.ToList();
            }
            if (excludeCoinAmounts != null)
            {
                data.exclude_coin_ids = excludeCoinAmounts.ToList();
            }

            return await WalletProxy.SendMessage<TransactionRecord>("cat_spend", data, "transaction", cancellationToken).ConfigureAwait(false);
        }
    }
}
