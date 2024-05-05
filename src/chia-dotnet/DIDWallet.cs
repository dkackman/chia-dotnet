using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace chia.dotnet
{
    /// <summary>
    /// Wraps a Distributed Identity Wallet
    /// </summary>
    /// <remarks>
    /// ctor
    /// </remarks>
    /// <param name="walletId">The wallet_id to wrap</param>
    /// <param name="walletProxy">Wallet RPC proxy to use for communication</param>
    public sealed class DIDWallet(uint walletId, WalletProxy walletProxy) : Wallet(walletId, walletProxy)
    {
        /// <summary>
        /// Validates that <see cref="Wallet.WalletId"/> is a <see cref="WalletType.DISTRIBUTED_ID"/>
        /// </summary>
        /// <returns>True if the wallet is a DID wallet</returns>
        public override async Task Validate(CancellationToken cancellationToken = default)
        {
            await Validate(WalletType.DISTRIBUTED_ID, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Updates recovery ID's
        /// </summary>
        /// <param name="newList">The new ids</param>
        /// <param name="numVerificationsRequired">The number of verifications required</param>
        /// <param name="reusePuzhash"></param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>A ist of <see cref="TransactionRecord"/>s</returns>
        public async Task<IEnumerable<TransactionRecord>> UpdateRecoveryIds(IEnumerable<string> newList, ulong? numVerificationsRequired = null, bool? reusePuzhash = null, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(newList);

            dynamic data = CreateWalletDataObject();
            data.new_list = newList.ToList();
            data.num_verifications_required = numVerificationsRequired;
            data.reuse_puzhash = reusePuzhash;

            return await WalletProxy.SendMessage<IEnumerable<TransactionRecord>>("did_update_recovery_ids", data, "transactions", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Updates recovery ID's
        /// </summary>
        /// <param name="newList">The new ids</param>
        /// <param name="numVerificationsRequired">The number of verifications required</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>A ist of <see cref="TransactionRecord"/>s</returns>
        public async Task<IEnumerable<TransactionRecord>> UpdateRecoveryIds(IEnumerable<string> newList, ulong numVerificationsRequired, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(newList);

            dynamic data = CreateWalletDataObject();
            data.new_list = newList.ToList();
            data.num_verifications_required = numVerificationsRequired;

            return await WalletProxy.SendMessage<IEnumerable<TransactionRecord>>("did_update_recovery_ids", data, "transactions", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Spend from the DID wallet
        /// </summary>
        /// <param name="puzzlehash">The puzzle-hash to spend</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>An awaitable <see cref="Task"/></returns>
        public async Task Spend(string puzzlehash, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(puzzlehash))
            {
                throw new ArgumentNullException(nameof(puzzlehash));
            }

            dynamic data = CreateWalletDataObject();
            data.puzzlehash = puzzlehash;

            await WalletProxy.SendMessage("did_spend", data, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Get the distributed identity and coin if present
        /// </summary>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>A DID and optional CoinID</returns>
        public async Task<(string MyDid, string? CoinID)> GetDid(CancellationToken cancellationToken = default)
        {
            var response = await WalletProxy.SendMessage("did_get_did", CreateWalletDataObject(), cancellationToken).ConfigureAwait(false);

            // coin_id might be null
            return (response.my_did.ToString(), response.coin_id?.ToString());
        }

        /// <summary>
        /// Gets information about the DID wallets current coin
        /// </summary>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>The coin info</returns>
        public async Task<(string MyDid, string Parent, string InnerPuzzle, ulong Amount)> GetCurrentCoinInfo(CancellationToken cancellationToken = default)
        {
            var response = await WalletProxy.SendMessage("did_get_current_coin_info", CreateWalletDataObject(), cancellationToken).ConfigureAwait(false);

            return (
                response.my_did,
                response.did_parent,
                response.did_innerpuz,
                response.did_amount
                );
        }

        /// <summary>
        /// Get the wallet pubkey
        /// </summary>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>The pubkey</returns>
        public async Task<string> GetPubKey(CancellationToken cancellationToken = default)
        {
            return await WalletProxy.SendMessage<string>("did_get_pubkey", CreateWalletDataObject(), "pubkey", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Get the wallet name
        /// </summary>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>The name</returns>
        public async Task<string> GetName(CancellationToken cancellationToken = default)
        {
            return await WalletProxy.SendMessage<string>("did_get_wallet_name", CreateWalletDataObject(), "name", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Spends a DID message.
        /// </summary>
        /// <param name="puzzleAnnouncements"></param>
        /// <param name="coinAnnouncements"></param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns><see cref="SpendBundle"/></returns>
        public async Task<SpendBundle> MessageSpend(IEnumerable<string> puzzleAnnouncements, IEnumerable<string> coinAnnouncements, CancellationToken cancellationToken = default)
        {
            dynamic data = CreateWalletDataObject();
            data.coin_announcements = coinAnnouncements.ToList();
            data.puzzle_announcements = puzzleAnnouncements.ToList();
            return await WalletProxy.SendMessage<SpendBundle>("did_message_spend", data, "spend_bundle", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Sets the name
        /// </summary>
        /// <param name="name">The name</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>An awaitable <see cref="Task"/></returns>
        public async Task SetName(string name, CancellationToken cancellationToken = default)
        {
            dynamic data = CreateWalletDataObject();
            data.name = name;

            await WalletProxy.SendMessage("did_set_wallet_name", data, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Updates the metadata
        /// </summary>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>The metadata</returns>
        public async Task<IDictionary<string, string>> GetMetadata(CancellationToken cancellationToken = default)
        {
            var response = await WalletProxy.SendMessage("did_get_metadata", CreateWalletDataObject(), cancellationToken).ConfigureAwait(false);

            return Converters.ToObject<IDictionary<string, string>>(response.metadata);
        }

        /// <summary>
        /// Updates the metadata
        /// </summary>
        /// <param name="metadata">The name</param>
        /// <param name="reusePuzhash"></param>
        /// <param name="fee">Fee (in units of mojos)</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>An <see cref="SpendBundle"/></returns>
        public async Task<(SpendBundle SpendBundle, IEnumerable<TransactionRecord> Transactions)> UpdateMetadata(string metadata, bool? reusePuzhash = null, ulong fee = 0, CancellationToken cancellationToken = default)
        {
            dynamic data = CreateWalletDataObject();
            data.metadata = metadata;
            data.reuse_puzhash = reusePuzhash;
            data.fee = fee;

            var response = await WalletProxy.SendMessage("did_update_metadata", data, cancellationToken).ConfigureAwait(false);

            return (
                Converters.ToObject<SpendBundle>(response.spend_bundle),
                Converters.ToObject<IEnumerable<TransactionRecord>>(response.transactions)
                );
        }

        /// <summary>
        /// Recovery spend
        /// </summary>
        /// <param name="attestData">List of attest messages. Must be >= num_of_backup_ids_needed</param>
        /// <param name="pubkey">The public key</param>
        /// <param name="puzzlehash">The puzzlehash of the spend</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>An <see cref="SpendBundle"/></returns>
        public async Task<(SpendBundle SpendBundle, IEnumerable<TransactionRecord> Transactions)> RecoverySpend(IEnumerable<string> attestData, string? pubkey, string? puzzlehash, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(attestData);

            dynamic data = CreateWalletDataObject();
            data.attest_data = attestData.ToList();
            data.pubkey = pubkey;
            data.puzhash = puzzlehash;

            var response = await WalletProxy.SendMessage("did_recovery_spend", data, cancellationToken).ConfigureAwait(false);

            return (
                Converters.ToObject<SpendBundle>(response.spend_bundle),
                Converters.ToObject<IEnumerable<TransactionRecord>>(response.transactions)
                );
        }

        /// <summary>
        /// Get the recover list
        /// </summary>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>The recover list and num required property of the wallet</returns>
        public async Task<(IEnumerable<string> RecoverList, int NumRequired)> GetRecoveryList(CancellationToken cancellationToken = default)
        {
            var response = await WalletProxy.SendMessage("did_get_recovery_list", CreateWalletDataObject(), cancellationToken).ConfigureAwait(false);

            return (Converters.ToObject<IEnumerable<string>>(response.recovery_list), response.num_required);
        }

        /// <summary>
        /// Create an attest file
        /// </summary>
        /// <param name="coinName">The coin name</param>
        /// <param name="pubkey">The public key</param>
        /// <param name="puzHash">The puzzlehash</param>        
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>A spendbundle and information about the attest</returns>
        public async Task<(string MessageSpendBundle, (string Parent, string InnerPuzzleHash, ulong Amount) Info, string AttestData, IEnumerable<TransactionRecord> Transactions)>
            CreateAttest(string coinName, string pubkey, string puzHash, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(coinName))
            {
                throw new ArgumentNullException(nameof(coinName));
            }

            if (string.IsNullOrEmpty(pubkey))
            {
                throw new ArgumentNullException(nameof(pubkey));
            }

            if (string.IsNullOrEmpty(puzHash))
            {
                throw new ArgumentNullException(nameof(puzHash));
            }

            dynamic data = CreateWalletDataObject();
            data.coin_name = coinName;
            data.pubkey = pubkey;
            data.puzhash = puzHash;

            var response = await WalletProxy.SendMessage("did_create_attest", data, cancellationToken).ConfigureAwait(false);

            return (
                response.message_spend_bundle, (
                    response.info[0],
                    response.info[1],
                    response.info[2]
                    ),
                    response.attest_data,
                    Converters.ToObject<IEnumerable<TransactionRecord>>(response.transactions)
                );
        }

        /// <summary>
        /// Create an attestment
        /// </summary>       
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>A spendbundle and information about the attest</returns>
        public async Task<(string MyDID, string CoinName, string NewPuzzleHash, string PublicKey, IEnumerable<byte> BackUpIds)> GetInformationNeededForRecovery(CancellationToken cancellationToken = default)
        {
            var response = await WalletProxy.SendMessage("did_get_information_needed_for_recovery", CreateWalletDataObject(), cancellationToken).ConfigureAwait(false);

            return (
                response.my_did,
                response.coin_name,
                response.newpuzhash,
                response.pubkey,
                Converters.ToEnumerable<byte>(response.backup_dids)
                );
        }

        /// <summary>
        /// Create a backup of the wallet
        /// </summary>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>The backup data</returns>
        public async Task<string> CreateBackupFile(CancellationToken cancellationToken = default)
        {
            dynamic data = CreateWalletDataObject();

            return await WalletProxy.SendMessage<string>("did_create_backup_file", data, "backup_data", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Transfer the DID wallet to another owner
        /// </summary>
        /// <param name="innerAddress">the address</param>
        /// <param name="withRecoveryInfo">Indicator whether to include recovery info</param>
        /// <param name="reusePuzhash"></param>
        /// <param name="fee">Fee (in units of mojos)</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>The backup data</returns>
        public async Task<(TransactionRecord Transaction, IEnumerable<TransactionRecord> Transactions)> Transfer(string innerAddress, bool withRecoveryInfo = true, bool? reusePuzhash = null, ulong fee = 0, CancellationToken cancellationToken = default)
        {
            dynamic data = CreateWalletDataObject();
            data.inner_address = innerAddress;
            data.with_recovery_info = withRecoveryInfo;
            data.reuse_puzhash = reusePuzhash;
            data.fee = fee;

            var response = await WalletProxy.SendMessage("did_transfer_did", data, cancellationToken).ConfigureAwait(false);

            return (
                Converters.ToObject<TransactionRecord>(response.transaction),
                Converters.ToObject<IEnumerable<TransactionRecord>>(response.transactions)
                );
        }
    }
}
