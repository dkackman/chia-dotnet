using System.Dynamic;
using System.Threading;
using System.Threading.Tasks;

namespace chia.dotnet
{
    /// <summary>
    /// Proxy that communicates with the full node via the daemon
    /// </summary>
    public class FarmerProxy : ServiceProxy
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="daemon">The <see cref="Daemon"/> to handle RPC</param>
        public FarmerProxy(Daemon daemon)
            : base(daemon, ServiceNames.Farmer)
        {
        }

        /// <summary>
        /// Get the farmer reqard targets 
        /// </summary>
        /// <param name="searchForPrivateKey">Include private key in search</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>Colleciton of blocks</returns>
        public async Task<dynamic> GetRewardTargets(bool searchForPrivateKey, CancellationToken cancellationToken)
        {
            dynamic data = new ExpandoObject();
            data.search_for_private_key = searchForPrivateKey;

            var message = CreateMessage("get_reward_targets", data);
            var response = await Daemon.SendMessage(message, cancellationToken);

            return response.Data;
        }

        /// <summary>
        /// Sets the farm and pool targets for the farmer
        /// </summary>
        /// <param name="farmerTarget">Farmer target</param>
        /// <param name="poolTarget">Pool target</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>list of headers</returns>
        public async Task SetRewardTargets(string farmerTarget, string poolTarget, CancellationToken cancellationToken)
        {
            dynamic data = new ExpandoObject();
            data.farmer_target = farmerTarget;
            data.pool_target = poolTarget;

            var message = CreateMessage("set_reward_targets", data);
            _ = await Daemon.SendMessage(message, cancellationToken);
        }

        /// <summary>
        /// Get signage points
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>List of signage points</returns>
        public async Task<dynamic> GetSignagePoints(CancellationToken cancellationToken)
        {
            var message = CreateMessage("get_signage_points");
            var response = await Daemon.SendMessage(message, cancellationToken);

            return response.Data.signage_points;
        }

        /// <summary>
        /// Get connections that the farmer has
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>Collection of connections</returns>
        public async Task<dynamic> GetConnections(CancellationToken cancellationToken)
        {
            var message = CreateMessage("get_connections");
            var response = await Daemon.SendMessage(message, cancellationToken);

            return response.Data.connections;
        }

        /// <summary>
        /// Add a connection
        /// </summary>
        /// <param name="host">The host name of the connection</param>
        /// <param name="port">The port to use</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>Collection of connections</returns>
        public async Task OpenConnection(string host, int port, CancellationToken cancellationToken)
        {
            dynamic data = new ExpandoObject();
            data.host = host;
            data.port = port;

            var message = CreateMessage("open_connection", data);
            _ = await Daemon.SendMessage(message, cancellationToken);
        }

        /// <summary>
        /// Closes a connection
        /// </summary>
        /// <param name="nodeId">The id of the node to close</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>Collection of connections</returns>
        public async Task CloseConnection(string nodeId, CancellationToken cancellationToken)
        {
            dynamic data = new ExpandoObject();
            data.node_id = nodeId;

            var message = CreateMessage("close_connection", data);
            _ = await Daemon.SendMessage(message, cancellationToken);
        }
    }
}
