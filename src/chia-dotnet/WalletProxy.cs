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
    public class WalletProxy : ServiceProxy
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

            var message = CreateMessage("log_in", data);
            var response = await Daemon.SendMessage(message, cancellationToken);

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

            var message = CreateMessage("log_in", data);
            var response = await Daemon.SendMessage(message, cancellationToken);

            return (uint)response.Data.fingerprint;
        }

        /// <summary>
        /// Get the list of wallets
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>A list of wallets</returns>
        public async Task<IEnumerable<dynamic>> GetWallets(CancellationToken cancellationToken)
        {
            var message = CreateMessage("get_wallets");
            var response = await Daemon.SendMessage(message, cancellationToken);

            return response.Data.wallets;
        }

        /// <summary>
        /// Get the balance of a specific wallet
        /// </summary>
        /// <param name="walletId">The numeric id of the wallet to query</param>        
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>The wallet balance</returns>
        public async Task<dynamic> GetWalletBalance(uint walletId, CancellationToken cancellationToken)
        {
            dynamic data = new ExpandoObject();
            data.wallet_id = walletId;

            var message = CreateMessage("get_wallet_balance", data);
            var response = await Daemon.SendMessage(message, cancellationToken);

            return response.Data.wallet_balance;
        }

        /// <summary>
        /// Get all root public keys accessible by the wallet
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>all root public keys accessible by the walle</returns>
        public async Task<IEnumerable<uint>> GetPublicKeys(CancellationToken cancellationToken)
        {
            var message = CreateMessage("get_public_keys");
            var response = await Daemon.SendMessage(message, cancellationToken);

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

            var message = CreateMessage("get_private_key", data);
            var response = await Daemon.SendMessage(message, cancellationToken);

            return response.Data.private_key;
        }

        /// <summary>
        /// Get the wallet's sync status
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>sync status</returns>
        public async Task<(bool GenesisInitialized, bool Synced, bool Syncing)> GetSyncStatus(CancellationToken cancellationToken)
        {
            var message = CreateMessage("get_sync_status");
            var response = await Daemon.SendMessage(message, cancellationToken);

            return (response.Data.genesis_initialized, response.Data.synced, response.Data.syncing);
        }

        /// <summary>
        /// Retrieves some information about the current network
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>network name and prefix</returns>
        public async Task<(string NetworkName, string NetworkPrefix)> GetNetworkInfo(CancellationToken cancellationToken)
        {
            var message = CreateMessage("get_network_info");
            var response = await Daemon.SendMessage(message, cancellationToken);

            return (response.Data.network_name, response.Data.network_prefix);
        }

        /// <summary>
        /// Get blockchain height info
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>Current block height</returns>
        public async Task<uint> GetHeightInfo(CancellationToken cancellationToken)
        {
            var message = CreateMessage("get_height_info");
            var response = await Daemon.SendMessage(message, cancellationToken);

            return response.Data.height;
        }

        /// <summary>
        /// Get the list of transactions
        /// </summary>
        /// <param name="walletId">The numeric id of the wallet to query</param> 
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>A list of transactions</returns>
        public async Task<IEnumerable<dynamic>> GetTransactions(uint walletId, CancellationToken cancellationToken)
        {
            dynamic data = new ExpandoObject();
            data.wallet_id = walletId;

            var message = CreateMessage("get_transactions", data);
            var response = await Daemon.SendMessage(message, cancellationToken);

            return response.Data.transactions;
        }

        /// <summary>
        /// Get the last address or create a new one
        /// </summary>
        /// <param name="walletId">The numeric id of the wallet to query</param> 
        /// <param name="newAddress">Whether to generate a new address</param> 
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>An address</returns>
        public async Task<string> GetNextAddress(uint walletId, bool newAddress, CancellationToken cancellationToken)
        {
            dynamic data = new ExpandoObject();
            data.wallet_id = walletId;
            data.new_address = newAddress;

            var message = CreateMessage("get_next_address", data);
            var response = await Daemon.SendMessage(message, cancellationToken);

            return response.Data.address;
        }

        /// <summary>
        /// Get the amount farmed
        /// </summary>
        /// <param name="walletId">The numeric id of the wallet to query</param> 
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>The amount farmed</returns>
        public async Task<(uint FarmedAmount, uint FarmerRewardAmount, uint FeeAmount, uint LastHieghtFarmed, uint PoolReqardAmount)> GetFarmedAmount(uint walletId, CancellationToken cancellationToken)
        {
            dynamic data = new ExpandoObject();
            data.wallet_id = walletId;

            var message = CreateMessage("get_farmed_amount", data);
            var response = await Daemon.SendMessage(message, cancellationToken);

            return (response.Data.farmed_amount, response.Data.farmer_reward_amount, response.Data.fee_amount, response.Data.last_height_farmed, response.Data.pool_reward_amount);
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

            var message = CreateMessage("create_backup", data);
            _ = await Daemon.SendMessage(message, cancellationToken);
        }

        /// <summary>
        /// Get the number of transactions
        /// </summary>
        /// <param name="walletId">The numeric id of the wallet to query</param> 
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>The number of transactions</returns>
        public async Task<uint> GetTransactionCount(uint walletId, CancellationToken cancellationToken)
        {
            dynamic data = new ExpandoObject();
            data.wallet_id = walletId;

            var message = CreateMessage("get_transaction_count", data);
            var response = await Daemon.SendMessage(message, cancellationToken);

            return response.Data.count;
        }
    }
}
