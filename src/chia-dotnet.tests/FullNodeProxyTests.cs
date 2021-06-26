using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace chia.dotnet.tests
{
    /// <summary>
    /// This class is a test harness for interation with an actual daemon instance
    /// </summary>
    [TestClass]
    [TestCategory("Integration")]
    //[Ignore] // uncomment to suppress completely
    public class FullNodeProxyTests
    {
        private static Daemon _theDaemon;
        private static FullNodeProxy _theFullNode;

        [ClassInitialize]
        public static async Task Initialize(TestContext context)
        {
            _theDaemon = new Daemon(Config.Open().GetEndpoint("ui"), "unit_tests");

            await _theDaemon.Connect(CancellationToken.None);
            await _theDaemon.Register(CancellationToken.None);
            _theFullNode = new FullNodeProxy(_theDaemon);
        }

        [ClassCleanup()]
        public static void ClassCleanup()
        {
            _theDaemon?.Dispose();
        }

        [TestMethod]
        public async Task GetBlockChainState()
        {
            var state = await _theFullNode.GetBlockchainState(CancellationToken.None);

            Assert.IsNotNull(state);
        }

        [TestMethod]
        public async Task GetBlock()
        {
            var block = await _theFullNode.GetBlock("0xc5d6292aaf50c3cdc3f8481a30b2e9f12babf274c0488ab24db3dd9b1dd41364", CancellationToken.None);

            Assert.IsNotNull(block);
        }

        [TestMethod]
        public async Task GetBlockRecord()
        {
            var record = await _theFullNode.GetBlockRecord("0xc5d6292aaf50c3cdc3f8481a30b2e9f12babf274c0488ab24db3dd9b1dd41364", CancellationToken.None);

            Assert.IsNotNull(record);
        }

        [TestMethod]
        public async Task GetBlocks()
        {
            var blocks = await _theFullNode.GetBlocks(435160, 435167, CancellationToken.None);

            Assert.IsNotNull(blocks);
        }

        [TestMethod()]
        public async Task GetNetworkInfo()
        {
            var info = await _theFullNode.GetNetworkInfo(CancellationToken.None);

            Assert.IsNotNull(info);
        }

        [TestMethod]
        public async Task GetNetworkSpace()
        {
            var space = await _theFullNode.GetNetworkSpace("0x457649e7e6dabb5660f8c3cd9e08534522361d97cb237bdfa341bce01e91c3f5", "0x1353f4d1a01d5393cb7f8f0631487774e11125f47af487426fd9cbcd24151a15", CancellationToken.None);
            Assert.IsNotNull(space);

            Debug.WriteLine(space);
        }

        [TestMethod]
        public async Task Ping()
        {
            await _theFullNode.Ping(CancellationToken.None);
        }

        [TestMethod]
        public async Task GetConnections()
        {
            var connections = await _theFullNode.GetConnections(CancellationToken.None);
            Assert.IsNotNull(connections);
        }

        [TestMethod]
        public async Task OpenConnection()
        {
            await _theFullNode.OpenConnection("node.chia.net", 8444, CancellationToken.None);
        }
    }
}
