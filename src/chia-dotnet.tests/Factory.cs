namespace chia.dotnet.tests
{
    internal static class Factory
    {
        public static Daemon CreateDaemon()
        {
            var endpoint = new EndpointInfo()
            {
                Uri = new System.Uri("wss://172.30.175.41:55400"),
                CertPath = @"\\wsl$\Ubuntu-20.04\home\don\.chia\mainnet\config\ssl\daemon\private_daemon.crt",
                KeyPath = @"\\wsl$\Ubuntu-20.04\home\don\.chia\mainnet\config\ssl\daemon\private_daemon.key",
            };
            //var config = Config.Open(@"C:\Users\dkack\.chia\testnet9\config\config.yaml");

            return new Daemon(endpoint, "unit_tests");
        }
    }
}
