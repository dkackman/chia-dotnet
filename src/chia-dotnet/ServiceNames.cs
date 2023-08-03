namespace chia.dotnet
{
    /// <summary>
    /// The names of chia services. These are used as <see cref="Message.Destination"/> values
    /// </summary>
    public struct ServiceNames
    {
        public const string FullNode = "chia_full_node";
        public const string Wallet = "chia_wallet";
        public const string Farmer = "chia_farmer";
        public const string Harvester = "chia_harvester";
        public const string Simulator = "chia_full_node_simulator";
        public const string Plotter = "chia_plotter";
        public const string Daemon = "daemon";
        public const string Crawler = "chia_crawler";
        public const string DataLayer = "chia_data_layer";
    }
}
