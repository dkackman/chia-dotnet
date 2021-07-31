using System.Diagnostics;
using System.Threading.Tasks;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace chia.dotnet.tests
{
    /// <summary>
    /// This class is a test harness for interation with an actual daemon instance
    /// </summary>
    [TestClass]
    [TestCategory("Integration")]
    public class FullNodeProxyTests
    {
        private static Daemon _theDaemon;
        private static FullNodeProxy _theFullNode;

        [ClassInitialize]
        public static async Task Initialize(TestContext context)
        {
            _theDaemon = DaemonFactory.CreateDaemonFromHardcodedLocation();

            await _theDaemon.Connect();
            await _theDaemon.Register();
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
            var state = await _theFullNode.GetBlockchainState();

            Assert.IsNotNull(state);
        }

        [TestMethod]
        public async Task GetBlock()
        {
            var block = await _theFullNode.GetBlock("0xcb5c085a1f0259ab5581ebfce219f82cac9ec288da29665ce31e21a5b5856089");

            Assert.IsNotNull(block);
        }

        [TestMethod]
        public async Task GetBlockRecord()
        {
            var record = await _theFullNode.GetBlockRecord("0xcb5c085a1f0259ab5581ebfce219f82cac9ec288da29665ce31e21a5b5856089");

            Assert.IsNotNull(record);
        }

        [TestMethod]
        public async Task GetBlocks()
        {
            var blocks = await _theFullNode.GetBlocks(435160, 435167, false);

            Assert.IsNotNull(blocks);
        }

        [TestMethod()]
        public async Task GetNetworkInfo()
        {
            var info = await _theFullNode.GetNetworkInfo();

            Assert.IsNotNull(info);
        }

        [TestMethod]
        public async Task GetNetworkSpace()
        {
            var space = await _theFullNode.GetNetworkSpace("0xcb5c085a1f0259ab5581ebfce219f82cac9ec288da29665ce31e21a5b5856089", "0x9edc235dfcb12a14e20e8f83b53060d067d97d217d6f9a3420fc9dbb470040fe");
            Assert.IsNotNull(space);
        }

        [TestMethod]
        public async Task Ping()
        {
            await _theFullNode.Ping();
        }

        [TestMethod]
        public async Task GetConnections()
        {
            var connections = await _theFullNode.GetConnections();
            Assert.IsNotNull(connections);
        }

        [TestMethod]
        [Ignore] // only works on mainnet
        public async Task OpenConnection()
        {
            await _theFullNode.OpenConnection("node.chia.net", 8444);
        }

        [TestMethod()]
        public async Task GetBlockRecordByHeight()
        {
            var blockRecord = await _theFullNode.GetBlockRecordByHeight(12441);
            Assert.IsNotNull(blockRecord);
        }

        [TestMethod()]
        public async Task GetBlockRecords()
        {
            var blockRecords = await _theFullNode.GetBlockRecords(12000, 12441);
            Assert.IsNotNull(blockRecords);
        }


        [TestMethod()]
        public async Task GetUnfinishedBlockHeaders()
        {
            var blockHeaders = await _theFullNode.GetUnfinishedBlockHeaders();
            Assert.IsNotNull(blockHeaders);
        }

        [TestMethod()]
        public async Task GetCoinRecordsByPuzzleHash()
        {
            var records = await _theFullNode.GetCoinRecordsByPuzzleHash("0xb5a83c005c4ee98dc807a560ea5bc361d6d3b32d2f4d75061351d1f6d2b6085f", true);
            Assert.IsNotNull(records);
        }

        [TestMethod()]
        public async Task GetCoinRecordByName()
        {
            var coinRecord = await _theFullNode.GetCoinRecordByName("0x2b83ca807d305cd142e0e91d4e7a18f8e57df0ac6b4fa403bff249d0d491c609");
            Assert.IsNotNull(coinRecord);
        }

        [TestMethod()]
        public async Task GetAdditionsAndRemovals()
        {
            var additionsAndRemovals = await _theFullNode.GetAdditionsAndRemovals("7d83874e532ea08b0a5882ce8df705a5f45fc94bdeae4b1f568f05ce3010c6ae");
            Assert.IsNotNull(additionsAndRemovals);
        }

        [TestMethod()]
        public async Task GetAllMempoolItems()
        {
            var items = await _theFullNode.GetAllMempoolItems();
            Assert.IsNotNull(items);
        }

        [TestMethod()]
        public async Task GetAllMemmpoolTxIds()
        {
            var ids = await _theFullNode.GetAllMemmpoolTxIds();
            Assert.IsNotNull(ids);
        }

        [TestMethod()]
        public async Task GetMemmpooItemByTxId()
        {
            var ids = await _theFullNode.GetAllMemmpoolTxIds();
            Assert.IsNotNull(ids);
            Assert.IsTrue(ids.Count() > 0);

            var item = await _theFullNode.GetMemmpooItemByTxId((string)ids.First());
            Assert.IsNotNull(item);
        }

        [TestMethod()]
        public async Task GetRecentSignagePoint()
        {
            var sp = await _theFullNode.GetRecentSignagePoint("0xf3ca7a33ce723b38c9a72156252b3b2395ead751213eef5d8ed40c941c6a9017");
            Assert.IsNotNull(sp);
        }
    }
}
