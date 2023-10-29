using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace chia.dotnet
{
    /// <summary>
    /// Wraps a CRCAT  Wallet
    /// </summary>
    public sealed class CRCATWallet : Wallet
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="walletId">The wallet_id to wrap</param>
        /// <param name="walletProxy">Wallet RPC proxy to use for communication</param>
        public CRCATWallet(uint walletId, WalletProxy walletProxy)
            : base(walletId, walletProxy)
        {
        }

        /// <summary>
        /// Validates that <see cref="Wallet.WalletId"/> is a <see cref="WalletType.CRCAT"/>
        /// </summary>
        /// <returns>True if the wallet is a pooling wallet</returns>
        public override async Task Validate(CancellationToken cancellationToken = default)
        {
            await Validate(WalletType.CRCAT, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Moving any "pending approval" CR-CATs into the spendable balance of the wallet.
        /// </summary>
        /// <param name="minAmountToClaim">The minimum amount to claim (in units of mojos)</param>
        /// <param name="reusePuzhash"></param>
        /// <param name="fee">Fee (in units of mojos)</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>A list of <see cref="TransactionRecord"/></returns>
        public async Task<IEnumerable<TransactionRecord>> ApprovePending(ulong? minAmountToClaim = null, bool? reusePuzhash = null, ulong fee = 0, CancellationToken cancellationToken = default)
        {
            dynamic data = CreateWalletDataObject();
            data.min_amount_to_claim = minAmountToClaim;
            data.reuse_puzhash = reusePuzhash;
            data.fee = fee;

            return await WalletProxy.SendMessage<IEnumerable<TransactionRecord>>("crcat_approve_pending", data, "transactions", cancellationToken).ConfigureAwait(false);
        }
    }
}
