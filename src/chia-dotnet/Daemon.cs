using System;
using System.Dynamic;
using System.Threading;
using System.Threading.Tasks;

namespace chia.dotnet
{
    /// <summary>
    /// The names of chia services. These are used as <see cref="Message.Destination"/> values
    /// </summary>
    public struct ServiceNames
    {
        public const string FullNode = "chia_full_node";
        public const string Wallet = "chia_wallet";
        public const string Farmer = "chia_farmer";
        public const string Harvester = "chia_harvester";
        public const string Simulator = "chia_full_node_simulator";
        public const string Plotter = "chia plots create";
    }

    /// <summary>
    /// <see cref="RpcClient"/> for the daemon interface. The daemon can be used to proxy messages to and from other chia services.
    /// </summary>
    public class Daemon : RpcClient
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="endpoint">Details of the websocket endpoint</param>
        /// <param name="originServiceName">The name of the service that is running. Will be used as the 'origin' of all messages</param>
        public Daemon(EndpointInfo endpoint, string originServiceName)
            : base(endpoint)
        {
            if (string.IsNullOrEmpty(value: originServiceName))
            {
                throw new ArgumentNullException(nameof(originServiceName));
            }

            OriginService = originServiceName;
        }

        /// <summary>
        /// The name of the service that is running. Will be used as the <see cref="Message.Origin"/> of all messages
        /// </summary>
        public string OriginService { get; private set; }

        /// <summary>
        /// Tells the daemon at the RPC endpoint to exit.
        /// </summary>
        /// <remarks>There isn't a way to start the daemon remotely via RPC so take care that you have access to the RPC host if needed</remarks>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>Awaitable <see cref="Task"/></returns>
        public async Task Exit(CancellationToken cancellationToken)
        {
            var response = await SendMessage(Message.Create("exit", new ExpandoObject(), "daemon", OriginService), cancellationToken);

            if (response.Data.success == false)
            {
                throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Determines if the named service is running
        /// </summary>
        /// <param name="service">The <see cref="ServiceNames"/> of the service</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>Awaitable <see cref="Task"/> with the boolean value indicating whether the service is running</returns>
        public async Task<bool> IsServiceRunning(string service, CancellationToken cancellationToken)
        {
            var response = await SendMessage(CreateServiceMessage("is_running", service), cancellationToken);

            return response.Data.is_running;
        }

        /// <summary>
        /// Registers this websocket to receive messages using <see cref="OriginService"/> This is needed to receive responses from services other than the daemon.
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>Awaitable <see cref="Task"/></returns>
        public async Task Register(CancellationToken cancellationToken)
        {
            await RegisterService(OriginService, cancellationToken);
        }

        /// <summary>
        /// Registers this websocket to receive messages. This is needed to receive responses from services other than the daemon. 
        /// This is not a <see cref="ServiceNames"/> but usually the name of the consumer application such as 'wallet_ui'
        /// </summary>
        /// <param name="service">The name to register</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>Awaitable <see cref="Task"/></returns>
        public async Task RegisterService(string service, CancellationToken cancellationToken)
        {
            var response = await SendMessage(CreateServiceMessage("register_service", service), cancellationToken);

            if (response.Data.success == false)
            {
                throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Starts the named service
        /// </summary>
        /// <param name="service">The <see cref="ServiceNames"/> of the service</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>Awaitable <see cref="Task"/></returns>
        public async Task StartService(string service, CancellationToken cancellationToken)
        {
            var response = await SendMessage(CreateServiceMessage("start_service", service), cancellationToken);

            if (response.Data.success == false)
            {
                throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Stops the named service
        /// </summary>
        /// <param name="service">The <see cref="ServiceNames"/> of the service</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>Awaitable <see cref="Task"/></returns>
        public async Task StopService(string service, CancellationToken cancellationToken)
        {
            var response = await SendMessage(CreateServiceMessage("stop_service", service), cancellationToken);

            if (response.Data.success == false)
            {
                throw new InvalidOperationException();
            }
        }

        private Message CreateServiceMessage(string command, string service)
        {
            dynamic data = new ExpandoObject();
            data.service = service;
            return Message.Create(command, data, "daemon", OriginService);
        }
    }
}
