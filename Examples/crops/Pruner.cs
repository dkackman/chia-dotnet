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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>")]
        public async Task Prune(PruneOptions options)
        {
            using var rpcClient = await Program.Factory.CreateRpcClient(options, ServiceNames.FullNode);

            if (options.ProneOld)
            {
                await PruneOldConnections(options, rpcClient);
            }
            else
            {
                await PruneByHeight(options, rpcClient);
            }
        }

        private static async Task PruneOldConnections(PruneOptions options, IRpcClient rpcClient)
        {
            if (options.Age < 1)
            {
                throw new InvalidOperationException("Age must be 1 or more");
            }

            using var cts = new CancellationTokenSource(2000);
            var fullnode = new FullNodeProxy(rpcClient, Program.Factory.OriginService);

            var cutoff = DateTime.UtcNow - new TimeSpan(options.Age, 0, 0);
            options.Message($"Pruning connections that haven't sent a message since {cutoff}");

            var connections = await fullnode.GetConnections(cts.Token);
            int n = 0;
            foreach (var connection in connections.Where(c => c.Type == NodeType.FULL_NODE)) // only prune other full nodes, not famers, harvesters, and wallets etc
            {
                if (connection.LastMessageDateTime < cutoff)
                {
                    using var cts1 = new CancellationTokenSource(1000);
                    await fullnode.CloseConnection(connection.NodeId, cts1.Token);
                    options.Message($"Closed connection at {connection.PeerHost}:{connection.PeerServerPort} that last updated {connection.LastMessageDateTime}");
                    n++;
                }
            }

            options.Message($"Pruned {n} connections", true);
        }

        private static async Task PruneByHeight(PruneOptions options, IRpcClient rpcClient)
        {
            using var cts = new CancellationTokenSource(4000);
            var fullnode = new FullNodeProxy(rpcClient, Program.Factory.OriginService);
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
            foreach (var connection in connections.Where(c => c.Type == NodeType.FULL_NODE)) // only prune other full nodes, not famers, harvesters, and wallets etc
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
