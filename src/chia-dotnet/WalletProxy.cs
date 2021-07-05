using System.Dynamic;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

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
        /// <param name="daemon">The <see cref="Daemon"/> to handle RPC</param>
        public WalletProxy(Daemon daemon)
            : base(daemon, ServiceNames.Wallet)
        {
        }

        /// <summary>
        /// Sets a key to active.
        /// </summary>
        /// <param name="fingerprint">The fingerprint</param>          
        /// <param name="skipImport">Indicator whether to skip the import at login</param>          
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>a key fingerprint</returns>
        public async Task<uint> LogIn(uint fingerprint, bool skipImport, CancellationToken cancellationToken)
        {
            dynamic data = new ExpandoObject();
            data.fingerprint = fingerprint;
            data.type = skipImport ? "skip" : "normal";
            data.host = DefaultBackupHost;

            var response = await SendMessage("log_in", data, cancellationToken);

            return (uint)response.Data.fingerprint;
        }

        /// <summary>
        /// Sets a key to active.
        /// </summary>
        /// <param name="fingerprint">The fingerprint</param>
        /// <param name="filePath">The path to the backup file</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>a key fingerprint</returns>
        public async Task<uint> LogInAndRestoreBackup(uint fingerprint, string filePath, CancellationToken cancellationToken)
        {
            dynamic data = new ExpandoObject();
            data.fingerprint = fingerprint;
            data.type = "restore_backup";
            data.file_path = filePath;
            data.host = DefaultBackupHost;

            var response = await SendMessage("log_in", data, cancellationToken);

            return (uint)response.Data.fingerprint;
        }

        /// <summary>
        /// Get the list of wallets
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>A list of wallets</returns>
        public async Task<IEnumerable<dynamic>> GetWallets(CancellationToken cancellationToken)
        {
            var response = await SendMessage("get_wallets", cancellationToken);

            return response.Data.wallets;
        }

        /// <summary>
        /// Get all root public keys accessible by the wallet
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>all root public keys accessible by the walle</returns>
        public async Task<IEnumerable<uint>> GetPublicKeys(CancellationToken cancellationToken)
        {
            var response = await SendMessage("get_public_keys", cancellationToken);

            return ((IEnumerable<dynamic>)response.Data.public_key_fingerprints).Select(item => (uint)item);
        }

        /// <summary>
        /// Get the private key accessible by the wallet
        /// </summary>
        /// <param name="fingerprint">The fingerprint</param>          
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>a private key</returns>
        public async Task<dynamic> GetPrivateKey(uint fingerprint, CancellationToken cancellationToken)
        {
            dynamic data = new ExpandoObject();
            data.fingerprint = fingerprint;

            var response = await SendMessage("get_private_key", data, cancellationToken);

            return response.Data.private_key;
        }

        /// <summary>
        /// Get the wallet's sync status
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>sync status</returns>
        public async Task<(bool GenesisInitialized, bool Synced, bool Syncing)> GetSyncStatus(CancellationToken cancellationToken)
        {
            var response = await SendMessage("get_sync_status", cancellationToken);

            return (response.Data.genesis_initialized, response.Data.synced, response.Data.syncing);
        }

        /// <summary>
        /// Retrieves some information about the current network
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>network name and prefix</returns>
        public async Task<(string NetworkName, string NetworkPrefix)> GetNetworkInfo(CancellationToken cancellationToken)
        {
            var response = await SendMessage("get_network_info", cancellationToken);

            return (response.Data.network_name, response.Data.network_prefix);
        }

        /// <summary>
        /// Get blockchain height info
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>Current block height</returns>
        public async Task<uint> GetHeightInfo(CancellationToken cancellationToken)
        {
            var response = await SendMessage("get_height_info", cancellationToken);

            return response.Data.height;
        }

        /// <summary>
        /// Get a specific transaction
        /// </summary>
        /// <param name="transactionId">The id of the transaction to find</param> 
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>A transaction</returns>
        public async Task<dynamic> GetTransaction(string transactionId, CancellationToken cancellationToken)
        {
            dynamic data = new ExpandoObject();
            data.transaction_id = transactionId;

            var response = await SendMessage("get_transaction", data, cancellationToken);

            return response.Data.transaction;
        }

        /// <summary>
        /// Backup the wallet
        /// </summary>
        /// <param name="filePath">Path to the backup file to create</param> 
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>An awaitable <see cref="Task"/></returns>
        public async Task CreateBackup(string filePath, CancellationToken cancellationToken)
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
        public async Task<uint> AddKey(IEnumerable<string> mnemonic, bool skipImport, CancellationToken cancellationToken)
        {
            dynamic data = new ExpandoObject();
            data.mnemonic = mnemonic.ToList();
            data.type = skipImport ? "skip" : "new_wallet";

            var response = await SendMessage("add_key", data, cancellationToken);

            return (uint)response.Data.fingerprint;
        }

        /// <summary>
        /// Add a new key and restores from backup
        /// </summary>
        /// <param name="fingerprint">The fingerprint</param>
        /// <param name="filePath">The path to the backup file</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>a key fingerprint</returns>
        public async Task<uint> AddKeyAndRestoreBackup(uint fingerprint, string filePath, CancellationToken cancellationToken)
        {
            dynamic data = new ExpandoObject();
            data.fingerprint = fingerprint;
            data.type = "restore_backup";
            data.file_path = filePath;

            var response = await SendMessage("log_in", data, cancellationToken);

            return (uint)response.Data.fingerprint;
        }

        /// <summary>
        /// Deletes a specific key from the wallet
        /// </summary>        
        /// <param name="fingerprint">The key's fingerprint</param>  
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>An awaitable <see cref="Task"/></returns>
        public async Task DeleteKey(uint fingerprint, CancellationToken cancellationToken)
        {
            dynamic data = new ExpandoObject();
            data.fingerprint = fingerprint;

            _ = await SendMessage("delete_all_keys", data, cancellationToken);
        }

        /// <summary>
        /// Deletes all keys from the wallet
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>An awaitable <see cref="Task"/></returns>
        public async Task DeleteAllKeys(CancellationToken cancellationToken)
        {
            _ = await SendMessage("delete_all_keys", cancellationToken);
        }

        /// <summary>
        /// Generates a new mnemonic phrase
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>The new mnemonic as an <see cref="IEnumerable{T}"/> of 24 words</returns>
        public async Task<IEnumerable<string>> GenerateMnemonic(CancellationToken cancellationToken)
        {
            var response = await SendMessage("generate_mnemonic", cancellationToken);

            return ((IEnumerable<dynamic>)response.Data.mnemonic).Select<dynamic, string>(item => item.ToString());
        }

        /// <summary>
        /// Create a new colour coin wallet
        /// </summary>
        /// <param name="amount">the amount to put in the wallet</param>
        /// <param name="fee">fee to create the wallet</param>
        /// <param name="colour">The coin Colour</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>Information about the wallet</returns>
        public async Task<(byte Type, string Colour, uint WalletId)> CreateColourCoinWallet(BigInteger amount, BigInteger fee, string colour, CancellationToken cancellationToken)
        {
            dynamic data = new ExpandoObject();
            data.wallet_type = "cc_wallet";
            data.host = DefaultBackupHost;
            data.amount = amount;
            data.fee = fee;
            data.mode = "new";
            data.colour = colour;

            var response = await SendMessage("create_new_wallet", data, cancellationToken);

            return (response.Data.type, response.Data.colour, response.Data.wallet_id);
        }

        /// <summary>
        /// Create a coloured coin wallet for an existing colour
        /// </summary>
        /// <param name="colour">The coin Colour</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>        
        /// <returns>Information about the wallet</returns>
        public async Task<byte> CreateColouredCoinForColour(string colour, CancellationToken cancellationToken)
        {
            dynamic data = new ExpandoObject();
            data.wallet_type = "cc_wallet";
            data.host = DefaultBackupHost;
            data.mode = "existing";
            data.colour = colour;

            var response = await SendMessage("create_new_wallet", data, cancellationToken);

            return response.Data.type;
        }

        /// <summary>
        /// Creates a new Admin Rate Limited wallet
        /// </summary>
        /// <param name="pubkey">fee to create the wallet</param>
        /// <param name="interval">The limit interval</param>
        /// <param name="limit">The limit amount</param>
        /// <param name="amount">the amount to put in the wallet</param>     
        /// <param name="fee">fee to create the wallet</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>Information about the wallet</returns>
        public async Task<(uint Id, byte Type, dynamic origin, string pubkey)> CreateRateLimitedAdminWallet(string pubkey, BigInteger interval, BigInteger limit, BigInteger amount, BigInteger fee, CancellationToken cancellationToken)
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

            return (response.Data.id, response.Data.type, response.Data.origin, response.Data.pubkey);
        }

        /// <summary>
        /// Creates a new User Rate Limited wallet
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>Information about the wallet</returns>
        public async Task<(uint Id, byte Type, string pubkey)> CreateRateLimitedUserWallet(CancellationToken cancellationToken)
        {
            dynamic data = new ExpandoObject();
            data.wallet_type = "rl_wallet";
            data.rl_type = "user";
            data.host = DefaultBackupHost;

            var response = await SendMessage("create_new_wallet", data, cancellationToken);

            return (response.Data.id, response.Data.type, response.Data.pubkey);
        }

        /// <summary>
        /// Creates a new DID wallet
        /// </summary>
        /// <param name="backupDIDs">Backup DIDs</param>
        /// <param name="numOfBackupIdsNeeded">The number of back ids needed to create the wallet</param>
        /// <param name="amount">the amount to put in the wallet</param>           
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>Information about the wallet</returns>
        public async Task<(uint Type, string myDID, uint walletId)> CreateDIDWallet(IEnumerable<string> backupDIDs, BigInteger numOfBackupIdsNeeded, int amount, CancellationToken cancellationToken)
        {
            dynamic data = new ExpandoObject();
            data.wallet_type = "did_wallet";
            data.did_type = "new";
            data.backup_dids = backupDIDs.ToList();
            data.num_of_backup_ids_needed = numOfBackupIdsNeeded;
            data.amount = amount;
            data.host = DefaultBackupHost;

            var response = await SendMessage("create_new_wallet", data, cancellationToken);

            return (response.Data.type, response.Data.my_did, response.Data.wallet_id);
        }

        /// <summary>
        /// Recover a DID wallet
        /// </summary>
        /// <param name="filename">Filename to recover from</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>Information about the wallet</returns>
        public async Task<(uint Type, string myDID, uint walletId, string coinName, IEnumerable<dynamic> coinList, string newPuzHash, string pubkey, IEnumerable<dynamic> backupDIDs, BigInteger numVerificationsRequired)>
            RecoverDIDWallet(string filename, CancellationToken cancellationToken)
        {
            dynamic data = new ExpandoObject();
            data.wallet_type = "did_wallet";
            data.did_type = "recovery";
            data.filename = filename;

            var response = await SendMessage("create_new_wallet", data, cancellationToken);

            return (response.Data.type, response.Data.my_did, response.Data.wallet_id, response.Data.coin_name, response.Data.coin_list, response.Data.newpuzhash, response.Data.pubkey, response.Data.backup_dids, response.Data.num_verifications_required);
        }

        /// <summary>
        /// Creates a new pool wallet
        /// </summary>
        /// <param name="initialTargetState">The desired intiial state of the wallet</param>
        /// <param name="p2SingletonDelayedPH">A delayed address (can be null or empty to not use)</param>
        /// <param name="p2SingletonDelayTime">Delay time to create the wallet</param>           
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>Information about the wallet</returns>
        public async Task<(dynamic transaction, string launcherId, string p2SingletonHash)> CreatePoolWallet(dynamic initialTargetState, BigInteger p2SingletonDelayTime, string p2SingletonDelayedPH, CancellationToken cancellationToken)
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

            return (response.Data.transaction, response.Data.launcher_id, response.Data.p2_singleton_puzzle_hash);
        }
    }
}
