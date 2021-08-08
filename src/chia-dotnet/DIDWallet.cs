using System.Collections.Generic;
using System.Dynamic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace chia.dotnet
{
    /// <summary>
    /// Wraps a Distributed Identity Wallet
    /// </summary>
    public sealed class DIDWallet : Wallet
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="walletId">The wallet_id to wrap</param>
        /// <param name="walletProxy">Wallet RPC proxy to use for communication</param>
        public DIDWallet(uint walletId, WalletProxy walletProxy)
            : base(walletId, walletProxy)
        {
        }

        /// <summary>
        /// Updates recovery ID's
        /// </summary>
        /// <param name="newList">The new ids</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>An awaitable <see cref="Task"/></returns>
        public async Task UpdateRecoveryIds(IEnumerable<string> newList, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.wallet_id = WalletId;
            data.new_list = newList.ToList();

            _ = await WalletProxy.SendMessage("did_update_recovery_ids", data, cancellationToken);
        }

        /// <summary>
        /// Updates recovery ID's
        /// </summary>
        /// <param name="newList">The new ids</param>
        /// <param name="numVerificationsRequired">The number of verifications required</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>An awaitable <see cref="Task"/></returns>
        public async Task UpdateRecoveryIds(IEnumerable<string> newList, ulong numVerificationsRequired, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.wallet_id = WalletId;
            data.new_list = newList.ToList();
            data.num_verifications_required = numVerificationsRequired;

            _ = await WalletProxy.SendMessage("did_update_recovery_ids", data, cancellationToken);
        }

        /// <summary>
        /// Spend from the DID wallet
        /// </summary>
        /// <param name="puzzlehash">The puzzlehash to spend</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>An awaitable <see cref="Task"/></returns>
        public async Task Spend(string puzzlehash, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.wallet_id = WalletId;
            data.puzzlehash = puzzlehash;

            _ = await WalletProxy.SendMessage("did_spend", data, cancellationToken);
        }

        /// <summary>
        /// Get the distributed identity and coin if present
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>A DID and optional CoinID</returns>
        public async Task<(string MyDID, string CoinID)> GetDID(CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.wallet_id = WalletId;

            var response = await WalletProxy.SendMessage("did_get_did", data, cancellationToken);

            // coin_id might be null
            return (response.my_did.ToString(), response.coin_id?.ToString());
        }

        /// <summary>
        /// Get the wallet pubkey
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>The pubkey</returns>
        public async Task<string> GetPubKey(CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.wallet_id = WalletId;

            var response = await WalletProxy.SendMessage("did_get_pubkey", data, cancellationToken);

            return response.pubkey.ToString();
        }

        /// <summary>
        /// Recovery spend
        /// </summary>
        /// <param name="attestFilenames">List of attest files. Must be >= num_of_backup_ids_needed</param>
        /// <param name="pubkey">The public key</param>
        /// <param name="puzzlehash">The puzzlehash of the spend</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>An awaitable <see cref="Task"/></returns>
        public async Task RecoverySpend(IEnumerable<string> attestFilenames, string pubkey, string puzzlehash, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.wallet_id = WalletId;
            data.attest_filenames = attestFilenames.ToList();
            if (!string.IsNullOrEmpty(pubkey))
            {
                data.pubkey = pubkey;
            }
            if (!string.IsNullOrEmpty(puzzlehash))
            {
                data.puzhash = puzzlehash;
            }

            _ = await WalletProxy.SendMessage("did_recovery_spend", data, cancellationToken);
        }

        /// <summary>
        /// Get the recover list
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>The recover list and num required property of the wallet</returns>
        public async Task<(IEnumerable<string> RecoverList, int NumRequired)> GetRecoveryList(CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.wallet_id = WalletId;

            var response = await WalletProxy.SendMessage("did_get_recovery_list", data, cancellationToken);

            return (Converters.ToStrings(response.recover_list), response.num_required);
        }

        /// <summary>
        /// Create an attest file
        /// </summary>
        /// <param name="filename">file name of the attest</param>
        /// <param name="coinName">The coin name</param>
        /// <param name="pubkey">The public key</param>
        /// <param name="puzHash">The puzzlehash</param>        
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>A spendbundle and information about the attest</returns>
        public async Task<(string MessageSpendBundle, dynamic Info)> CreateAttest(string filename, string coinName, string pubkey, string puzHash, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.wallet_id = WalletId;
            data.filename = filename;
            data.coin_name = coinName;
            data.pubkey = pubkey;
            data.puzhash = puzHash;

            var response = await WalletProxy.SendMessage("did_create_attest", data, cancellationToken);

            return (response.message_spend_bundle, response.info);
        }

        /// <summary>
        /// Create an attest file
        /// </summary>       
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>A spendbundle and information about the attest</returns>
        public async Task<(string MyDID, string CoinName, string NewPuzzleHash, string PublicKey, IEnumerable<dynamic> BackUpIds)> GetInformationNeededForRecovery(CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.wallet_id = WalletId;

            var response = await WalletProxy.SendMessage("did_get_information_needed_for_recovery", data, cancellationToken);

            return (response.my_did, response.coin_name, response.newpuzhash, response.pubkey, response.backup_dids);
        }

        /// <summary>
        /// Create a backup file of the wallet
        /// </summary>
        /// <param name="filename">The filename ot create</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>An awaitable <see cref="Task"/></returns>
        public async Task CreateBackupFile(IEnumerable<string> filename, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.attest_filenames = WalletId;
            data.filename = filename;

            _ = await WalletProxy.SendMessage("did_create_backup_file", data, cancellationToken);
        }
    }
}
