using System;
using System.Threading;
using System.Threading.Tasks;

namespace chia.dotnet
{
    /// <summary>
    /// Wraps a Coloured Coin wallet
    /// </summary>
    public sealed class ColouredCoinWallet : Wallet
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="walletId">The wallet_id to wrap</param>
        /// <param name="walletProxy">Wallet RPC proxy to use for communication</param>
        public ColouredCoinWallet(uint walletId, WalletProxy walletProxy)
            : base(walletId, walletProxy)
        {
        }

        /// <summary>
        /// Validates that <see cref="Wallet.WalletId"/> is a <see cref="WalletType.COLOURED_COIN"/>
        /// </summary>
        /// <returns>True if the wallet is a CC wallet</returns>
        public override async Task Validate(CancellationToken cancellationToken = default)
        {
            await Validate(WalletType.COLOURED_COIN, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Get the name of a wallet's coloured coin
        /// </summary>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>The coin name</returns>
        public async Task<string> GetName(CancellationToken cancellationToken = default)
        {
            var response = await WalletProxy.SendMessage("cc_get_name", CreateWalletDataObject(), cancellationToken).ConfigureAwait(false);

            return response.name.ToString();
        }

        /// <summary>
        /// Set the name of a wallet's coloured coin
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

            _ = await WalletProxy.SendMessage("cc_set_name", data, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Get the colour of a wallet's coloured coin
        /// </summary>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>The colour as a string</returns>
        public async Task<string> GetColour(CancellationToken cancellationToken = default)
        {
            var response = await WalletProxy.SendMessage("cc_get_colour", CreateWalletDataObject(), cancellationToken).ConfigureAwait(false);

            return response.colour;
        }

        /// <summary>
        /// Spend a coloured coin
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

            return await WalletProxy.SendMessage<TransactionRecord>("cc_spend", data, "transaction", cancellationToken).ConfigureAwait(false);
        }
    }
}
