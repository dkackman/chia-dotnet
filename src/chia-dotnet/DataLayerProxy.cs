using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace chia.dotnet
{
    /// <summary>
    /// Proxy that communicates with the Data Layer
    /// </summary>
    /// <remarks>
    /// ctor
    /// </remarks>
    /// <param name="rpcClient"><see cref="IRpcClient"/> instance to use for rpc communication</param>
    /// <param name="originService"><see cref="Message.Origin"/></param>
    public sealed class DataLayerProxy(IRpcClient rpcClient, string originService) : ServiceProxy(rpcClient, ServiceNames.DataLayer, originService)
    {
        /// <summary>
        /// Adds a mirror
        /// </summary>
        /// <param name="id">Mirror id</param>
        /// <param name="amount">The Amount</param>
        /// <param name="urls">List of mirror urls</param>
        /// <param name="fee">Fee amount (in units of mojos)</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>An awaitable Task</returns>
        public async Task AddMirror(string id, ulong amount, IEnumerable<string> urls, ulong fee = 0, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.id = id;
            data.amount = amount;
            data.urls = urls.ToList();
            data.fee = fee;
            await SendMessage("add_mirror", data, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Adds missing files
        /// </summary>
        /// <param name="ids">List of file id's</param>
        /// <param name="foldername">The folder name</param>
        /// <param name="overwrite">Indicator whether to overwrite files</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>An awaitable Task</returns>
        public async Task AddMissingFiles(string[] ids, string foldername, bool overwrite = false, CancellationToken cancellationToken = default)
        {
            ArgumentException.ThrowIfNullOrEmpty(foldername, nameof(foldername));

            dynamic data = new ExpandoObject();
            data.ids = ids;
            data.foldername = foldername;
            data.overwrite = overwrite;
            await SendMessage("add_missing_files", data, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Applies a batch of updates.
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="changeList">Name value pairs of changes</param>
        /// <param name="submitOnChain">Indicates to submit the updates on the blockchain</param>
        /// <param name="fee">Fee amount (in units of mojos)</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>Transaction id</returns>
        public async Task<string> BatchUpdate(string id, IDictionary<string, string> changeList, bool submitOnChain = true, ulong fee = 0, CancellationToken cancellationToken = default)
        {
            ArgumentException.ThrowIfNullOrEmpty(id, nameof(id));

            dynamic data = new ExpandoObject();
            data.id = id;
            data.changelist = changeList;
            data.fee = fee;
            data.submit_on_chain = submitOnChain;

            return await SendMessage<string>("batch_update", data, "tx_id", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Cancels an offer using a transaction
        /// </summary>
        /// <param name="tradeId">The trade id of the offer</param>
        /// <param name="fee">Transaction fee</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <param name="secure">This will create a transaction that includes coins that were offered</param>
        /// <returns>An awaitable Task</returns>
        public async Task CancelOffer(string tradeId, bool secure = false, ulong fee = 0, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.trade_id = tradeId;
            data.fee = fee;
            data.secure = secure;
            await SendMessage("cancel_offer", data, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Clears pending roots.
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns><see cref="Root"/></returns>
        public async Task<Root> ClearPendingRoots(string storeId, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.store_id = storeId;
            return await SendMessage<Root>("clear_pending_roots", data, "", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Checks the status of plugins.
        /// </summary>
        /// <returns><see cref="PluginStatus"/></returns>
        public async Task<PluginStatus> CheckPlugins(CancellationToken cancellationToken = default)
        {
            return await SendMessage<PluginStatus>("check_plugins", null, "plugin_status", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Creates a data store.
        /// </summary>
        /// <param name="fee">Fee amount (in units of mojos)</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>The tree id and list of transactions</returns>
        public async Task<(string id, IEnumerable<TransactionRecord> txs)> CreateDataStore(ulong fee = 0, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.fee = fee;
            var response = await SendMessage("create_data_store", data, cancellationToken).ConfigureAwait(false);

            return (
                response.id,
                Converters.ToObject<IEnumerable<TransactionRecord>>(response.txs)
            );
        }

        /// <summary>
        /// Deletes a data store.
        /// </summary>
        /// <param name="key">Row key</param>
        /// <param name="id">Row id</param>
        /// <param name="fee">Fee amount (in units of mojos)</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>Transaction id</returns>
        public async Task<string> DeleteKey(string key, string id, ulong fee = 0, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.key = key;
            data.id = id;
            data.fee = fee;
            return await SendMessage<string>("delete_key", data, "tx_id", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Deletes a mirror.
        /// </summary>
        /// <param name="coinId">Mirror coin id</param>
        /// <param name="fee">Fee amount (in units of mojos)</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns></returns>
        public async Task DeleteMirror(string coinId, ulong fee = 0, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.coin_id = coinId;
            data.fee = fee;
            await SendMessage("delete_mirror", data, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the list of ancestors for a given id/hash pair.
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="hash">Hash</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns></returns>
        public async Task<IEnumerable<InternalNode>> GetAncestors(string id, string hash, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.id = id;
            data.hash = hash;
            return await SendMessage<IEnumerable<InternalNode>>("get_ancestors", data, "ancestors", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the list of ancestors for a given id/hash pair.
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="rootHash">Root Hash</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns></returns>
        public async Task<IEnumerable<string>> GetKeys(string id, string? rootHash, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.id = id;
            data.root_hash = rootHash;
            return await SendMessage<IEnumerable<string>>("get_keys", data, "keys", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Get the keys and values for a given id/root_hash pair.
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="rootHash">Root Hash</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns></returns>
        public async Task<IEnumerable<TerminalNode>> GetKeysValues(string id, string rootHash, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.id = id;
            data.root_hash = rootHash;
            return await SendMessage<IEnumerable<TerminalNode>>("get_keys_values", data, "keys_values", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Get kv diff between two root hashes.
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="hash1">First Hash</param>
        /// <param name="hash2">Second Hash</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns><see cref="KVDiff"/></returns>
        public async Task<KVDiff> GetKVDiff(string id, string hash1, string hash2, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.id = id;
            data.hash1 = hash1;
            data.hash2 = hash2;
            return await SendMessage<KVDiff>("get_kv_diff", data, "diff", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets hash of latest tree root saved in our local datastore.
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>A hash</returns>
        public async Task<KVDiff> GetLocalRoot(string id, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.id = id;
            return await SendMessage<string>("get_local_root", data, "hash", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the mirrors for a given store id.
        /// </summary>
        /// <param name="id">Store Id</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>A list of <see cref="Mirror"></see></returns>
        public async Task<IEnumerable<Mirror>> GetMirrors(string id, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.id = id;
            return await SendMessage<IEnumerable<Mirror>>("get_mirrors", data, "mirrors", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the list of owned store ids.
        /// </summary>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>A list of <see cref="Mirror"></see></returns>
        public async Task<IEnumerable<string>> GetOwnedStores(CancellationToken cancellationToken = default)
        {
            return await SendMessage<IEnumerable<string>>("get_owned_stores", null, "store_ids", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets hash of latest tree root.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>A <see cref="Root"/></returns>
        public async Task<RootHash> GetRoot(string id, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.id = id;
            return await SendMessage<RootHash>("get_root", data, null, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Get history of state hashes for a store.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>A list of <see cref="RootHistory"/></returns>
        public async Task<IEnumerable<RootHistory>> GetRootHistory(string id, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.id = id;
            return await SendMessage<IEnumerable<RootHistory>>("get_root_history", data, "root_history", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets state hashes for a list of roots
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>A list of <see cref="RootHash"/></returns>
        public async Task<IEnumerable<RootHash>> GetRoots(IEnumerable<string> ids, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.ids = ids.ToList();
            return await SendMessage<IEnumerable<RootHash>>("get_roots", data, "root_hashes", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the sync status of a store.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>A list of <see cref="DataLayerSyncStatus"/></returns>
        public async Task<DataLayerSyncStatus> GetSyncStatus(string id, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.id = id;
            return await SendMessage<DataLayerSyncStatus>("get_sync_status", data, "sync_status", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Get the value for a given id/key pair.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="key"></param>
        /// <param name="rootHash"></param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns><see cref="string"/></returns>
        public async Task<string> GetValue(string id, string key, string? rootHash, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.id = id;
            data.key = key;
            data.root_hash = rootHash;
            return await SendMessage<string>("get_value", data, "value", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Adds a list of clvm objects as bytes to add to table.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        /// <param name="fee">Fee (in units of mojos)</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns><see cref="string"/></returns>
        public async Task<string> Insert(string id, string value, ulong fee = 0, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.value = value;
            data.id = id;
            data.fee = fee;
            return await SendMessage<string>("insert", data, "tx_id", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Makes an offer.
        /// </summary>
        /// <param name="maker"></param>
        /// <param name="taker"></param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <param name="fee">Fee (in units of mojos)</param>
        /// <returns><see cref="DataLayerOffer"/></returns>
        public async Task<DataLayerOffer> MakeOffer(IEnumerable<OfferStore> maker, IEnumerable<OfferStore> taker, ulong fee = 0, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.maker = maker.ToList();
            data.taker = taker.ToList();
            data.fee = fee;
            return await SendMessage<DataLayerOffer>("make_offer", data, "offer", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Removes subscriptions for the given id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="urls"></param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>An awaitable Task</returns>
        public async Task RemoveSubscriptions(string id, IEnumerable<string> urls, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.urls = urls.ToList();
            data.id = id;
            await SendMessage("remove_subscriptions", data, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Subscribe to singleton.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="urls"></param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>An awaitable Task</returns>
        public async Task Subscribe(string id, IEnumerable<string> urls, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.urls = urls.ToList();
            data.id = id;
            await SendMessage("subscribe", data, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// List current subscriptions.
        /// </summary>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>A list of <see cref="string"/></returns>
        public async Task<IEnumerable<string>> Subscriptions(CancellationToken cancellationToken = default)
        {
            return await SendMessage<IEnumerable<string>>("subscriptions", null, "store_ids", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Takes an offer.
        /// </summary>
        /// <param name="offer"></param>
        /// <param name="fee">Fee (in units of mojos)</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns><see cref="string"/></returns>
        public async Task<string> TakeOffer(object offer, ulong fee = 0, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.offer = offer;
            data.fee = fee;
            return await SendMessage<string>("take_offer", data, "trade_id", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Unsubscribe from singleton.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="retain"></param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>An awaitable Task</returns>
        public async Task Unsubscribe(string id, bool retain = false, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.id = id;
            data.retain = retain;
            await SendMessage("unsubscribe", data, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Verifies an offer.
        /// </summary>
        /// <param name="offer"></param>
        /// <param name="fee">Fee (in units of mojos)</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>boolean valid flag and fee amount</returns>
        public async Task<(bool Valid, ulong Fee)> VerifyOffer(DataLayerOffer offer, ulong fee = 0, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.offer = offer;
            data.fee = fee;
            var response = await SendMessage("verify_offer", data, cancellationToken).ConfigureAwait(false);
            return (response.valid, response.fee);
        }

        /// <summary>
        /// Sets a key to active.
        /// </summary>
        /// <param name="fingerprint">The fingerprint</param>          
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>An awaitable task</returns>
        public async Task WalletLogIn(uint fingerprint, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.fingerprint = fingerprint;
            await SendMessage("wallet_log_in", data, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Verifies a proof.
        /// </summary>
        /// <param name="proof"></param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>Proof verification</returns>
        public async Task<(bool CurrentRoot, ProofResultInclusions VerifiedClvmHashes)> VerifyProof(DLProof proof, CancellationToken cancellationToken = default)
        {
            dynamic response = await SendMessage("verify_proof", proof, cancellationToken).ConfigureAwait(false);

            return (
                response.current_root,
                Converters.ToObject<ProofResultInclusions>(response.verified_clvm_hashes)
                );
        }

        /// <summary>
        /// Retrieves a proof.
        /// </summary>
        /// <param name="storeId"></param>
        /// <param name="keys"></param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns><see cref="DLProof"/></returns>
        public async Task<DLProof> GetProof(string storeId, IEnumerable<string> keys, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.store_id = storeId;
            data.keys = keys.ToList();

            return await SendMessage<DLProof>("get_proof", data, "proof", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Submits a pending root
        /// </summary>
        /// <param name="storeId">The store id</param>
        /// <param name="fee">Fee (in units of mojos)</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>A transaction id</returns>
        public async Task<string> SubmitPendingRoot(string storeId, ulong fee = 0, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.store_id = storeId;
            data.fee = fee;

            return await SendMessage<string>("submit_pending_root", data, "tx_id", cancellationToken).ConfigureAwait(false);
        }
    }
}
