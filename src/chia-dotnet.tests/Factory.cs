using System;
using System.IO;

namespace chia.dotnet.tests
{
    /// <summary>
    /// Use this class to setup the connection to the daemon under test
    /// </summary>
    internal static class Factory
    {
        // this is the ip address of the chia node
        private const string NodeHostAddress = "chiapas";

        public static HttpRpcClient CreateDirectRpcClientFromHardcodedLocation(int port)
        {
            var endpoint = new EndpointInfo()
            {
                Uri = new Uri($"https://{NodeHostAddress}:{port}"),
                //CertPath = @"\\wsl$/Ubuntu-20.04/home/don/.chia/mainnet/config/ssl/full_node/private_full_node.crt",
                //KeyPath = @"\\wsl$/Ubuntu-20.04/home/don/.chia/mainnet/config/ssl/full_node/private_full_node.key",
                //CertPath = @"C:\Users\dkack\.rchia\certs\chiapas\private_daemon.crt",
                //KeyPath = @"C:\Users\dkack\.rchia\certs\chiapas\private_daemon.key",
                CertPath = @"C:\Users\dkack\.rchia\certs\chiapas\private_crawler.crt",
                KeyPath = @"C:\Users\dkack\.rchia\certs\chiapas\private_crawler.key",
                //CertPath = @"C:\Users\dkack\.rchia\certs\chiapas\private_full_node.crt",
                //KeyPath = @"C:\Users\dkack\.rchia\certs\chiapas\private_full_node.key",
            };

            return new HttpRpcClient(endpoint);
        }
        /// <summary>
        /// Create a rpc client instance from a hardcoded address
        /// </summary>
        /// <returns><see cref="DaemonProxy"/></returns>
        public static WebSocketRpcClient CreateDaemon()
        {
            /*
#warning YOU MIGHT BE USING A PRODUCTION NODE
            var endpoint = Config.Open().GetEndpoint("ui");
            */

            ///*                  
            var endpoint = new EndpointInfo()
            {
                Uri = new Uri($"wss://{NodeHostAddress}:55400"),
                CertPath = @"C:\Users\dkack\.rchia\certs\chiapas\private_daemon.crt",
                KeyPath = @"C:\Users\dkack\.rchia\certs\chiapas\private_daemon.key",
                //CertPath = @"\\wsl$\Ubuntu-20.04\home\don\.chia\mainnet\config\ssl\daemon\private_daemon.crt",
                //KeyPath = @"\\wsl$\Ubuntu-20.04\home\don\.chia\mainnet\config\ssl\daemon\private_daemon.key",
                //CertPath = @"\\wsl.localhost\Ubuntu\home\don\.chia\mainnet\config\ssl\daemon\private_daemon.crt",
                //KeyPath = @"\\wsl.localhost\Ubuntu\home\don\.chia\mainnet\config\ssl\daemon\private_daemon.key",
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
        private static WebSocketRpcClient CreateRpcClientFromConfig(string filePath)
        {
            var config = Config.Open(filePath);
            var endpoint = config.GetEndpoint("daemon");
            return new WebSocketRpcClient(endpoint);
        }

        /// <summary>
        /// Create a daemon instance from default localhost testnet config.yaml
        /// </summary>
        /// <returns><see cref="WebSocketRpcClient"/></returns>
        /// <exception cref="ApplicationException">Thrown if config.yaml not found at the default testnet path</exception>
        public static WebSocketRpcClient CreateDaemonClientFromDefaultTestnetConfig()
        {
            var userPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            var configPath = Path.Combine(userPath, @".chia\testnet\config\config.yaml");

            return !File.Exists(configPath)
                ? throw new ApplicationException("No config file found for localhost testnet")
                : CreateRpcClientFromConfig(configPath);
        }
    }
}
