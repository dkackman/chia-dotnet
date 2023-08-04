﻿using System.Collections.Generic;
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
        /// Initialize the new data layer wallets.
        /// </summary>
        /// <param name="root"></param>
        /// <param name="fee"></param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns><see cref=""/></returns>
        public async Task<(IEnumerable<TransactionRecord> Transactions, string LauncherId)> CreateNewDl(string root, ulong fee = 0, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.root = root;
            data.fee = fee;
            var response = await WalletProxy.SendMessage("create_new_dl", data, cancellationToken).ConfigureAwait(false);
            return (Converters.ToEnumerable<TransactionRecord>(response.transactions), response.launcher_id);
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
            if (minGeneration.HasValue)
            {
                data.min_generation = minGeneration;
            }
            if (maxGeneration.HasValue)
            {
                data.max_generation = maxGeneration;
            }
            if (numResults.HasValue)
            {
                data.num_results = numResults;
            }
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
        /// Get all owned singleton records.
        /// </summary>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>A list of <see cref="SingletonRecord"/></returns>
        public async Task<IEnumerable<SingletonRecord>> DlOwnedSingletons(CancellationToken cancellationToken = default)
        {
            return await SendMessage<IEnumerable<SingletonRecord>>("dl_owned_singletons", null, "history", cancellationToken).ConfigureAwait(false);
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
            return await WalletProxy.SendMessage<IEnumerable<SingletonRecord>>("dl_singletons_by_root", data, "singleton", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Stop tracking the data layer wallets.
        /// </summary>
        /// <param name="launcherId"></param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns><see cref=""/></returns>
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
        /// <returns><see cref=""/></returns>
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
        /// <param name="fee"></param>
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