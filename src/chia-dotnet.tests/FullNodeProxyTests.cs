using System.Diagnostics;
using System.Threading;
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
            _theDaemon = Factory.CreateDaemon();

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

        [TestMethod()]
        public async Task GetBlockRecordByHeight()
        {
            var blockRecord = await _theFullNode.GetBlockRecordByHeight(12441, CancellationToken.None);
            Assert.IsNotNull(blockRecord);
        }

        [TestMethod()]
        public async Task GetBlockRecords()
        {
            var blockRecords = await _theFullNode.GetBlockRecords(12000, 12441, CancellationToken.None);
            Assert.IsNotNull(blockRecords);
        }


        [TestMethod()]
        public async Task GetUnfinishedBlockHeaders()
        {
            var blockHeaders = await _theFullNode.GetUnfinishedBlockHeaders(CancellationToken.None);
            Assert.IsNotNull(blockHeaders);
        }

        [TestMethod()]
        public async Task GetCoinRecordsByPuzzleHash()
        {
            var records = await _theFullNode.GetCoinRecordsByPuzzleHash("0xb5a83c005c4ee98dc807a560ea5bc361d6d3b32d2f4d75061351d1f6d2b6085f", true, CancellationToken.None);
            Assert.IsNotNull(records);
        }

        [TestMethod()]
        public async Task GetCoinRecordByName()
        {
            var coinRecord = await _theFullNode.GetCoinRecordByName("0x2b83ca807d305cd142e0e91d4e7a18f8e57df0ac6b4fa403bff249d0d491c609", CancellationToken.None);
            Assert.IsNotNull(coinRecord);
        }

        [TestMethod()]
        public async Task GetAdditionsAndRemovals()
        {
            var additionsAndRemovals = await _theFullNode.GetAdditionsAndRemovals("6b143b214f731021106d411c5bdce2fe03de0af5449c63830f111f25dc7d0a2b", CancellationToken.None);
            Assert.IsNotNull(additionsAndRemovals);
        }

        [TestMethod()]
        public async Task GetAllMempoolItems()
        {
            var items = await _theFullNode.GetAllMempoolItems(CancellationToken.None);
            Assert.IsNotNull(items);
        }

        [TestMethod()]
        public async Task GetAllMemmpoolTxIds()
        {
            var ids = await _theFullNode.GetAllMemmpoolTxIds(CancellationToken.None);
            Assert.IsNotNull(ids);
        }

        [TestMethod()]
        public async Task GetMemmpooItemByTxId()
        {
            var ids = await _theFullNode.GetAllMemmpoolTxIds(CancellationToken.None);
            Assert.IsNotNull(ids);
            Assert.IsTrue(ids.Count() > 0);

            var item = await _theFullNode.GetMemmpooItemByTxId((string)ids.First(), CancellationToken.None);
            Assert.IsNotNull(item);
        }
    }
}
