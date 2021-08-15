using System;

namespace chia.dotnet.tests
{
    /// <summary>
    /// Use this class to setup the connection to the daemon under test
    /// </summary>
    internal static class Factory
    {
        // this is the ip address of the chia node
        private const string NodeHostAddress = "172.25.181.156";

        public static HttpRpcClient CreateDirectRpcClientFromHardcodedLocation(int port)
        {
            var endpoint = new EndpointInfo()
            {
                Uri = new Uri($"https://{NodeHostAddress}:{port}"),
                CertPath = @"\\wsl$/Ubuntu-20.04/home/don/.chia/mainnet/config/ssl/full_node/private_full_node.crt",
                KeyPath = @"\\wsl$/Ubuntu-20.04/home/don/.chia/mainnet/config/ssl/full_node/private_full_node.key",
            };
        
            return new HttpRpcClient(endpoint);
        }
        /// <summary>
        /// Create a rpc client instance from a hardcoded address
        /// </summary>
        /// <returns><see cref="DaemonProxy"/></returns>
        public static WebSocketRpcClient CreateRpcClientFromHardcodedLocation()
        {
            /*
            # warning YOU MIGHT BE USING A PRODUCTION NODE
            var endpoint = Config.Open().GetEndpoint("ui");
            */

            ///*                  
            var endpoint = new EndpointInfo()
            {
                Uri = new Uri($"wss://{NodeHostAddress}:55400"),
                CertPath = @"\\wsl$/Ubuntu-20.04/home/don/.chia/mainnet/config/ssl/daemon/private_daemon.crt",
                KeyPath = @"\\wsl$/Ubuntu-20.04/home/don/.chia/mainnet/config/ssl/daemon/private_daemon.key",
                //Uri = new Uri("wss://localhost:55400"),
                //CertPath = @"/home/don/.chia/mainnet/config/ssl/daemon/private_daemon.crt",
                //KeyPath = @"/home/don/.chia/mainnet/config/ssl/daemon/private_daemon.key",
            };
            //*/
            return new WebSocketRpcClient(endpoint);
        }

        /// <summary>
        /// Create a daemon instance from the specified config file
        /// </summary>
        /// <param name="filePath">Full path to the chia config file</param>
        /// <returns><see cref="DaemonProxy"/></returns>
        public static WebSocketRpcClient CreateRpcClientFromConfig(string filePath)
        {
            var config = Config.Open(filePath);
            var endpoint = config.GetEndpoint("daemon");
            return new WebSocketRpcClient(endpoint);
        }
    }
}
