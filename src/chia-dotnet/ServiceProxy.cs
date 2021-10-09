using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading;
using System.Threading.Tasks;

namespace chia.dotnet
{
    /// <summary>
    /// Base class that uses an <see cref="IRpcClient"/> to send and receive messages to services
    /// </summary>
    /// <remarks>The lifetime of the RpcClient is not controlled by the proxy. It should be disposed outside of this class.</remarks>
    public abstract class ServiceProxy
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="rpcClient"><see cref="IRpcClient"/> instance to use for rpc communication</param>
        /// <param name="destinationService"><see cref="Message.Destination"/></param>
        /// <param name="originService"><see cref="Message.Origin"/></param>        
        public ServiceProxy(IRpcClient rpcClient, string destinationService, string originService)
        {
            RpcClient = rpcClient ?? throw new ArgumentNullException(nameof(rpcClient));

            if (string.IsNullOrEmpty(destinationService))
            {
                throw new ArgumentNullException(nameof(destinationService));
            }

            if (string.IsNullOrEmpty(originService))
            {
                throw new ArgumentNullException(nameof(originService));
            }

            DestinationService = destinationService;
            OriginService = originService;
        }

        /// <summary>
        /// The name of the service that is running. Will be used as the <see cref="Message.Origin"/> of all messages
        /// as well as the identifier used for <see cref="DaemonProxy.RegisterService(string, CancellationToken)"/>
        /// </summary>
        public string OriginService { get; init; }

        /// <summary>
        /// <see cref="Message.Destination"/>
        /// </summary>
        public string DestinationService { get; init; }

        /// <summary>
        /// The <see cref="IRpcClient"/> used for underlying RPC
        /// </summary>
        public IRpcClient RpcClient { get; init; }

        /// <summary>
        /// Sends ping message to the service
        /// </summary>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>Awaitable <see cref="Task"/></returns>
        public async Task Ping(CancellationToken cancellationToken = default)
        {
            _ = await SendMessage("ping", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Stops the service
        /// </summary>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>Awaitable <see cref="Task"/></returns>
        public async Task StopNode(CancellationToken cancellationToken = default)
        {
            _ = await SendMessage("stop_node", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Get the service's connections
        /// </summary>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>A list of <see cref="ConnectionInfo"/>s</returns>
        public async Task<IEnumerable<ConnectionInfo>> GetConnections(CancellationToken cancellationToken = default)
        {
            return await SendMessage<IEnumerable<ConnectionInfo>>("get_connections", "connections", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Add a connection
        /// </summary>
        /// <param name="host">The host name of the connection</param>
        /// <param name="port">The port to use</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
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

            _ = await SendMessage("open_connection", data, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Closes a connection
        /// </summary>
        /// <param name="nodeId">The id of the connection to close</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>An awaitable <see cref="Task"/></returns>
        public async Task CloseConnection(string nodeId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(nodeId))
            {
                throw new ArgumentNullException(nameof(nodeId));
            }

            dynamic data = new ExpandoObject();
            data.node_id = nodeId;

            _ = await SendMessage("close_connection", data, cancellationToken).ConfigureAwait(false);
        }

        //
        // These methods are the important ones that package up the request for the rpc client and then
        // parse and convert the response for the requester
        //
        internal async Task<dynamic> SendMessage(string command, dynamic? data, CancellationToken cancellationToken = default)
        {
            var message = Message.Create(command, data, DestinationService, OriginService);

            try
            {
                return await RpcClient.SendMessage(message, cancellationToken).ConfigureAwait(false);
            }
            catch (ResponseException)
            {
                throw;
            }
            catch (TaskCanceledException)
            {
                throw;
            }
            catch (Exception e) // wrap eveything else in a resposne exception - this will include websocket or http specific failures
            {
                throw new ResponseException(message, "Something went wrong sending the rpc message. Inspect the InnerException for details.", e);
            }
        }

        internal async Task<dynamic> SendMessage(string command, CancellationToken cancellationToken = default)
        {
            return await SendMessage(command, null, cancellationToken).ConfigureAwait(false);
        }

        //
        // If the return is a collection, specify the collection type in the caller e.g. ICollection<SomeConcreteType>, IList, IEnumerable
        //
        internal async Task<T> SendMessage<T>(string command, string? childItem = null, CancellationToken cancellationToken = default)
        {
            return await SendMessage<T>(command, null, childItem, cancellationToken).ConfigureAwait(false);
        }

        //
        // If the return is a collection, specify the collection type in the caller e.g. ICollection<SomeConcreteType>, IList, IEnumerable
        //
        internal async Task<T> SendMessage<T>(string command, dynamic? data, string? childItem = null, CancellationToken cancellationToken = default)
        {
            var d = await SendMessage(command, data, cancellationToken).ConfigureAwait(false);

            return Converters.ToObject<T>(d, childItem);
        }
    }
}
