using System;
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
        public async Task<IEnumerable<WalletInfo>> GetWallets(bool includeData = true, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.include_data = includeData;

            return await SendMessage<IEnumerable<WalletInfo>>("get_wallets", data, "wallets", cancellationToken).ConfigureAwait(false);
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
        /// Get a list of all unacknowledged CATs.
        /// </summary>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>The list of <see cref="Token"/>s</returns>
        public async Task<IEnumerable<Token>> GetStrayCats(CancellationToken cancellationToken = default)
        {
            return await SendMessage<IEnumerable<Token>>("get_stray_cats", "stray_cats", cancellationToken).ConfigureAwait(false);
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
        public async Task<uint> GetLoggedInFingerprint(CancellationToken cancellationToken = default)
        {
            return await SendMessage<uint>("get_logged_in_fingerprint", null, "fingerprint", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Get the private key accessible by the wallet
        /// </summary>
        /// <param name="fingerprint">The fingerprint</param>          
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>The private key for the fingerprint</returns>
        public async Task<PrivateKeyDetail> GetPrivateKey(uint fingerprint, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.fingerprint = fingerprint;

            return await SendMessage<PrivateKeyDetail>("get_private_key", data, "private_key", cancellationToken).ConfigureAwait(false);
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
            var response = await SendMessage("get_height_info", cancellationToken).ConfigureAwait(false);

            return response.height;
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

            var response = await SendMessage("add_key", data, cancellationToken).ConfigureAwait(false);

            return (uint)response.fingerprint;
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

            _ = await SendMessage("delete_key", data, cancellationToken).ConfigureAwait(false);
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
        public async Task<(uint FingerPrint, bool UsedForFarmerRewards, bool UsedForPoolRewards, bool WalletBalance)> CheckDeleteKey(uint fingerprint, CancellationToken cancellationToken = default)
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
            _ = await SendMessage("delete_all_keys", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Generates a new mnemonic phrase
        /// </summary>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>The new mnemonic as an <see cref="IEnumerable{T}"/> of 24 words</returns>
        public async Task<IEnumerable<string>> GenerateMnemonic(CancellationToken cancellationToken = default)
        {
            var response = await SendMessage("generate_mnemonic", cancellationToken).ConfigureAwait(false);

            return Converters.ToEnumerable<string>(response.mnemonic);
        }

        /// <summary>
        /// Create a new CAT wallet
        /// </summary>
        /// <param name="amount">The amount to put in the wallet (in units of mojos)</param>
        /// <param name="fee">Fee to create the wallet (in units of mojos)</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>Information about the wallet</returns>
        public async Task<(WalletType Type, string AssetId, uint WalletId)> CreateCATWallet(ulong amount, ulong fee, CancellationToken cancellationToken = default)
        {
            return await CreateCATWallet(string.Empty, amount, fee, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Create a new CAT wallet
        /// </summary>
        /// <param name="amount">The amount to put in the wallet (in units of mojos)</param>
        /// <param name="fee">Fee to create the wallet (in units of mojos)</param>
        /// <param name="name">The wallet name</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>Information about the wallet</returns>
        public async Task<(WalletType Type, string AssetId, uint WalletId)> CreateCATWallet(string name, ulong amount, ulong fee, CancellationToken cancellationToken = default)
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
        /// Creates a new Admin Rate Limited wallet
        /// </summary>
        /// <param name="pubkey">admin pubkey</param>
        /// <param name="interval">The limit interval</param>
        /// <param name="limit">The limit amount</param>
        /// <param name="amount">The amount to put in the wallet (in units of mojos)</param>     
        /// <param name="fee">Fee to create the wallet (in units of mojos)</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>Information about the wallet</returns>
        public async Task<(uint Id, WalletType Type, Coin origin, string pubkey)> CreateRateLimitedAdminWallet(string pubkey, ulong interval, ulong limit, ulong amount, ulong fee, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(pubkey))
            {
                throw new ArgumentNullException(nameof(pubkey));
            }

            dynamic data = new ExpandoObject();
            data.wallet_type = "rl_wallet";
            data.rl_type = "admin";
            data.pubkey = pubkey;
            data.amount = amount;
            data.fee = fee;
            data.interval = interval;
            data.limit = limit;

            var response = await SendMessage("create_new_wallet", data, cancellationToken).ConfigureAwait(false);

            return (
                response.id,
                response.type,
                Converters.ToObject<Coin>(response.origin),
                response.pubkey
                );
        }

        /// <summary>
        /// Creates a new User Rate Limited wallet
        /// </summary>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>Information about the wallet</returns>
        public async Task<(uint Id, WalletType Type, string pubkey)> CreateRateLimitedUserWallet(CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.wallet_type = "rl_wallet";
            data.rl_type = "user";

            var response = await SendMessage("create_new_wallet", data, cancellationToken).ConfigureAwait(false);

            return (
                (uint)response.id,
                (WalletType)response.type,
                response.pubkey
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

            var response = await SendMessage("nft_get_by_did", data, cancellationToken).ConfigureAwait(false);

            return (uint)response.wallet_id;
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
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>The wallet id</returns>
        public async Task<NFTInfo> GetNFTInfo(string coinId, bool latest = true, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.coin_id = coinId;
            data.latest = latest;

            return await SendMessage("nft_get_info", "nft_info", data, cancellationToken).ConfigureAwait(false);
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
        /// Create but do not send a transaction
        /// </summary>
        /// <param name="additions">Additions to the block chain</param>
        /// <param name="fee">Fee amount (in units of mojos)</param>
        /// <param name="coins">Coins to include</param>
        /// <param name="coinAnnouncements">Coins to announce</param>
        /// <param name="puzzleAnnouncements">Puzzles to announce</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>The signed <see cref="TransactionRecord"/></returns>
        public async Task<TransactionRecord> CreateSignedTransaction(IEnumerable<Coin> additions, ulong fee, IEnumerable<Coin>? coins = null, IEnumerable<CoinAnnouncement>? coinAnnouncements = null, IEnumerable<PuzzleAnnouncement>? puzzleAnnouncements = null, CancellationToken cancellationToken = default)
        {
            if (additions is null)
            {
                throw new ArgumentNullException(nameof(additions));
            }

            dynamic data = new ExpandoObject();
            data.additions = additions.ToList();
            data.fee = fee;
            if (coins is not null) // coins are optional
            {
                data.coins = coins.ToList();
            }
            if (coinAnnouncements is not null) // coins are optional
            {
                data.coin_announcements = coinAnnouncements.ToList();
            }
            if (puzzleAnnouncements is not null) // coins are optional
            {
                data.puzzle_announcements = puzzleAnnouncements.ToList();
            }
            return await SendMessage<TransactionRecord>("create_signed_transaction", data, "signed_tx", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Create but do not send a transaction
        /// </summary>
        /// <param name="additions">Additions to the block chain</param>
        /// <param name="fee">Fee amount (in units of mojos)</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>The signed <see cref="TransactionRecord"/></returns>
        public async Task<TransactionRecord> CreateSignedTransaction(IEnumerable<Coin> additions, ulong fee, CancellationToken cancellationToken = default)
        {
            return await CreateSignedTransaction(additions, fee: fee, cancellationToken).ConfigureAwait(false);
        }
    }
}
