namespace chia.dotnet.tests
{
    internal static class Factory
    {

        public const double OneMojo = 0.000000000001;

        public static Daemon CreateDaemon()
        {
            var config = Config.Open(@"C:\Users\dkack\.chia\testnet9\config\config.yaml");

            return new Daemon(config.GetEndpoint("daemon"), "unit_tests");
        }
    }
}
