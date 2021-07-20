using System;
using System.Dynamic;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Diagnostics;

namespace chia.dotnet
{
    /// <summary>
    /// Base class that uses a <see cref="Daemon"/> to send and receive messages to other services
    /// </summary>
    /// <remarks>The lifetime of the daemon is not controlled by the proxy. It should be disposed outside of this class. <see cref="RpcClient.Connect(CancellationToken)"/>
    /// and <see cref="Daemon.Register(CancellationToken)"/> should be invoked </remarks>
    public abstract class ServiceProxy
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
        public async Task Ping(CancellationToken cancellationToken = default)
        {
            _ = await SendMessage("ping", cancellationToken);
        }

        /// <summary>
        /// Stops the node
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>Awaitable <see cref="Task"/></returns>
        public async Task StopNode(CancellationToken cancellationToken = default)
        {
            _ = await SendMessage("stop_node", cancellationToken);
        }

        /// <summary>
        /// Get connections that the service has
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>A list of connections</returns>
        public async Task<IEnumerable<dynamic>> GetConnections(CancellationToken cancellationToken = default)
        {
            var response = await SendMessage("get_connections", cancellationToken);

            return response.Data.connections;
        }

        /// <summary>
        /// Add a connection
        /// </summary>
        /// <param name="host">The host name of the connection</param>
        /// <param name="port">The port to use</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>An awaitable <see cref="Task"/></returns>
        public async Task OpenConnection(string host, int port, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(host))
            {
                throw new ArgumentNullException(nameof(host));
            }

            dynamic data = new ExpandoObject();
            data.host = host;
            data.port = port;

            _ = await SendMessage("open_connection", data, cancellationToken);
        }

        /// <summary>
        /// Closes a connection
        /// </summary>
        /// <param name="nodeId">The id of the node to close</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>An awaitable <see cref="Task"/></returns>
        public async Task CloseConnection(string nodeId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(nodeId))
            {
                throw new ArgumentNullException(nameof(nodeId));
            }

            dynamic data = new ExpandoObject();
            data.node_id = nodeId;

            _ = await SendMessage("close_connection", data, cancellationToken);
        }

        /// <summary>
        /// Sends a message via the <see cref="Daemon"/> to <see cref="DestinationService"/>
        /// </summary>
        /// <param name="command">The command</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>A response <see cref="Message"/></returns>
        /// <exception cref="ResponseException">Throws when <see cref="Message.IsSuccessfulResponse"/> is False</exception>
        internal async Task<Message> SendMessage(string command, CancellationToken cancellationToken)
        {
            Debug.Assert(!string.IsNullOrEmpty(command));

            var message = Message.Create(command, null, DestinationService, Daemon.OriginService);
            return await Daemon.SendMessage(message, cancellationToken);
        }

        /// <summary>
        /// Sends a message via the <see cref="Daemon"/> to <see cref="DestinationService"/>
        /// </summary>
        /// <param name="command">The command</param>
        /// <param name="data">Data to go along with the command</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>A response <see cref="Message"/></returns>
        /// <exception cref="ResponseException">Throws when <see cref="Message.IsSuccessfulResponse"/> is False</exception>
        internal async Task<Message> SendMessage(string command, dynamic data, CancellationToken cancellationToken)
        {
            Debug.Assert(!string.IsNullOrEmpty(command));

            var message = Message.Create(command, data, DestinationService, Daemon.OriginService);
            return await Daemon.SendMessage(message, cancellationToken);
        }
    }
}
