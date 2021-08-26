using System;
using System.Dynamic;
using System.Threading;
using System.Threading.Tasks;

namespace chia.dotnet
{
    /// <summary>
    /// <see cref="WebSocketRpcClient"/> for the daemon interface. 
    /// The daemon can be used to proxy messages to and from other chia services as well
    /// as controlling the <see cref="PlotterProxy"/> and having it's own procedures
    /// </summary>
    public sealed class DaemonProxy : ServiceProxy
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="rpcClient"><see cref="IRpcClient"/> instance to use for rpc communication</param>
        /// <param name="originService"><see cref="Message.Origin"/></param>
        public DaemonProxy(WebSocketRpcClient rpcClient, string originService)
            : base(rpcClient, ServiceNames.Daemon, originService)
        {
        }

        /// <summary>
        /// Tells the daemon at the RPC endpoint to exit.
        /// </summary>
        /// <remarks>There isn't a way to start the daemon remotely via RPC, so take care that you have access to the RPC host if needed</remarks>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>Awaitable <see cref="Task"/></returns>
        public async Task Exit(CancellationToken cancellationToken = default)
        {
            _ = await SendMessage("exit", cancellationToken);
        }

        /// <summary>
        /// Determines if the named service is running
        /// </summary>
        /// <param name="service">The <see cref="ServiceNames"/> of the service</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>A boolean value indicating whether the service is running</returns>
        public async Task<bool> IsServiceRunning(string service, CancellationToken cancellationToken = default)
        {
            var response = await SendMessage("is_running", CreateDataObject(service), cancellationToken);

            return response.is_running;
        }

        /// <summary>
        /// Registers this websocket to receive messages using <see cref="ServiceProxy.OriginService"/> 
        /// This is needed to receive responses from services other than the daemon.
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>Awaitable <see cref="Task"/></returns>
        public async Task RegisterService(CancellationToken cancellationToken = default)
        {
            await RegisterService(OriginService, cancellationToken);
        }

        /// <summary>
        /// Registers this daemon to receive messages. This is needed to receive responses from services other than the daemon. 
        /// This is not a <see cref="ServiceNames"/> but usually the name of the consumer application such as 'wallet_ui'
        /// </summary>
        /// <param name="service">The name to register</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>Awaitable <see cref="Task"/></returns>
        public async Task RegisterService(string service, CancellationToken cancellationToken = default)
        {
            _ = await SendMessage("register_service", CreateDataObject(service), cancellationToken);
        }

        /// <summary>
        /// Starts the named service.
        /// </summary>
        /// <param name="service">The <see cref="ServiceNames"/> of the service</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>Awaitable <see cref="Task"/></returns>
        public async Task StartService(string service, CancellationToken cancellationToken = default)
        {
            _ = await SendMessage("start_service", CreateDataObject(service), cancellationToken);
        }

        /// <summary>
        /// Stops the named service
        /// </summary>
        /// <param name="service">The <see cref="ServiceNames"/> of the service</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>Awaitable <see cref="Task"/></returns>
        public async Task StopService(string service, CancellationToken cancellationToken = default)
        {
            _ = await SendMessage("stop_service", CreateDataObject(service), cancellationToken);
        }

        private static object CreateDataObject(string service)
        {
            if (string.IsNullOrEmpty(service))
            {
                throw new ArgumentNullException(nameof(service));
            }

            dynamic data = new ExpandoObject();
            data.service = service;
            return data;
        }
    }
}
