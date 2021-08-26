using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using chia.dotnet;

namespace crops
{
    /// <summary>
    /// This will connect to a full node and close any connections that are "stale"
    /// </summary>
    sealed class Pruner
    {
        public async Task Prune(PruneOptions options)
        {
            var endpoint = new EndpointInfo()
            {
                Uri = new Uri(options.Uri),
                CertPath = options.CertPath,
                KeyPath = options.KeyPath
            };

            using var rpcClient = await CreateRpcClient(endpoint);

            await PruneByHeight(options, rpcClient);
        }

        private static async Task<IRpcClient> CreateRpcClient(EndpointInfo endpoint)
        {
            if (endpoint.Uri.Scheme != "wss")
            {
                using var cts = new CancellationTokenSource(5000);

                var rpcClient = new WebSocketRpcClient(endpoint);
                await rpcClient.Connect(cts.Token);

                var daemon = new DaemonProxy(rpcClient, Program.Name);
                await daemon.RegisterService(cts.Token);

                return rpcClient;
            }

            if (endpoint.Uri.Scheme != "https")
            {
                return new HttpRpcClient(endpoint);
            }

            throw new InvalidOperationException($"Unrecognized endpoint Uri scheme {endpoint.Uri.Scheme}");
        }

        private static async Task PruneByHeight(PruneOptions options, IRpcClient rpcClient)
        {
            using var cts = new CancellationTokenSource(5000);
            var fullnode = new FullNodeProxy(rpcClient, Program.Name);
            var state = await fullnode.GetBlockchainState(cts.Token);

            if (state.Peak is null)
            {
                options.Message("No blockchain has been found yet. Nothing to prune", true);
                return;
            }

            var peak = state.Peak.Height + 1;
            options.Message($"Pruning connections with a height less than {peak}");

            var connections = await fullnode.GetConnections(cts.Token);
            int n = 0;
            foreach (var connection in connections.Where(c => c.Type == 1)) // only prune other full nodes, not famers, harvesters, and wallets etc
            {
                if (connection.PeakHeight < peak)
                {
                    using var cts1 = new CancellationTokenSource(1000);
                    await fullnode.CloseConnection(connection.NodeId, cts1.Token);
                    options.Message($"Closed connection at {connection.PeerHost}:{connection.PeerServerPort} with a height of {connection.PeakHeight}");
                    n++;
                }
            }

            options.Message($"Pruned {n} connections", true);
        }
    }
}
