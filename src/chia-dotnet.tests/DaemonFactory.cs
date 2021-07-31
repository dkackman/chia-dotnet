namespace chia.dotnet.tests
{
    /// <summary>
    /// Use this class to setup the connection to the daemon under test
    /// </summary>
    internal static class DaemonFactory
    {
        /// <summary>
        /// Create a daemon instance from a hardcoded address
        /// </summary>
        /// <returns><see cref="Daemon"/></returns>
        public static Daemon CreateDaemonFromHardcodedLocation()
        {
            // this is an example using a WSL instance running locally
            var endpoint = new EndpointInfo()
            {
                Uri = new System.Uri("wss://172.26.208.64:55400"),
                CertPath = @"\\wsl$/Ubuntu-20.04/home/don/.chia/mainnet/config/ssl/daemon/private_daemon.crt",
                KeyPath = @"\\wsl$/Ubuntu-20.04/home/don/.chia/mainnet/config/ssl/daemon/private_daemon.key",
            };

            return new Daemon(endpoint, "unit_tests");
        }

        /// <summary>
        /// Create a daemon instance from the specified config file
        /// </summary>
        /// <param name="filePath">Full path to the chia config file</param>
        /// <returns><see cref="Daemon"/></returns>
        public static Daemon CreateDaemonFromConfig(string filePath)
        {
            var config = Config.Open(filePath);

            return new Daemon(config.GetEndpoint("daemon"), "unit_tests");
        }
    }
}
