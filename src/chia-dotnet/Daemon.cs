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
    /// Rpc client for the daemon interface. The daemon can be used to proxy messages to and from other chia services.
    /// </summary>
    public class Daemon : RpcClient
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="endpoint">Details of thw websocket endpoint</param>
        /// <param name="serviceName">The name of the service that is running. Will be used as the 'origin' of all messages</param>
        public Daemon(EndpointInfo endpoint, string serviceName)
            : base(endpoint, serviceName)
        {
        }

        public async Task Exit(CancellationToken cancellationToken)
        {
            var response = await SendMessage(Message.Create("exit", new ExpandoObject(), "daemon", ServiceName), cancellationToken);

            if (response.Data.success == false)
            {
                throw new InvalidOperationException();
            }
        }

        public async Task<bool> IsServiceRunning(string service, CancellationToken cancellationToken)
        {
            var response = await SendMessage(CreateServiceMessage("is_running", service), cancellationToken);

            return response.Data.is_running;
        }

        public async Task Register(CancellationToken cancellationToken)
        {
            await RegisterService(ServiceName, cancellationToken);
        }

        public async Task RegisterService(string service, CancellationToken cancellationToken)
        {
            var response = await SendMessage(CreateServiceMessage("register_service", service), cancellationToken);

            if (response.Data.success == false)
            {
                throw new InvalidOperationException();
            }
        }

        public async Task StartService(string service, CancellationToken cancellationToken)
        {
            var response = await SendMessage(CreateServiceMessage("start_service", service), cancellationToken);

            if (response.Data.success == false)
            {
                throw new InvalidOperationException();
            }
        }

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
            return Message.Create(command, data, "daemon", ServiceName);
        }
    }
}
