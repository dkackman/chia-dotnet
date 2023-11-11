using System.IO;

using Xunit;

namespace chia.dotnet.tests
{
    public class ConfigTests
    {
        [Fact]
        public void CanOpenDefaultConfig()
        {
            var config = Config.Open();

            Assert.NotNull(config);
        }

        [Fact]
        public void CanGetDefaultDatalayerEndpoint()
        {
            var file = new FileInfo("config.yaml");
            var config = Config.Open(file.FullName);

            var endpoint = config.GetEndpoint("data_layer");
            Assert.NotNull(endpoint);
            Assert.Equal(8562, endpoint.Uri.Port);
        }

        [Fact]
        public void CanOpenConfigFromPath()
        {
            var file = new FileInfo("config.yaml");

            var config = Config.Open(file.FullName);

            Assert.NotNull(config);
        }

        [Fact]
        public void GetUIEndpointFromConfig()
        {
            var file = new FileInfo("config.yaml");
            var config = Config.Open(file.FullName);

            var endpoint = config.GetEndpoint("ui");
            Assert.NotNull(endpoint);
        }


        [Fact]
        public void GetDaemonEndpointFromConfig()
        {
            var file = new FileInfo("config.yaml");
            var config = Config.Open(file.FullName);

            var endpoint = config.GetEndpoint("daemon");
            Assert.NotNull(endpoint);
        }

        [Fact]
        public void GetFullNodeEndpointFromConfig()
        {
            var file = new FileInfo("config.yaml");
            var config = Config.Open(file.FullName);

            var endpoint = config.GetEndpoint("full_node");
            Assert.NotNull(endpoint);
        }

        [Fact]
        public void GetHarvesterEndpointFromConfig()
        {
            var file = new FileInfo("config.yaml");
            var config = Config.Open(file.FullName);

            var endpoint = config.GetEndpoint("harvester");
            Assert.NotNull(endpoint);
        }

        [Fact]
        public void GetFarmerEndpointFromConfig()
        {
            var file = new FileInfo("config.yaml");
            var config = Config.Open(file.FullName);

            var endpoint = config.GetEndpoint("farmer");
            Assert.NotNull(endpoint);
        }

        [Fact]
        public void GetWalletEndpointFromConfig()
        {
            var file = new FileInfo("config.yaml");
            var config = Config.Open(file.FullName);

            var endpoint = config.GetEndpoint("wallet");
            Assert.NotNull(endpoint);
        }
    }
}
