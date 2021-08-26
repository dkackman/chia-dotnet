using System;
using System.Threading;
using System.Threading.Tasks;

namespace chia.dotnet.console
{
    internal class ClientFactory
    {
        public ClientFactory(string originService)
        {
            OriginService = originService;
        }

        public string OriginService { get; init; }

        public async Task<IRpcClient> CreateRpcClient(SharedOptions options, string serviceName)
        {
            var endpoint = GetEndpointInfo(options, serviceName);

            if (endpoint.Uri.Scheme == "wss")
            {
                using var cts = new CancellationTokenSource(5000);

                var rpcClient = new WebSocketRpcClient(endpoint);
                await rpcClient.Connect(cts.Token);

                var daemon = new DaemonProxy(rpcClient, OriginService);
                await daemon.RegisterService(cts.Token);

                return rpcClient;
            }

            if (endpoint.Uri.Scheme == "https")
            {
                return new HttpRpcClient(endpoint);
            }

            throw new InvalidOperationException($"Unrecognized endpoint Uri scheme {endpoint.Uri.Scheme}");
        }

        private static EndpointInfo GetEndpointInfo(SharedOptions options, string serviceName)
        {
            if (options.UseDefaultConfig)
            {
                return Config.Open().GetEndpoint(serviceName);
            }

            if (!string.IsNullOrEmpty(options.ConfigPath))
            {
                return Config.Open(options.ConfigPath).GetEndpoint(serviceName);
            }

            return new EndpointInfo()
            {
                Uri = new Uri(options.Uri),
                CertPath = options.CertPath,
                KeyPath = options.KeyPath
            };
        }
    }
}
