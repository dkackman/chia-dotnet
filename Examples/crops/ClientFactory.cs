using System;
using System.Threading;
using System.Threading.Tasks;

using chia.dotnet;

namespace crops
{
    internal static class ClientFactory
    {
        public static async Task<IRpcClient> CreateRpcClient(SharedOptions options)
        {
            var endpoint = new EndpointInfo()
            {
                Uri = new Uri(options.Uri),
                CertPath = options.CertPath,
                KeyPath = options.KeyPath
            };

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
    }
}