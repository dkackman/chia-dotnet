namespace chia.dotnet.tests
{
    internal static class Factory
    {
        internal static Daemon CreateDaemon()
        {
            var config = Config.Open(@"C:\Users\dkack\.chia\testnet9\config\config.yaml");

            return new Daemon(config.GetEndpoint("daemon"), "unit_tests");
        }
    }
}
