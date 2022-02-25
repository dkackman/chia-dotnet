using System;
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

            _ = await WalletProxy.SendMessage("cat_set_name", data, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Spend a CAT
        /// </summary>
        /// <param name="innerAddress">The inner address for the spend</param>
        /// <param name="amount">The amount to put in the wallet (in units of mojos)</param> 
        /// <param name="fee">The fee to create the wallet (in units of mojos)</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>A <see cref="TransactionRecord"/></returns>
        public async Task<TransactionRecord> Spend(string innerAddress, ulong amount, ulong fee, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(innerAddress))
            {
                throw new ArgumentNullException(nameof(innerAddress));
            }

            dynamic data = CreateWalletDataObject();
            data.inner_address = innerAddress;
            data.amount = amount;
            data.fee = fee;

            return await WalletProxy.SendMessage<TransactionRecord>("cat_spend", data, "transaction", cancellationToken).ConfigureAwait(false);
        }
    }
}
