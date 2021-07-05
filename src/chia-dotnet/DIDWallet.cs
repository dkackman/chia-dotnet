using System;
using System.Collections.Generic;
using System.Numerics;
using System.Dynamic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace chia.dotnet
{
    /// <summary>
    /// Wraps a Distributed Identity Wallet
    /// </summary>
    public sealed class DIDWallet
    {
        private readonly WalletProxy _walletProxy;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="walletId">The wallet_id to wrap</param>
        /// <param name="walletProxy">Wallet RPC proxy to use for communication</param>
        public DIDWallet(uint walletId, WalletProxy walletProxy)
        {
            WalletId = walletId;
            _walletProxy = walletProxy ?? throw new ArgumentNullException(nameof(walletProxy));
        }

        /// <summary>
        /// The id of the wallet
        /// </summary>
        public uint WalletId { get; init; }

        /// <summary>
        /// Login to the wallet
        /// </summary>
        /// <remarks>Always login before interacting with the wallet. Logged in state is kept on the serve so might have changed</remarks>
        /// <returns>an awaitable <see cref="Task"/></returns>
        public async Task<uint> Login(CancellationToken cancellationToken)
        {
            var fingerprints = await _walletProxy.GetPublicKeys(cancellationToken);

            // not 100% sure this applies in all cases but wallets seem to come back in id order
            // haven't figured out a different or better way to get a fingerprint from an id
            var fingerprint = fingerprints.First();

            return await _walletProxy.LogIn(fingerprint, true, cancellationToken);
        }

        /// <summary>
        /// Updates recovery ID's
        /// </summary>
        /// <param name="newList">The new ids</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>An awaitable <see cref="Task"/></returns>
        public async Task UpdateRecoveryIds(IEnumerable<string> newList, CancellationToken cancellationToken)
        {
            dynamic data = new ExpandoObject();
            data.wallet_id = WalletId;
            data.new_list = newList.ToList();

            _ = await _walletProxy.SendMessage("did_update_recovery_ids", data, cancellationToken);
        }

        /// <summary>
        /// Updates recovery ID's
        /// </summary>
        /// <param name="newList">The new ids</param>
        /// <param name="numVerificationsRequired">The number of verifications required</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>An awaitable <see cref="Task"/></returns>
        public async Task UpdateRecoveryIds(IEnumerable<string> newList, BigInteger numVerificationsRequired, CancellationToken cancellationToken)
        {
            dynamic data = new ExpandoObject();
            data.wallet_id = WalletId;
            data.new_list = newList.ToList();
            data.num_verifications_required = numVerificationsRequired;

            _ = await _walletProxy.SendMessage("did_update_recovery_ids", data, cancellationToken);
        }

        /// <summary>
        /// Spend form the DID wallet
        /// </summary>
        /// <param name="puzzlehash">The puzzlehash to spend</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>An awaitable <see cref="Task"/></returns>
        public async Task Spend(string puzzlehash, CancellationToken cancellationToken)
        {
            dynamic data = new ExpandoObject();
            data.wallet_id = WalletId;
            data.puzzlehash = puzzlehash;

            _ = await _walletProxy.SendMessage("did_spend", data, cancellationToken);
        }

        /// <summary>
        /// Get the distributed identiy and coin if present
        /// </summary>
        /// <param name="puzzlehash">The puzzlehash to spend</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>A DID and optional CoinID</returns>
        public async Task<(string MyDID, string CoinID)> GetDID(CancellationToken cancellationToken)
        {
            dynamic data = new ExpandoObject();
            data.wallet_id = WalletId;

            var response = await _walletProxy.SendMessage("did_get_did", data, cancellationToken);

            // coin_id might be null
            return (response.Data.my_did.ToString(), response.Data.coin_id?.ToString());
        }
    }
}
