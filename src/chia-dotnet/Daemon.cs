using System;
using System.Dynamic;
using System.Threading;
using System.Threading.Tasks;

namespace chia.dotnet
{
    public struct ServiceNames
    {
        public const string FullNode = "chia_full_node";
        public const string Wallet = "chia_wallet";
        public const string Farmer = "chia_farmer";
        public const string Harvester = "chia_harvester";
        public const string Simulator = "chia_full_node_simulator";
        public const string Plotter = "chia plots create";
    }

    public class Daemon : RpcClient
    {
        private readonly string _origin;

        public Daemon(EndpointInfo endpoint, string origin)
            : base(endpoint)
        {
            if (string.IsNullOrEmpty(origin))
            {
                throw new ArgumentNullException("origin");
            }

            _origin = origin;
        }

        public async Task<bool> IsServiceRunning(string service, CancellationToken cancellationToken)
        {
            dynamic data = new ExpandoObject();
            data.service = service;
            var message = Message.Create("is_running", data, "daemon", _origin);

            var response = await SendMessage(message, cancellationToken);

            return response.Data.is_running;
        }
    }
}
