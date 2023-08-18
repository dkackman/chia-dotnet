using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace chia.dotnet
{
    /// <summary>
    /// Wraps a Data Layer Wallet
    /// </summary>
    public sealed class DataLayerWallet : Wallet
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="walletId">The wallet_id to wrap</param>
        /// <param name="walletProxy">Wallet RPC proxy to use for communication</param>
        public DataLayerWallet(uint walletId, WalletProxy walletProxy)
            : base(walletId, walletProxy)
        {
        }

        /// <summary>
        /// Validates that <see cref="Wallet.WalletId"/> is a <see cref="WalletType.DATA_LAYER"/>
        /// </summary>
        /// <returns>True if the wallet is a Data Layer wallet</returns>
        public override async Task Validate(CancellationToken cancellationToken = default)
        {
            await Validate(WalletType.DATA_LAYER, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Remove an existing mirror for a specific singleton.
        /// </summary>
        /// <param name="coinId"></param>
        /// <param name="fee">Fee (in units of mojos)</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>A list of <see cref="TransactionRecord"/></returns>
        public async Task<IEnumerable<TransactionRecord>> DeleteMirror(string coinId, ulong fee = 0, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.coin_id = coinId;
            data.fee = fee;
            return await WalletProxy.SendMessage<IEnumerable<TransactionRecord>>("dl_delete_mirror", data, "transactions", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Get all of the mirrors for a specific singleton.
        /// </summary>
        /// <param name="launcherId"></param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>A list of <see cref="Mirror"/></returns>
        public async Task<IEnumerable<Mirror>> GetMirrors(string launcherId, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.launcher_id = launcherId;
            return await WalletProxy.SendMessage<IEnumerable<Mirror>>("dl_get_mirrors", data, "mirrors", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Get the singleton record for the latest singleton of a launcher ID.
        /// </summary>
        /// <param name="launcherId"></param>
        /// <param name="minGeneration"></param>
        /// <param name="maxGeneration"></param>
        /// <param name="numResults"></param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>A list of <see cref="SingletonRecord"/></returns>
        public async Task<IEnumerable<SingletonRecord>> History(string launcherId, uint? minGeneration = null, uint? maxGeneration = null, uint? numResults = null, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.launcher_id = launcherId;
            data.min_generation = minGeneration;
            data.max_generation = maxGeneration;
            data.num_results = numResults;

            return await WalletProxy.SendMessage<IEnumerable<SingletonRecord>>("dl_history", data, "history", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Get the singleton records that contain the specified root.
        /// </summary>
        /// <param name="root"></param>
        /// <param name="launcherId"></param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns><see cref="LineageProof"/></returns>
        public async Task<LineageProof> LatestSingleton(string root, string launcherId, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.launcher_id = launcherId;
            data.root = root;
            return await WalletProxy.SendMessage<LineageProof>("dl_latest_singleton", data, "singleton", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Add a new on chain message for a specific singleton.
        /// </summary>
        /// <param name="launcherId"></param>
        /// <param name="amount"></param>
        /// <param name="urls"></param>
        /// <param name="fee">Fee (in units of mojos)</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>A list of <see cref="TransactionRecord"/></returns>
        public async Task<IEnumerable<TransactionRecord>> NewMirror(string launcherId, ulong amount, IEnumerable<string> urls, ulong fee = 0, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.launcher_id = launcherId;
            data.amount = amount;
            data.urls = urls.ToList();
            data.fee = fee;
            return await WalletProxy.SendMessage<IEnumerable<TransactionRecord>>("dl_new_mirror", data, "transactions", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Get all owned singleton records.
        /// </summary>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>A list of <see cref="SingletonRecord"/></returns>
        public async Task<IEnumerable<SingletonRecord>> OwnedSingletons(CancellationToken cancellationToken = default)
        {
            return await WalletProxy.SendMessage<IEnumerable<SingletonRecord>>("dl_owned_singletons", null, "singletons", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Get the singleton records that contain the specified root.
        /// </summary>
        /// <param name="root"></param>
        /// <param name="launcherId"></param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>A list of <see cref="SingletonRecord"/></returns>
        public async Task<IEnumerable<SingletonRecord>> SingletonsByRoot(string root, string launcherId, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.launcher_id = launcherId;
            data.root = root;
            return await WalletProxy.SendMessage<IEnumerable<SingletonRecord>>("dl_singletons_by_root", data, "singletons", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Stop tracking the data layer wallets.
        /// </summary>
        /// <param name="launcherId"></param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>An awaitable Task</returns>
        public async Task StopTracking(string launcherId, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.launcher_id = launcherId;
            await WalletProxy.SendMessage("dl_stop_tracking", data, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Track the new data layer wallet
        /// </summary>
        /// <param name="launcherId"></param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>An awaitable Task</returns>
        public async Task TrackNew(string launcherId, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.launcher_id = launcherId;
            await WalletProxy.SendMessage("dl_track_new", data, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Update multiple singletons with new merkle roots
        /// </summary>
        /// <param name="updates"></param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>A list of <see cref="TransactionRecord"/></returns>
        public async Task<IEnumerable<TransactionRecord>> UpdateMultiple(IEnumerable<SingletonInfo> updates, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.updates = updates.ToList();
            return await WalletProxy.SendMessage<IEnumerable<TransactionRecord>>("dl_update_multiple", data, "tx_records", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Update a data layer root.
        /// </summary>
        /// <param name="newRoot"></param>
        /// <param name="launcherId"></param>
        /// <param name="fee">Fee (in units of mojos)</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns><see cref="TransactionRecord"/></returns>
        public async Task<TransactionRecord> UpdateRoot(string newRoot, string launcherId, ulong fee = 0, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.launcher_id = launcherId;
            data.new_root = newRoot;
            data.fee = fee;
            return await WalletProxy.SendMessage<TransactionRecord>("dl_update_root", data, "tx_record", cancellationToken).ConfigureAwait(false);
        }
    }
}
