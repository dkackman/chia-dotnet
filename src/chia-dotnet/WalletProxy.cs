﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;

namespace chia.dotnet
{
    /// <summary>
    /// Proxy that communicates with the wallet endpoint
    /// </summary>
    public sealed class WalletProxy : ServiceProxy
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="rpcClient"><see cref="IRpcClient"/> instance to use for rpc communication</param>
        /// <param name="originService"><see cref="Message.Origin"/></param>
        public WalletProxy(IRpcClient rpcClient, string originService)
            : base(rpcClient, ServiceNames.Wallet, originService)
        {
        }

        /// <summary>
        /// The fingerprint used to login to the wallet.
        /// </summary>
        /// <remarks>Will be null until <see cref="LogIn(CancellationToken)"/> is called</remarks>
        public uint? Fingerprint { get; private set; }

        /// <summary>
        /// Sets the first key to active.
        /// </summary>       
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>The key fingerprint</returns>
        public async Task<uint> LogIn(CancellationToken cancellationToken = default)
        {
            var fingerprints = await GetPublicKeys(cancellationToken).ConfigureAwait(false);

            return fingerprints.Any()
                ? await LogIn(fingerprints.First(), cancellationToken).ConfigureAwait(false)
                : throw new InvalidOperationException("There are no public keys present'");
        }

        /// <summary>
        /// Sets a key to active.
        /// </summary>
        /// <param name="fingerprint">The fingerprint</param>          
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>The key fingerprint</returns>
        public async Task<uint> LogIn(uint fingerprint, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.fingerprint = fingerprint;

            var response = await SendMessage("log_in", data, cancellationToken).ConfigureAwait(false);

            Fingerprint = (uint)response.fingerprint;
            return Fingerprint.Value;
        }

        /// <summary>
        /// Get the list of wallets
        /// </summary>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>The list of wallets</returns>
        public async Task<(IEnumerable<WalletInfo> Wallets, uint Fingerprint)> GetWallets(bool includeData = true, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.include_data = includeData;

            var response = await SendMessage("get_wallets", data, cancellationToken).ConfigureAwait(false);

            return (Converters.ToObject<IEnumerable<WalletInfo>>(response.wallets), response.fingerprint);
        }

        /// <summary>
        /// Get the list of wallets
        /// </summary>
        /// <param name="type">Return only wallets of this type</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>The list of wallets</returns>
        public async Task<IEnumerable<WalletInfo>> GetWallets(WalletType type, bool includeData = true, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.type = type;
            data.include_data = includeData;

            return await SendMessage<IEnumerable<WalletInfo>>("get_wallets", data, "wallets", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Get all root public keys accessible by the wallet
        /// </summary>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>All root public keys accessible by the wallet</returns>
        public async Task<IEnumerable<uint>> GetPublicKeys(CancellationToken cancellationToken = default)
        {
            var response = await SendMessage("get_public_keys", cancellationToken).ConfigureAwait(false);

            return Converters.ToEnumerable<uint>(response.public_key_fingerprints);
        }

        /// <summary>
        /// Retrieves the logged in fingerprint
        /// </summary>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>The logged in fingerprint</returns>
        public async Task<uint?> GetLoggedInFingerprint(CancellationToken cancellationToken = default)
        {
            return await SendMessage<uint?>("get_logged_in_fingerprint", null, "fingerprint", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Get the private key accessible by the wallet
        /// </summary>
        /// <param name="fingerprint">The fingerprint</param>          
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>The private key for the fingerprint</returns>
        public async Task<PrivateKey> GetPrivateKey(uint fingerprint, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.fingerprint = fingerprint;

            return await SendMessage<PrivateKey>("get_private_key", data, "private_key", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Get the wallet's sync status
        /// </summary>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>The current sync status</returns>
        public async Task<(bool GenesisInitialized, bool Synced, bool Syncing)> GetSyncStatus(CancellationToken cancellationToken = default)
        {
            var response = await SendMessage("get_sync_status", cancellationToken).ConfigureAwait(false);

            return (
                response.genesis_initialized,
                response.synced,
                response.syncing
                );
        }

        /// <summary>
        /// Retrieves information about the current network
        /// </summary>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>The current network name and prefix</returns>
        public async Task<(string NetworkName, string NetworkPrefix)> GetNetworkInfo(CancellationToken cancellationToken = default)
        {
            var response = await SendMessage("get_network_info", cancellationToken).ConfigureAwait(false);

            return (response.network_name, response.network_prefix);
        }

        /// <summary>
        /// Get blockchain height info
        /// </summary>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>Current block height</returns>
        public async Task<uint> GetHeightInfo(CancellationToken cancellationToken = default)
        {
            return await SendMessage<uint>("get_height_info", "height", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Get a specific transaction
        /// </summary>
        /// <param name="transactionId">The id of the transaction to find</param> 
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>The <see cref="TransactionRecord"/></returns>
        public async Task<TransactionRecord> GetTransaction(string transactionId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(transactionId))
            {
                throw new ArgumentNullException(nameof(transactionId));
            }

            dynamic data = new ExpandoObject();
            data.transaction_id = transactionId;

            return await SendMessage<TransactionRecord>("get_transaction", data, "transaction", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Pushes a transaction / spend bundle to the mempool and blockchain. 
        /// Returns whether the spend bundle was successfully included into the mempool
        /// </summary>
        /// <param name="spendBundle"></param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>Indicator of whether the spend bundle was successfully included in the mempool</returns>
        public async Task<bool> PushTx(SpendBundle spendBundle, CancellationToken cancellationToken = default)
        {
            if (spendBundle is null)
            {
                throw new ArgumentNullException(nameof(spendBundle));
            }

            dynamic data = new ExpandoObject();
            data.spend_bundle = spendBundle;

            var response = await SendMessage("push_tx", data, cancellationToken).ConfigureAwait(false);

            return response.status?.ToString() == "SUCCESS";
        }

        /// <summary>
        /// Pushes a list of transactions to the mempool and blockchain. 
        /// </summary>
        /// <param name="spendBundle"></param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>An awaitable task</returns>
        public async Task PushTransactions(IEnumerable<TransactionRecord> transactions, CancellationToken cancellationToken = default)
        {
            if (transactions is null)
            {
                throw new ArgumentNullException(nameof(transactions));
            }

            dynamic data = new ExpandoObject();
            data.transactions = transactions.ToList();

            await SendMessage("push_transactions", data, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Adds a new key to the wallet
        /// </summary>        
        /// <param name="mnemonic">The key mnemonic</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>The new key's fingerprint</returns>
        public async Task<uint> AddKey(IEnumerable<string> mnemonic, CancellationToken cancellationToken = default)
        {
            if (mnemonic is null)
            {
                throw new ArgumentNullException(nameof(mnemonic));
            }

            dynamic data = new ExpandoObject();
            data.mnemonic = mnemonic.ToList();

            return await SendMessage<uint>("add_key", data, "fingerprint", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Deletes a specific key from the wallet
        /// </summary>        
        /// <param name="fingerprint">The key's fingerprint</param>  
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>An awaitable <see cref="Task"/></returns>
        public async Task DeleteKey(uint fingerprint, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.fingerprint = fingerprint;

            await SendMessage("delete_key", data, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Check the key use prior to possible deletion
        /// checks whether key is used for either farm or pool rewards
        /// checks if any wallets have a non-zero balance
        /// </summary>        
        /// <param name="fingerprint">The key's fingerprint</param>  
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>
        /// Indicators of how the wallet is used
        /// </returns>
        public async Task<(uint Fingerprint, bool UsedForFarmerRewards, bool UsedForPoolRewards, bool WalletBalance)> CheckDeleteKey(uint fingerprint, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.fingerprint = fingerprint;

            var response = await SendMessage("check_delete_key", data, cancellationToken).ConfigureAwait(false);

            return (
                (uint)response.fingerprint,
                response.used_for_farmer_rewards,
                response.used_for_pool_rewards,
                response.wallet_balance
                );
        }

        /// <summary>
        /// Deletes all keys from the wallet
        /// </summary>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>An awaitable <see cref="Task"/></returns>
        public async Task DeleteAllKeys(CancellationToken cancellationToken = default)
        {
            await SendMessage("delete_all_keys", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Generates a new mnemonic phrase
        /// </summary>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>The new mnemonic as an <see cref="IEnumerable{T}"/> of 24 words</returns>
        public async Task<IEnumerable<string>> GenerateMnemonic(CancellationToken cancellationToken = default)
        {
            return await SendMessage<IEnumerable<string>>("generate_mnemonic", null, "mnemonic", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Create a new CAT wallet
        /// </summary>
        /// <param name="amount">The amount to put in the wallet (in units of mojos)</param>
        /// <param name="fee">Fee (in units of mojos)</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>Information about the wallet</returns>
        public async Task<(WalletType Type, string AssetId, uint WalletId)> CreateCATWallet(ulong amount, ulong fee = 0, CancellationToken cancellationToken = default)
        {
            return await CreateCATWallet(string.Empty, amount, fee, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Create a new CAT wallet
        /// </summary>
        /// <param name="amount">The amount to put in the wallet (in units of mojos)</param>
        /// <param name="fee">Fee (in units of mojos)</param>
        /// <param name="name">The wallet name</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>Information about the wallet</returns>
        public async Task<(WalletType Type, string AssetId, uint WalletId)> CreateCATWallet(string name, ulong amount, ulong fee = 0, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.wallet_type = "cat_wallet";
            data.mode = "new";
            data.amount = amount;
            data.fee = fee;
            if (!string.IsNullOrEmpty(name))
            {
                data.name = name;
            }

            var response = await SendMessage("create_new_wallet", data, cancellationToken).ConfigureAwait(false);

            return (
                (WalletType)response.type,
                response.asset_id,
                (uint)response.wallet_id
                );
        }

        /// <summary>
        /// Create a wallet for an existing CAT
        /// </summary>
        /// <param name="assetId">The id of the CAT</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>        
        /// <returns>The wallet type</returns>
        public async Task<(WalletType Type, string AssetID, uint WalletId)> CreateWalletForCAT(string assetId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(assetId))
            {
                throw new ArgumentNullException(nameof(assetId));
            }

            dynamic data = new ExpandoObject();
            data.wallet_type = "cat_wallet";
            data.mode = "existing";
            data.asset_id = assetId;

            var response = await SendMessage("create_new_wallet", data, cancellationToken).ConfigureAwait(false);

            return (
                (WalletType)response.type,
                response.asset_id,
                (uint)response.wallet_id
                );
        }

        /// <summary>
        /// Get an NFT wallet by DID ID
        /// </summary>
        /// <param name="didId">The DID ID</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>The wallet id</returns>
        public async Task<uint> GetNFTByDID(string didId, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.did_id = didId;

            return await SendMessage<uint>("nft_get_by_did", data, "wallet_id", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets all the wallets with DIDs
        /// </summary>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>The list of wallets</returns>
        public async Task<IEnumerable<(uint WalletId, string DIDId, uint DIDWalletID)>> GetWalletsWithDIDs(CancellationToken cancellationToken = default)
        {
            var response = await SendMessage("nft_get_wallets_with_dids", cancellationToken).ConfigureAwait(false);

            var list = new List<(uint, string, uint)>();
            foreach (var d in response.nft_wallets)
            {
                list.Add((d.wallet_id, d.did_id, d.did_wallet_id));
            }

            return list;
        }

        /// <summary>
        /// Get info about an NFT
        /// </summary>
        /// <param name="didId">The coin id</param>
        /// <param name="latest">Get latest NFT</param>
        /// <param name="ignoreSizeLimit"></param>
        /// <param name="reusePuzhash"></param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>The wallet id</returns>
        public async Task<NFTInfo> GetNFTInfo(string coinId, bool latest = true, bool ignoreSizeLimit = false, bool? reusePuzhash = null, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.coin_id = coinId;
            data.latest = latest;
            data.ignore_size_limit = ignoreSizeLimit;
            data.reuse_puzhash = reusePuzhash;

            return await SendMessage<NFTInfo>("nft_get_info", data, "nft_info", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Creates a new NFT wallet
        /// </summary>
        /// <param name="didId">An optional DID ID</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>Information about the wallet</returns>
        public async Task<(uint Id, WalletType Type)> CreateNFTWallet(string? didId = null, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.wallet_type = "nft_wallet";
            if (!string.IsNullOrEmpty(didId))
            {
                data.did_id = didId;
            }

            var response = await SendMessage("create_new_wallet", data, cancellationToken).ConfigureAwait(false);

            return (
                (uint)response.id,
                (WalletType)response.type
                );
        }

        /// <summary>
        /// Creates a new DID wallet
        /// </summary>
        /// <param name="backupDIDs">Backup DIDs</param>
        /// <param name="numOfBackupIdsNeeded">The number of back ids needed to create the wallet</param>
        /// <param name="amount">The amount to put in the wallet (in units of mojos)</param>           
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>Information about the wallet</returns>
        public async Task<(WalletType Type, string myDID, uint walletId)> CreateDIDWallet(IEnumerable<string> backupDIDs, ulong numOfBackupIdsNeeded, string name, IDictionary<string, string>? metaData = null, ulong fee = 0, CancellationToken cancellationToken = default)
        {
            if (backupDIDs is null)
            {
                throw new ArgumentNullException(nameof(backupDIDs));
            }

            dynamic data = new ExpandoObject();
            data.wallet_type = "did_wallet";
            data.did_type = "new";
            data.backup_dids = backupDIDs.ToList();
            data.num_of_backup_ids_needed = numOfBackupIdsNeeded;
            data.wallet_name = name;
            data.fee = fee;
            if (metaData is not null)
            {
                data.meta_data = metaData;
            }

            var response = await SendMessage("create_new_wallet", data, cancellationToken).ConfigureAwait(false);

            return (
                (WalletType)response.type,
                response.my_did,
                (uint)response.wallet_id
                );
        }

        /// <summary>
        /// Recover a DID wallet
        /// </summary>
        /// <param name="filename">Filename to recover from</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>Information about the wallet</returns>
        public async Task<(WalletType Type, string myDID, uint walletId, string coinName, Coin coin, string newPuzHash, string pubkey, IEnumerable<byte> backupDIDs, ulong numVerificationsRequired)>
            RecoverDIDWallet(string backupData, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(backupData))
            {
                throw new ArgumentNullException(nameof(backupData));
            }

            dynamic data = new ExpandoObject();
            data.wallet_type = "did_wallet";
            data.did_type = "recovery";
            data.backup_data = backupData;

            var response = await SendMessage("create_new_wallet", data, cancellationToken).ConfigureAwait(false);

            // this gets serialzied back as an unnamed tuple [self.parent_coin_info, self.puzzle_hash, self.amount]
            var coinList = response.coin_list;
            var coin = new Coin()
            {
                ParentCoinInfo = coinList[0],
                PuzzleHash = coinList[1],
                Amount = coinList[2]
            };
            return (
                (WalletType)response.type,
                response.my_did,
                (uint)response.wallet_id,
                response.coin_name,
                coin,
                response.newpuzhash,
                response.pubkey,
                Converters.ToEnumerable<byte>(response.backup_dids),
                response.num_verifications_required
                );
        }

        /// <summary>
        /// Gets basic info about a pool that is used for pool wallet creation
        /// </summary>
        /// <param name="poolUri">The uri of the pool (not including 'pool_info')</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns><see cref="PoolInfo"/> that can be used to create a pool wallet and join this pool</returns>
        public static async Task<PoolInfo> GetPoolInfo(Uri poolUri, CancellationToken cancellationToken = default)
        {
            using var httpClient = new HttpClient(new SocketsHttpHandler(), true);
            using var response = await httpClient
                .GetAsync(new Uri(poolUri, "pool_info"), cancellationToken)
                .ConfigureAwait(false);
            using var responseContent = response
                .EnsureSuccessStatusCode()
                .Content;

            var responseJson = await responseContent
                .ReadAsStringAsync(cancellationToken)
                .ConfigureAwait(false);

            return responseJson.ToObject<PoolInfo>() ?? new PoolInfo();
        }

        /// <summary>
        /// Creates a new pool wallet
        /// </summary>
        /// <param name="initialTargetState">The desired intiial state of the wallet</param>
        /// <param name="p2SingletonDelayedPH">A delayed address (can be null or empty to not use)</param>
        /// <param name="p2SingletonDelayTime">Delay time to create the wallet</param>           
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>Information about the wallet</returns>
        public async Task<(TransactionRecord transaction, string launcherId, string p2SingletonHash)>
            CreatePoolWallet(PoolState initialTargetState, ulong? p2SingletonDelayTime = null, string? p2SingletonDelayedPH = null, CancellationToken cancellationToken = default)
        {
            if (initialTargetState is null)
            {
                throw new ArgumentNullException(nameof(initialTargetState));
            }

            dynamic data = new ExpandoObject();
            data.wallet_type = "pool_wallet";
            data.mode = "new";
            data.initial_target_state = initialTargetState;

            if (p2SingletonDelayTime is not null)
            {
                data.p2_singleton_delay_time = p2SingletonDelayTime;
            }

            if (!string.IsNullOrEmpty(p2SingletonDelayedPH))
            {
                data.p2_singleton_delayed_ph = p2SingletonDelayedPH;
            }

            var response = await SendMessage("create_new_wallet", data, cancellationToken).ConfigureAwait(false);

            return (
                Converters.ToObject<TransactionRecord>(response.transaction),
                response.launcher_id,
                response.p2_singleton_puzzle_hash
                );
        }

        /// <summary>
        /// Get the amount farmed
        /// </summary>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>The amount farmed</returns>
        public async Task<(ulong FarmedAmount, ulong FarmerRewardAmount, ulong FeeAmount, uint LastHeightFarmed, ulong PoolRewardAmount)> GetFarmedAmount(CancellationToken cancellationToken = default)
        {
            var response = await SendMessage("get_farmed_amount", cancellationToken).ConfigureAwait(false);

            return (
                response.farmed_amount,
                response.farmer_reward_amount,
                response.fee_amount,
                response.last_height_farmed,
                response.pool_reward_amount
                );
        }

        /// <summary>
        /// Creates and signs a transaction.
        /// </summary>
        /// <param name="excludeCoinAmounts"></param>
        /// <param name="excludeCoins"></param>
        /// <param name="maxCoinAmount"></param>
        /// <param name="minCoinAmount"></param>
        /// <param name="puzzleAnnouncements"></param>
        /// <param name="coinAnnouncements"></param>
        /// <param name="coins"></param>
        /// <param name="additions"></param>
        /// <param name="fee">Fee (in units of mojos)</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>The signed <see cref="TransactionRecord"/></returns>
        public async Task<(TransactionRecord SignedTx, IEnumerable<TransactionRecord> SignedTxs)> CreateSignedTransaction(
            IEnumerable<AmountWithPuzzlehash> additions,
            IEnumerable<ulong>? excludeCoinAmounts = null,
            IEnumerable<Coin>? excludeCoins = null,
            IEnumerable<PuzzleAnnouncement>? puzzleAnnouncements = null,
            IEnumerable<CoinAnnouncement>? coinAnnouncements = null,
            IEnumerable<Coin>? coins = null,
            ulong minCoinAmount = 0,
            ulong maxCoinAmount = 0,
            ulong fee = 0,
            CancellationToken cancellationToken = default)
        {
            if (additions is null)
            {
                throw new ArgumentNullException(nameof(additions));
            }

            dynamic data = new ExpandoObject();
            data.additions = additions.ToList();
            data.fee = fee;
            data.min_coin_amount = minCoinAmount;
            data.max_coin_amount = maxCoinAmount;
            if (excludeCoins is not null)
            {
                data.exclude_coins = excludeCoins.ToList();
            }
            if (excludeCoinAmounts is not null)
            {
                data.exclude_coin_amounts = excludeCoinAmounts.ToList();
            }
            if (coins is not null)
            {
                data.coins = coins.ToList();
            }
            if (coinAnnouncements is not null)
            {
                data.coin_announcements = coinAnnouncements.ToList();
            }
            if (puzzleAnnouncements is not null)
            {
                data.puzzle_announcements = puzzleAnnouncements.ToList();
            }
            var response = await SendMessage("create_signed_transaction", data, cancellationToken).ConfigureAwait(false);

            return (
                Converters.ToObject<TransactionRecord>(response.signed_tx),
                Converters.ToObject<IEnumerable<TransactionRecord>>(response.signed_txs)
                );
        }

        /// <summary>
        /// Create but do not send a transaction
        /// </summary>
        /// <param name="additions">Additions to the block chain</param>
        /// <param name="fee">Fee amount (in units of mojos)</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>The signed <see cref="TransactionRecord"/></returns>
        public async Task<TransactionRecord> CreateSignedTransaction(IEnumerable<Coin> additions, ulong fee = 0, CancellationToken cancellationToken = default)
        {
            return await CreateSignedTransaction(additions, fee: fee, cancellationToken).ConfigureAwait(false);
        }


        /// <summary>
        /// Retrieves the coins for given coin IDs
        /// </summary>
        /// <param name="names">The coin names</param>
        /// <param name="includeSpentCoins">Flag indicating whether to include spent coins or not</param>
        /// <param name="startHeight">confirmation start height for search</param>
        /// <param name="endHeight">confirmation end height for search</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>A list of <see cref="CoinRecord"/>s</returns>
        public async Task<IEnumerable<CoinRecord>> GetCoinRecordsByNames(IEnumerable<string> names, bool includeSpentCoins, uint? startHeight = null, uint? endHeight = null, CancellationToken cancellationToken = default)
        {
            if (names is null)
            {
                throw new ArgumentNullException(nameof(names));
            }

            dynamic data = new ExpandoObject();
            data.names = names.ToList();
            data.include_spent_coins = includeSpentCoins;
            data.start_height = startHeight;
            data.end_height = endHeight;

            return await SendMessage<IEnumerable<CoinRecord>>("get_coin_records_by_names", data, "coin_records", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Initialize the new data layer wallets.
        /// </summary>
        /// <param name="root"></param>
        /// <param name="fee">Fee (in units of mojos)</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>An awaitable Task</returns>
        public async Task<(IEnumerable<TransactionRecord> Transactions, string LauncherId)> CreateNewDl(string root, ulong fee = 0, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.root = root;
            data.fee = fee;
            var response = await SendMessage("create_new_dl", data, cancellationToken).ConfigureAwait(false);
            return (Converters.ToObject<IEnumerable<TransactionRecord>>(response.transactions), response.launcher_id);
        }

        /// <summary>
        /// Transfers an NFT to another address.
        /// </summary>
        /// <param name="fungibleAssets"></param>
        /// <param name="royaltyAssets"></param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>An awaitable Task</returns>
        public async Task<IDictionary<string, IEnumerable<AssetInfo>>> CalculateRoyalties(IEnumerable<FungibleAsset> fungibleAssets, IEnumerable<RoyaltyAsset> royaltyAssets, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.royalty_assets = royaltyAssets.ToList();
            data.fungible_assets = fungibleAssets.ToList();
            var response = await SendMessage("nft_calculate_royalties", data, cancellationToken).ConfigureAwait(false);
            return Converters.ToObject<IDictionary<string, IEnumerable<AssetInfo>>>(response);
        }

        /// <summary>
        /// Bulk set DID for NFTs across different wallets.
        /// </summary>
        /// <param name="didId"></param>
        /// <param name="nftCoinList"></param>
        /// <param name="reusePuzhash"></param>
        /// <param name="fee">Fee (in units of mojos)</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>Transaction number and <see cref="SpendBundle"/></returns>
        public async Task<(int TxNum, SpendBundle SpendBundle)> NftSetDidBulk(string didId, IEnumerable<NftCoinInfo> nftCoinList, bool? reusePuzhash = null, ulong fee = 0, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.nft_coin_list = nftCoinList.ToList();
            data.did_id = didId;
            data.fee = fee;
            data.reuse_puzhash = reusePuzhash;
            var response = await SendMessage("nft_set_did_bulk", data, cancellationToken).ConfigureAwait(false);
            return (response.tx_num, Converters.ToObject<SpendBundle>(response.spend_bundle));
        }

        /// <summary>
        /// Bulk transfer NFTs to an address.
        /// </summary>
        /// <param name="targetAddress"></param>
        /// <param name="nftCoinList"></param>
        /// <param name="reusePuzhash"></param>
        /// <param name="fee">Fee (in units of mojos)</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>Transaction number and a <see cref="SpendBundle"/></returns>
        public async Task<(int TxNum, SpendBundle SpendBundle)> NftTransferBulk(string targetAddress, IEnumerable<NftCoinInfo> nftCoinList, bool? reusePuzhash = null, ulong fee = 0, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.nft_coin_list = nftCoinList.ToList();
            data.target_address = targetAddress;
            data.fee = fee;
            data.reuse_puzhash = reusePuzhash;
            var response = await SendMessage("nft_transfer_bulk", data, cancellationToken).ConfigureAwait(false);
            return (response.tx_num, Converters.ToObject<SpendBundle>(response.spend_bundle));
        }

        /// <summary>
        /// Retrieves information about a DID.
        /// </summary>
        /// <param name="coinId"></param>
        /// <param name="latest"></param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>An awaitable Task</returns>
        public async Task<DidInfo> DidGetInfo(string coinId, bool latest = true, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.coin_id = coinId;
            data.latest = latest;
            return await SendMessage<DidInfo>("did_get_info", data, null, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Recover a missing or unspendable DID wallet by a coin id of the DID.
        /// </summary>
        /// <param name="coinId"></param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns><see cref="string"/></returns>
        public async Task<string> DidFindLostDid(string coinId, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.coin_id = coinId;
            return await SendMessage<string>("did_find_lost_did", data, "latest_coin_id", cancellationToken).ConfigureAwait(false);
        }
        /// <summary>
        /// Gets the current derivation index.
        /// </summary>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns><see cref="uint"/></returns>
        public async Task<uint> GetCurrentDerivationIndex(CancellationToken cancellationToken = default)
        {
            return await SendMessage<uint>("get_current_derivation_index", null, "index", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Extends the current derivation index.
        /// </summary>
        /// <param name="index"></param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns><see cref="uint"/></returns>
        public async Task<uint> ExtendDerivationIndex(uint index, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.index = index;
            return await SendMessage<uint>("extend_derivation_index", data, "index", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Retrieves the memo from a transaction.
        /// </summary>
        /// <param name="transactionId"></param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>An awaitable Task</returns>
        public async Task<IDictionary<string, IDictionary<string, IEnumerable<string>>>> GetTransactionMemo(string transactionId, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.transaction_id = transactionId;
            return await SendMessage<IDictionary<string, IDictionary<string, IEnumerable<string>>>>("get_transaction_memo", data, "", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Retrieves the balance of a specific list of wallets.
        /// </summary>
        /// <param name="walletIds"></param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>A list of <see cref="WalletBalance"/></returns>
        public async Task<IDictionary<string, WalletBalance>> GetWalletBalances(IEnumerable<uint> walletIds, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.wallet_ids = walletIds.ToList();
            return await SendMessage<IDictionary<string, WalletBalance>>("get_wallet_balances", data, "wallet_balances", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Deletes notifications.
        /// </summary>
        /// <param name="ids"></param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>An awaitable Task</returns>
        public async Task DeleteNotifications(IEnumerable<string> ids, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.ids = ids.ToList();
            await SendMessage("delete_notifications", data, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Sends a notification.
        /// </summary>
        /// <param name="fee">Fee (in units of mojos)</param>
        /// <param name="amount"></param>
        /// <param name="message">In hex</param>
        /// <param name="target"></param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns><see cref="TransactionRecord"/></returns>
        public async Task<TransactionRecord> SendNotification(ulong amount, string message, string target, ulong fee = 0, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.target = target;
            data.message = message;
            data.amount = amount;
            data.fee = fee;
            return await SendMessage<TransactionRecord>("send_notification", data, "tx", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Given a derived P2 address, sign the message by its private key.
        /// </summary>
        /// <param name="isHex"></param>
        /// <param name="message"></param>
        /// <param name="address"></param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>PubKey, Signature, and SigningMode</returns>
        public async Task<(string PubKey, string Signature, string SigningMode)> SignMessageByAddress(string message, string address, bool isHex = false, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.address = address;
            data.message = message;
            data.is_hex = isHex;
            var response = await SendMessage("sign_message_by_address", data, cancellationToken).ConfigureAwait(false);
            return (response.pubkey, response.signature, response.sigining_mode);
        }

        /// <summary>
        /// Given a NFT/DID ID, sign the message by the P2 private key.
        /// </summary>
        /// <param name="isHex"></param>
        /// <param name="message"></param>
        /// <param name="id"></param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>PubKey, Signature, and SigningMode</returns>
        public async Task<(string PubKey, string Signature, string SigningMode)> SignMessageById(string message, string id, bool isHex = false, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.id = id;
            data.message = message;
            data.is_hex = isHex;
            var response = await SendMessage("sign_message_by_id", data, "pubkey", cancellationToken).ConfigureAwait(false);
            return (response.pubkey, response.signature, response.sigining_mode);
        }

        /// <param name="reverse"></param>
        /// <param name="order"></param>
        /// <param name="spentRange"></param>
        /// <param name="confirmedRange"></param>
        /// <param name="amountRange"></param>
        /// <param name="amountFilter"></param>
        /// <param name="parentCoinIdFilter"></param>
        /// <param name="puzzleHashFilter"></param>
        /// <param name="coinIdFilter"></param>
        /// <param name="coinType"></param>
        /// <param name="walletType"></param>
        /// <param name="walletId"></param>
        /// <param name="limit"></param>
        /// <param name="offset"></param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns><see cref="IEnumerable{CoinRecord}"/></returns>
        public async Task<(IEnumerable<CoinRecord> CoinRecords, int? TotalCount)> GetCoinRecords(
            UInt32Range? spentRange = null,
            UInt32Range? confirmedRange = null,
            UInt64Range? amountRange = null,
            AmountFilter? amountFilter = null,
            HashFilter? parentCoinIdFilter = null,
            HashFilter? puzzleHashFilter = null,
            HashFilter? coinIdFilter = null,
            CoinType? coinType = null,
            WalletType? walletType = null,
            uint? walletId = null,
            uint? limit = null,
            CoinRecordOrder order = CoinRecordOrder.ConfirmedHeight,
            uint offset = 0,
            bool includeTotalCount = false,
            bool reverse = false,
            CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.limit = limit;
            data.wallet_id = walletId;
            data.wallet_type = walletType;
            data.coin_type = coinType;
            data.coin_id_filter = coinIdFilter;
            data.puzzle_hash_filter = puzzleHashFilter;
            data.parent_coin_id_filter = parentCoinIdFilter;
            data.amount_filter = amountFilter;
            data.amount_range = amountRange;
            data.confirmed_range = confirmedRange;
            data.spent_range = spentRange;

            data.offset = offset;
            data.order = order;
            data.reverse = reverse;
            data.include_total_count = includeTotalCount;
            var response = await SendMessage("get_coin_records", data, cancellationToken).ConfigureAwait(false);
            return (Converters.ToObject<IEnumerable<CoinRecord>>(response.coin_records), response.total_count);
        }

        /// <summary>
        /// Given a public key, message and signature, verify if it is valid.
        /// </summary>
        /// <param name="signingMode"></param>
        /// <param name="address"></param>
        /// <param name="signature"></param>
        /// <param name="message"></param>
        /// <param name="pubkey"></param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns><see cref="bool"/></returns>
        public async Task<bool> VerifySignature(string signature, string message, string pubkey, string? address = null, string? signingMode = null, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.pubkey = pubkey;
            data.message = message;
            data.signature = signature;
            data.address = address;
            data.signing_mode = signingMode;
            return await SendMessage<bool>("verify_signature", data, "isValid", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Retrieve the timestamp for a given block height.
        /// </summary>
        /// <param name="height"></param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>A timestamp</returns>
        public async Task<(ulong Timestamp, DateTime DateTimestamp)> GetTimestampForHeight(uint height, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.height = height;
            ulong timestamp = await SendMessage<ulong>("get_timestamp_for_height", data, "timestamp", cancellationToken).ConfigureAwait(false);

            return (timestamp, timestamp.ToDateTime());
        }

        /// <summary>
        /// Resync the current logged in wallet. The transaction and offer records will be kept.
        /// </summary>
        /// <param name="enable"></param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>An awaitable Task</returns>
        public async Task SetWalletResyncOnStartup(bool enable = true, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.enable = enable;
            await SendMessage("set_wallet_resync_on_startup", data, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Set auto claim merkle coins config
        /// </summary>
        /// <param name="enable"></param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns><see cref="AutoClaimSettings"/></returns>
        public async Task<AutoClaimSettings> SetAutoClaim(bool enabled, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.enabled = enabled;

            return await SendMessage<AutoClaimSettings>("set_auto_claim", data, "", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Set auto claim merkle coins config
        /// </summary>
        /// <param name="batchSize"></param>
        /// <param name="minAmount"></param>
        /// <param name="txFee"></param>
        /// <param name="enabled"></param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns><see cref="AutoClaimSettings"/></returns>
        public async Task<AutoClaimSettings> SetAutoClaim(bool enabled = true, ushort batchSize = 50, ulong minAmount = 0, ulong txFee = 0, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.enabled = enabled;
            data.tx_fee = txFee;
            data.min_amount = minAmount;
            data.batch_size = batchSize;
            return await SendMessage<AutoClaimSettings>("set_auto_claim", data, null, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Get auto claim merkle coins config
        /// </summary>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns><see cref="AutoClaimSettings"/></returns>
        public async Task<AutoClaimSettings> GetAutoClaim(CancellationToken cancellationToken = default)
        {
            return await SendMessage<AutoClaimSettings>("get_auto_claim", null, cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Spend clawback coins that were sent (to claw them back) or received (to claim them).
        /// </summary>
        /// <param name="batchSize"></param>
        /// <param name="fee">Fee (in units of mojos)</param>
        /// <param name="coinIds"></param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>A list of <see cref="string"/></returns>
        public async Task<IEnumerable<string>> SpendClawbackCoins(IEnumerable<string> coinIds, ushort batchSize = 50, ulong fee = 0, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.coin_ids = coinIds.ToList();
            data.fee = fee;
            data.batch_size = batchSize;
            return await SendMessage<IEnumerable<string>>("spend_clawback_coins", data, "transaction_ids", cancellationToken).ConfigureAwait(false);
        }
    }
}
