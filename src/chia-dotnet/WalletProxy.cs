using System.Dynamic;
using System.Threading;
using System.Threading.Tasks;

namespace chia.dotnet
{
    /// <summary>
    /// Proxy that communicates with the wallet via the daemon
    /// </summary>
    public class WalletProxy : ServiceProxy
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="daemon">The <see cref="Daemon"/> to handle RPC</param>
        public WalletProxy(Daemon daemon)
            : base(daemon, ServiceNames.Wallet)
        {
        }

        /// <summary>
        /// Get the list of wallets
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>A list of wallets</returns>
        public async Task<dynamic> GetWallets(CancellationToken cancellationToken)
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
        public async Task<dynamic> GetWalletBalance(int walletId, CancellationToken cancellationToken)
        {
            dynamic data = new ExpandoObject();
            data.wallet_id = walletId;

            var message = CreateMessage("get_wallet_balance", data);
            var response = await Daemon.SendMessage(message, cancellationToken);

            return response.Data.wallet_balance;
        }

        /// <summary>
        /// Get connections that the wallet has
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>A list of connections</returns>
        public async Task<dynamic> GetConnections(CancellationToken cancellationToken)
        {
            var message = CreateMessage("get_connections");
            var response = await Daemon.SendMessage(message, cancellationToken);

            return response.Data.connections;
        }

        /// <summary>
        /// Get the wallet's sync status
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>sync status</returns>
        public async Task<dynamic> GetSyncStatus(CancellationToken cancellationToken)
        {
            var message = CreateMessage("get_sync_status");
            var response = await Daemon.SendMessage(message, cancellationToken);

            return response.Data;
        }

        /// <summary>
        /// Get network info
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>network name and prefix</returns>
        public async Task<dynamic> GetNetworkInfo(CancellationToken cancellationToken)
        {
            var message = CreateMessage("get_network_info");
            var response = await Daemon.SendMessage(message, cancellationToken);

            return response.Data;
        }

        /// <summary>
        /// Get blockchain height info
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>Current block height</returns>
        public async Task<int> GetHeightInfo(CancellationToken cancellationToken)
        {
            var message = CreateMessage("get_height_info");
            var response = await Daemon.SendMessage(message, cancellationToken);

            return response.Data.height;
        }
    }
}
