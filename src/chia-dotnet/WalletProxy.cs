using System.Dynamic;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace chia.dotnet
{
    /// <summary>
    /// Proxy that communicates with the wallet via the daemon
    /// </summary>
    public sealed class WalletProxy : ServiceProxy
    {
        /// <summary>
        /// Default location for backups
        /// </summary>
        public const string DefaultBackupHost = "https://backup.chia.net";

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
        /// Sets a key to active.
        /// </summary>
        /// <param name="fingerprint">The fingerprint</param>          
        /// <param name="skipImport">Indicator whether to skip the import at login</param>          
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>The key fingerprint</returns>
        public async Task<uint> LogIn(uint fingerprint, bool skipImport, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.fingerprint = fingerprint;
            data.type = skipImport ? "skip" : "normal";
            data.host = DefaultBackupHost;

            var response = await SendMessage("log_in", data, cancellationToken);

            return (uint)response.fingerprint;
        }

        /// <summary>
        /// Sets a key to active.
        /// </summary>
        /// <param name="fingerprint">The fingerprint</param>
        /// <param name="filePath">The path to the backup file</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>The key fingerprint</returns>
        public async Task<uint> LogInAndRestoreBackup(uint fingerprint, string filePath, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.fingerprint = fingerprint;
            data.type = "restore_backup";
            data.file_path = filePath;
            data.host = DefaultBackupHost;

            var response = await SendMessage("log_in", data, cancellationToken);

            return (uint)response.fingerprint;
        }

        /// <summary>
        /// Get the list of wallets
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>The list of wallets</returns>
        public async Task<IEnumerable<WalletInfo>> GetWallets(CancellationToken cancellationToken = default)
        {
            return await SendMessage<IEnumerable<WalletInfo>>("get_wallets", "wallets", cancellationToken);
        }

        /// <summary>
        /// Get all root public keys accessible by the wallet
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>All root public keys accessible by the wallet</returns>
        public async Task<IEnumerable<uint>> GetPublicKeys(CancellationToken cancellationToken = default)
        {
            var response = await SendMessage("get_public_keys", cancellationToken);

            return Converters.ConvertList<uint>(response.public_key_fingerprints);
        }

        /// <summary>
        /// Get the private key accessible by the wallet
        /// </summary>
        /// <param name="fingerprint">The fingerprint</param>          
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>The private key for the fingerprint</returns>
        public async Task<(uint Fingerprint, string Sk, string Pk, string FarmerPk, string PoolPk, string Seed)> GetPrivateKey(uint fingerprint, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.fingerprint = fingerprint;

            var response = await SendMessage("get_private_key", data, cancellationToken);

            return (response.private_key.fingerprint, response.private_key.sk, response.private_key.pk, response.private_key.farmer_pk, response.private_key.pool_pk, response.private_key.seed);
        }

        /// <summary>
        /// Get the wallet's sync status
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>The current sync status</returns>
        public async Task<(bool GenesisInitialized, bool Synced, bool Syncing)> GetSyncStatus(CancellationToken cancellationToken = default)
        {
            var response = await SendMessage("get_sync_status", cancellationToken);

            return (response.genesis_initialized, response.synced, response.syncing);
        }

        /// <summary>
        /// Retrieves information about the current network
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>The current network name and prefix</returns>
        public async Task<(string NetworkName, string NetworkPrefix)> GetNetworkInfo(CancellationToken cancellationToken = default)
        {
            var response = await SendMessage("get_network_info", cancellationToken);

            return (response.network_name, response.network_prefix);
        }

        /// <summary>
        /// Get blockchain height info
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>Current block height</returns>
        public async Task<uint> GetHeightInfo(CancellationToken cancellationToken = default)
        {
            var response = await SendMessage("get_height_info", cancellationToken);

            return response.height;
        }

        /// <summary>
        /// Get a specific transaction
        /// </summary>
        /// <param name="transactionId">The id of the transaction to find</param> 
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>The <see cref="TransactionRecord"/></returns>
        public async Task<TransactionRecord> GetTransaction(string transactionId, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.transaction_id = transactionId;

            return await SendMessage<TransactionRecord>("get_transaction", data, "transaction", cancellationToken);
        }

        /// <summary>
        /// Backup the wallet
        /// </summary>
        /// <param name="filePath">Path to the backup file to create</param> 
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>An awaitable <see cref="Task"/></returns>
        public async Task CreateBackup(string filePath, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.file_path = filePath;

            _ = await SendMessage("create_backup", data, cancellationToken);
        }

        /// <summary>
        /// Deletes a specific key from the wallet
        /// </summary>        
        /// <param name="mnemonic">The key mnemonic</param>
        /// <param name="skipImport">Indicator whether to skip the import at login</param>                
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>The new key's fingerprint</returns>
        public async Task<uint> AddKey(IEnumerable<string> mnemonic, bool skipImport, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.mnemonic = mnemonic.ToList();
            data.type = skipImport ? "skip" : "new_wallet";

            var response = await SendMessage("add_key", data, cancellationToken);

            return (uint)response.fingerprint;
        }

        /// <summary>
        /// Add a new key and restores from backup
        /// </summary>
        /// <param name="fingerprint">The fingerprint</param>
        /// <param name="filePath">The path to the backup file</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>The key's fingerprint</returns>
        public async Task<uint> AddKeyAndRestoreBackup(uint fingerprint, string filePath, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.fingerprint = fingerprint;
            data.type = "restore_backup";
            data.file_path = filePath;

            var response = await SendMessage("log_in", data, cancellationToken);

            return (uint)response.fingerprint;
        }

        /// <summary>
        /// Deletes a specific key from the wallet
        /// </summary>        
        /// <param name="fingerprint">The key's fingerprint</param>  
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>An awaitable <see cref="Task"/></returns>
        public async Task DeleteKey(uint fingerprint, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.fingerprint = fingerprint;

            _ = await SendMessage("delete_key", data, cancellationToken);
        }

        /// <summary>
        /// Check the key use prior to possible deletion
        /// checks whether key is used for either farm or pool rewards
        /// checks if any wallets have a non-zero balance
        /// </summary>        
        /// <param name="fingerprint">The key's fingerprint</param>  
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>
        /// Indicators of how wallet is used
        /// </returns>
        public async Task<(bool UsedForFarmerRewards, bool UsedForPoolRewards, bool WalletBalance)> CheckDeleteKey(uint fingerprint, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.fingerprint = fingerprint;

            var response = await SendMessage("check_delete_key", data, cancellationToken);

            return (response.used_for_farmer_rewards, response.used_for_pool_rewards, response.wallet_balance);
        }

        /// <summary>
        /// Deletes all keys from the wallet
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>An awaitable <see cref="Task"/></returns>
        public async Task DeleteAllKeys(CancellationToken cancellationToken = default)
        {
            _ = await SendMessage("delete_all_keys", cancellationToken);
        }

        /// <summary>
        /// Generates a new mnemonic phrase
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>The new mnemonic as an <see cref="IEnumerable{T}"/> of 24 words</returns>
        public async Task<IEnumerable<string>> GenerateMnemonic(CancellationToken cancellationToken = default)
        {
            var response = await SendMessage("generate_mnemonic", cancellationToken);

            return Converters.ToStrings(response.mnemonic);
        }

        /// <summary>
        /// Create a new colour coin wallet
        /// </summary>
        /// <param name="amount">the amount to put in the wallet (in units of mojos)</param>
        /// <param name="fee">fee to create the wallet (in units of mojos)</param>
        /// <param name="colour">The coin Colour</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>Information about the wallet</returns>
        public async Task<(byte Type, string Colour, uint WalletId)> CreateColourCoinWallet(ulong amount, ulong fee, string colour, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.wallet_type = "cc_wallet";
            data.host = DefaultBackupHost;
            data.amount = amount;
            data.fee = fee;
            data.mode = "new";
            data.colour = colour;

            var response = await SendMessage("create_new_wallet", data, cancellationToken);

            return (response.type, response.colour, response.wallet_id);
        }

        /// <summary>
        /// Create a coloured coin wallet for an existing colour
        /// </summary>
        /// <param name="colour">The coin Colour</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>        
        /// <returns>The wallet type</returns>
        public async Task<byte> CreateColouredCoinForColour(string colour, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.wallet_type = "cc_wallet";
            data.host = DefaultBackupHost;
            data.mode = "existing";
            data.colour = colour;

            var response = await SendMessage("create_new_wallet", data, cancellationToken);

            return response.type;
        }

        /// <summary>
        /// Creates a new Admin Rate Limited wallet
        /// </summary>
        /// <param name="pubkey">admin pubkey</param>
        /// <param name="interval">The limit interval</param>
        /// <param name="limit">The limit amount</param>
        /// <param name="amount">the amount to put in the wallet (in units of mojos)</param>     
        /// <param name="fee">fee to create the wallet (in units of mojos)</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>Information about the wallet</returns>
        public async Task<(uint Id, byte Type, Coin origin, string pubkey)> CreateRateLimitedAdminWallet(string pubkey, ulong interval, ulong limit, ulong amount, ulong fee, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.wallet_type = "rl_wallet";
            data.rl_type = "admin";
            data.host = DefaultBackupHost;
            data.pubkey = pubkey;
            data.amount = amount;
            data.fee = fee;
            data.interval = interval;
            data.limit = limit;

            var response = await SendMessage("create_new_wallet", data, cancellationToken);

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
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>Information about the wallet</returns>
        public async Task<(uint Id, byte Type, string pubkey)> CreateRateLimitedUserWallet(CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.wallet_type = "rl_wallet";
            data.rl_type = "user";
            data.host = DefaultBackupHost;

            var response = await SendMessage("create_new_wallet", data, cancellationToken);

            return (response.id, response.type, response.pubkey);
        }

        /// <summary>
        /// Creates a new DID wallet
        /// </summary>
        /// <param name="backupDIDs">Backup DIDs</param>
        /// <param name="numOfBackupIdsNeeded">The number of back ids needed to create the wallet</param>
        /// <param name="amount">the amount to put in the wallet (in units of mojos)</param>           
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>Information about the wallet</returns>
        public async Task<(uint Type, string myDID, uint walletId)> CreateDIDWallet(IEnumerable<string> backupDIDs, ulong numOfBackupIdsNeeded, ulong amount, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.wallet_type = "did_wallet";
            data.did_type = "new";
            data.backup_dids = backupDIDs.ToList();
            data.num_of_backup_ids_needed = numOfBackupIdsNeeded;
            data.amount = amount;
            data.host = DefaultBackupHost;

            var response = await SendMessage("create_new_wallet", data, cancellationToken);

            return (response.type, response.my_did, response.wallet_id);
        }

        /// <summary>
        /// Recover a DID wallet
        /// </summary>
        /// <param name="filename">Filename to recover from</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>Information about the wallet</returns>
        public async Task<(uint Type, string myDID, uint walletId, string coinName, IEnumerable<dynamic> coinList, string newPuzHash, string pubkey, IEnumerable<dynamic> backupDIDs, ulong numVerificationsRequired)>
            RecoverDIDWallet(string filename, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.wallet_type = "did_wallet";
            data.did_type = "recovery";
            data.filename = filename;

            var response = await SendMessage("create_new_wallet", data, cancellationToken);

            return (
                    response.type,
                    response.my_did,
                    response.wallet_id,
                    response.coin_name,
                    response.coin_list,
                    response.newpuzhash,
                    response.pubkey,
                    response.backup_dids,
                    response.num_verifications_required
                    );
        }

        /// <summary>
        /// Creates a new pool wallet
        /// </summary>
        /// <param name="initialTargetState">The desired intiial state of the wallet</param>
        /// <param name="p2SingletonDelayedPH">A delayed address (can be null or empty to not use)</param>
        /// <param name="p2SingletonDelayTime">Delay time to create the wallet</param>           
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>Information about the wallet</returns>
        public async Task<(TransactionRecord transaction, string launcherId, string p2SingletonHash)>
            CreatePoolWallet(PoolState initialTargetState, ulong p2SingletonDelayTime, string p2SingletonDelayedPH, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.wallet_type = "pool_wallet";
            data.mode = "new";
            data.initial_target_state = initialTargetState;
            data.p2_singleton_delay_time = p2SingletonDelayTime;
            if (!string.IsNullOrEmpty(p2SingletonDelayedPH))
            {
                data.p2_singleton_delayed_ph = p2SingletonDelayedPH;
            }

            var response = await SendMessage("create_new_wallet", data, cancellationToken);

            return (
                    Converters.ToObject<TransactionRecord>(response.transaction),
                    response.launcher_id,
                    response.p2_singleton_puzzle_hash
                    );
        }

        /// <summary>
        /// Create an offer file from a set of id's
        /// </summary>
        /// <param name="ids">The set of ids</param>
        /// <param name="filename">path to the offer file to create</param>   
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>An awaitable <see cref="Task"/></returns>
        public async Task CreateOfferForIds(IDictionary<int, int> ids, string filename, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.ids = ids;
            data.filename = filename;

            _ = await SendMessage("create_offer_for_ids", data, cancellationToken);
        }

        /// <summary>
        /// Get offer discrepencies
        /// </summary>
        /// <param name="filename">path to the offer file</param>         
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>The discrepencies</returns>
        public async Task<dynamic> GetDiscrepenciesForOffer(string filename, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.filename = filename;

            var response = await SendMessage("get_discrepancies_for_offer", data, cancellationToken);

            return response.discrepancies;
        }

        /// <summary>
        /// Respond to an offer
        /// </summary>
        /// <param name="filename">path to the offer file</param>        
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>An awaitable <see cref="Task"/></returns>
        public async Task RespondToOffer(string filename, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.filename = filename;

            _ = await SendMessage("respond_to_offer", data, cancellationToken);
        }

        /// <summary>
        /// Get a trade
        /// </summary>
        /// <param name="tradeId">The id of the trade to find</param>         
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>The <see cref="TradeRecord"/></returns>
        public async Task<TradeRecord> GetTrade(string tradeId, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.trade_id = tradeId;

            return await SendMessage<TradeRecord>("get_trade", data, "trade", cancellationToken);
        }

        /// <summary>
        /// Get all trades
        /// </summary>        
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>The <see cref="TradeRecord"/>s</returns>
        public async Task<IEnumerable<TradeRecord>> GetAllTrades(CancellationToken cancellationToken = default)
        {
            return await SendMessage<IEnumerable<TradeRecord>>("get_all_trades", "trades", cancellationToken);
        }

        /// <summary>
        /// Cancel a trade
        /// </summary>
        /// <param name="tradeId">The id of the trade to find</param>         
        /// <param name="secure">Flag indicating whether to cancel pedning offer securely or not</param>         
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>An awaitable <see cref="Task"/></returns>
        public async Task CancelTrade(string tradeId, bool secure, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.trade_id = tradeId;
            data.secure = secure;

            _ = await SendMessage("cancel_trade", data, cancellationToken);
        }

        /// <summary>
        /// Get the amount farmed
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>The amount farmed</returns>
        public async Task<(uint FarmedAmount, uint FarmerRewardAmount, uint FeeAmount, uint LastHeightFarmed, uint PoolRewardAmount)> GetFarmedAmount(CancellationToken cancellationToken = default)
        {
            var response = await SendMessage("get_farmed_amount", cancellationToken);

            return (response.farmed_amount, response.farmer_reward_amount, response.fee_amount, response.last_height_farmed, response.pool_reward_amount);
        }

        /// <summary>
        /// Create but do not send a transaction
        /// </summary>
        /// <param name="additions">Additions to the block chain</param>
        /// <param name="coins">Coins to include</param>
        /// <param name="fee">Fee amount (in units of mojos)</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>The signed <see cref="TransactionRecord"/></returns>
        public async Task<TransactionRecord> CreateSignedTransaction(IEnumerable<Coin> additions, IEnumerable<Coin> coins, ulong fee, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.additions = additions.ToList();
            data.fee = fee;
            if (coins != null) // coins are optional
            {
                data.coins = coins.ToList();
            }

            return await SendMessage<TransactionRecord>("create_signed_transaction", data, "signed_tx", cancellationToken);
        }

        /// <summary>
        /// Create but do not send a transaction
        /// </summary>
        /// <param name="additions">Additions to the block chain</param>
        /// <param name="fee">Fee amount (in units of mojos)</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>The signed <see cref="TransactionRecord"/></returns>
        public async Task<TransactionRecord> CreateSignedTransaction(IEnumerable<Coin> additions, ulong fee, CancellationToken cancellationToken = default)
        {
            return await CreateSignedTransaction(additions, null, fee, cancellationToken);
        }
    }
}
