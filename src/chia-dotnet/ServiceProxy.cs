using System;
using System.Dynamic;
using System.Threading;
using System.Threading.Tasks;

namespace chia.dotnet
{
    /// <summary>
    /// Wrapper class that uses a <see cref="Daemon"/> to send and receive messages to other services
    /// </summary>
    /// <remarks>The lifetime of the daemon is not controlled by the proxy. It should be disposed outside of this class. <see cref="RpcClient.Connect(CancellationToken)"/></remarks>
    /// and <see cref="Daemon.Register(CancellationToken)"/> should be invoked 
    public class ServiceProxy
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="daemon"><see cref="Daemon"/> instance to use for rpc communication</param>
        /// <param name="destinationService"><see cref="Message.Destination"/></param>
        public ServiceProxy(Daemon daemon, string destinationService)
        {
            Daemon = daemon ?? throw new ArgumentNullException(nameof(daemon));

            if (string.IsNullOrEmpty(destinationService))
            {
                throw new ArgumentNullException(nameof(destinationService));
            }

            DestinationService = destinationService;
        }

        /// <summary>
        /// The <see cref="Daemon"/> used for underlying RPC
        /// </summary>
        public Daemon Daemon { get; init; }

        /// <summary>
        /// <see cref="Message.Destination"/>
        /// </summary>
        public string DestinationService { get; init; }

        /// <summary>
        /// Sends ping message to the service
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>Awaitable <see cref="Task"/></returns>
        public async Task Ping(CancellationToken cancellationToken)
        {
            var message = CreateMessage("ping");

            _ = await Daemon.SendMessage(message, cancellationToken);
        }

        /// <summary>
        /// Stops the node
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>Awaitable <see cref="Task"/></returns>
        public async Task StopNode(CancellationToken cancellationToken)
        {
            var message = CreateMessage("stop_node");

            _ = await Daemon.SendMessage(message, cancellationToken);
        }

        /// <summary>
        /// Get connections that the farmer has
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
        /// Add a connection
        /// </summary>
        /// <param name="host">The host name of the connection</param>
        /// <param name="port">The port to use</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>An awaitable <see cref="Task"/></returns>
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
        /// <returns>An awaitable <see cref="Task"/></returns>
        public async Task CloseConnection(string nodeId, CancellationToken cancellationToken)
        {
            dynamic data = new ExpandoObject();
            data.node_id = nodeId;

            var message = CreateMessage("close_connection", data);
            _ = await Daemon.SendMessage(message, cancellationToken);
        }

        /// <summary>
        /// Constructs a <see cref="Message"/> instance with <see cref="Message.Destination"/> and <see cref="Message.Origin"/> set
        /// from <see cref="DestinationService"/> and <see cref="Daemon.OriginService"/>
        /// </summary>
        /// <param name="command">The command to send</param>
        /// <returns><see cref="Message"/></returns>
        protected Message CreateMessage(string command)
        {
            return Message.Create(command, null, DestinationService, Daemon.OriginService);
        }

        /// <summary>
        /// Constructs a <see cref="Message"/> instance with <see cref="Message.Destination"/> and <see cref="Message.Origin"/> set
        /// from <see cref="DestinationService"/> and <see cref="Daemon.OriginService"/>
        /// </summary>
        /// <param name="command">The command to send</param>
        /// <param name="data">Data to send with the command</param>
        /// <returns><see cref="Message"/></returns>
        protected Message CreateMessage(string command, dynamic data)
        {
            return Message.Create(command, data, DestinationService, Daemon.OriginService);
        }
    }
}
