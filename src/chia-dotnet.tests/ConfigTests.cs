using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace chia.dotnet.tests
{
    [TestClass]
    [DeploymentItem(@"chia-dotnet.tests\config.yaml")]
    public class ConfigTests
    {
        [TestMethod]
        [TestCategory("Integration")]
        public void CanOpenDefaultConfig()
        {
            var config = Config.Open();

            Assert.IsNotNull(config);
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void InvalidFilenameThrowsFileNotFound()
        {
            _ = Config.Open(@"C:\this\path\does\not\exist\config.yaml");
        }

        [TestMethod]
        public void CanOpenConfigFromPath()
        {
            var file = new FileInfo("config.yaml");

            var config = Config.Open(file.FullName);

            Assert.IsNotNull(config);
        }

        [TestMethod]
        public void GetUIEndpointFromConfig()
        {
            var file = new FileInfo("config.yaml");
            var config = Config.Open(file.FullName);

            var endpoint = config.GetEndpoint("ui");
            Assert.IsNotNull(endpoint);
        }


        [TestMethod]
        public void GetDaemonEndpointFromConfig()
        {
            var file = new FileInfo("config.yaml");
            var config = Config.Open(file.FullName);

            var endpoint = config.GetEndpoint("daemon");
            Assert.IsNotNull(endpoint);
        }

        [TestMethod]
        public void GetFullNodeEndpointFromConfig()
        {
            var file = new FileInfo("config.yaml");
            var config = Config.Open(file.FullName);

            var endpoint = config.GetEndpoint("full_node");
            Assert.IsNotNull(endpoint);
        }

        [TestMethod]
        public void GetHarvesterEndpointFromConfig()
        {
            var file = new FileInfo("config.yaml");
            var config = Config.Open(file.FullName);

            var endpoint = config.GetEndpoint("harvester");
            Assert.IsNotNull(endpoint);
        }

        [TestMethod]
        public void GetFarmerEndpointFromConfig()
        {
            var file = new FileInfo("config.yaml");
            var config = Config.Open(file.FullName);

            var endpoint = config.GetEndpoint("farmer");
            Assert.IsNotNull(endpoint);
        }

        [TestMethod]
        public void GetWalletEndpointFromConfig()
        {
            var file = new FileInfo("config.yaml");
            var config = Config.Open(file.FullName);

            var endpoint = config.GetEndpoint("wallet");
            Assert.IsNotNull(endpoint);
        }
    }
}
