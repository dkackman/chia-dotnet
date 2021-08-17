using System.Linq;
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
    public class FullNodeProxyTests
    {
        private static FullNodeProxy _theFullNode;

        [ClassInitialize]
        public static async Task Initialize(TestContext context)
        {
            using var cts = new CancellationTokenSource(15000);
            var rpcClient = Factory.CreateRpcClientFromHardcodedLocation();
            await rpcClient.Connect(cts.Token);

            var daemon = new DaemonProxy(rpcClient, "unit_tests");
            await daemon.RegisterService(cts.Token);

            _theFullNode = new FullNodeProxy(rpcClient, "unit_tests");
        }

        [ClassCleanup()]
        public static void ClassCleanup()
        {
            _theFullNode.RpcClient?.Dispose();
        }

        [TestMethod]
        public async Task GetBlockChainState()
        {
            using var cts = new CancellationTokenSource(15000);
            var state = await _theFullNode.GetBlockchainState(cts.Token);

            Assert.IsNotNull(state);
        }

        [TestMethod]
        public async Task GetBlock()
        {
            using var cts = new CancellationTokenSource(15000);

            var state = await _theFullNode.GetBlockchainState(cts.Token);
            Assert.IsNotNull(state.Peak, "peak not retreived yet");
            var block = await _theFullNode.GetBlock(state.Peak.HeaderHash, cts.Token);

            Assert.IsNotNull(block);
        }

        [TestMethod]
        public async Task GetBlockRecord()
        {
            using var cts = new CancellationTokenSource(15000);

            var state = await _theFullNode.GetBlockchainState(cts.Token);
            Assert.IsNotNull(state.Peak, "peak not retreived yet");
            var record = await _theFullNode.GetBlockRecord(state.Peak.HeaderHash, cts.Token);

            Assert.IsNotNull(record);
        }

        [TestMethod]
        public async Task GetBlocks()
        {
            using var cts = new CancellationTokenSource(30000);

            var state = await _theFullNode.GetBlockchainState(cts.Token);
            Assert.IsNotNull(state.Peak, "peak not retreived yet");
            var blocks = await _theFullNode.GetBlocks(state.Peak.Height - 5, state.Peak.Height - 1, false, cts.Token);

            Assert.IsNotNull(blocks);
        }

        [TestMethod()]
        public async Task GetNetworkInfo()
        {
            using var cts = new CancellationTokenSource(15000);

            var info = await _theFullNode.GetNetworkInfo(cts.Token);

            Assert.IsNotNull(info);
        }

        [TestMethod]
        public async Task GetNetworkSpace()
        {
            using var cts = new CancellationTokenSource(15000);

            var state = await _theFullNode.GetBlockchainState(cts.Token);
            Assert.IsNotNull(state.Peak, "peak not retreived yet");
            var newerBlock = await _theFullNode.GetBlockRecordByHeight(state.Peak.Height, cts.Token);
            var olderBlock = await _theFullNode.GetBlockRecordByHeight(state.Peak.Height - 5, cts.Token);
            var space = await _theFullNode.GetNetworkSpace(newerBlock.HeaderHash, olderBlock.HeaderHash, cts.Token);

            Assert.IsNotNull(space);
        }

        [TestMethod]
        public async Task Ping()
        {
            using var cts = new CancellationTokenSource(15000);

            await _theFullNode.Ping(cts.Token);
        }

        [TestMethod]
        public async Task GetConnections()
        {
            using var cts = new CancellationTokenSource(15000);

            var connections = await _theFullNode.GetConnections(cts.Token);

            Assert.IsNotNull(connections);
        }

        [TestMethod]
        public async Task OpenConnection()
        {
            using var cts = new CancellationTokenSource(15000);

            await _theFullNode.OpenConnection("testnet-node.chia.net", 58444, cts.Token);
        }

        [TestMethod()]
        public async Task GetBlockRecordByHeight()
        {
            using var cts = new CancellationTokenSource(15000);

            var state = await _theFullNode.GetBlockchainState(cts.Token);
            Assert.IsNotNull(state.Peak, "peak not retreived yet");
            var blockRecord = await _theFullNode.GetBlockRecordByHeight(state.Peak.Height - 1, cts.Token);

            Assert.IsNotNull(blockRecord);
        }

        [TestMethod()]
        public async Task GetBlockRecords()
        {
            using var cts = new CancellationTokenSource(15000);

            var state = await _theFullNode.GetBlockchainState(cts.Token);
            Assert.IsNotNull(state.Peak, "peak not retreived yet");
            var blockRecords = await _theFullNode.GetBlockRecords(state.Peak.Height - 5, state.Peak.Height - 1, cts.Token);

            Assert.IsNotNull(blockRecords);
        }

        [TestMethod()]
        public async Task GetUnfinishedBlockHeaders()
        {
            using var cts = new CancellationTokenSource(15000);

            var blockHeaders = await _theFullNode.GetUnfinishedBlockHeaders(cts.Token);

            Assert.IsNotNull(blockHeaders);
        }

        [TestMethod()]
        public async Task GetCoinRecordsByPuzzleHash()
        {
            using var cts = new CancellationTokenSource(15000);

            var state = await _theFullNode.GetBlockchainState(cts.Token);
            Assert.IsNotNull(state.Peak, "peak not retreived yet");
            var records = await _theFullNode.GetCoinRecordsByPuzzleHash(state.Peak.FarmerPuzzleHash, true, cts.Token);

            Assert.IsNotNull(records);
        }

        [TestMethod()]
        public async Task GetCoinRecordByName()
        {
            using var cts = new CancellationTokenSource(25000);

            var items = await _theFullNode.GetAllMempoolItems(cts.Token);
            var item = items.FirstOrDefault();
            Assert.IsNotNull(item);
            Assert.IsNotNull(item.Value);

            var npc = item.Value.NPCResult.NpcList.FirstOrDefault();
            Assert.IsNotNull(npc);

            // this call can take a long time - notice longer timeout in cts
            var coinRecord = await _theFullNode.GetCoinRecordByName(npc.CoinName, cts.Token);

            Assert.IsNotNull(coinRecord);
        }

        [TestMethod()]
        public async Task GetAdditionsAndRemovals()
        {
            using var cts = new CancellationTokenSource(15000);

            var state = await _theFullNode.GetBlockchainState(cts.Token);
            Assert.IsNotNull(state.Peak, "peak not retreived yet");
            var blockRecord = await _theFullNode.GetBlockRecordByHeight(state.Peak.Height - 10, cts.Token);
            var additionsAndRemovals = await _theFullNode.GetAdditionsAndRemovals(blockRecord.HeaderHash, cts.Token);

            Assert.IsNotNull(additionsAndRemovals);
        }

        [TestMethod()]
        public async Task GetAllMempoolItems()
        {
            using var cts = new CancellationTokenSource(15000);

            var items = await _theFullNode.GetAllMempoolItems(cts.Token);

            Assert.IsNotNull(items);
        }

        [TestMethod()]
        public async Task GetAllMemmpoolTxIds()
        {
            using var cts = new CancellationTokenSource(15000);

            var ids = await _theFullNode.GetAllMemmpoolTxIds(cts.Token);

            Assert.IsNotNull(ids);
        }

        [TestMethod()]
        public async Task GetMemmpooItemByTxId()
        {
            using var cts = new CancellationTokenSource(15000);

            var ids = await _theFullNode.GetAllMemmpoolTxIds(cts.Token);
            Assert.IsNotNull(ids);
            Assert.IsTrue(ids.Count() > 0);

            var item = await _theFullNode.GetMemmpooItemByTxId(ids.First(), cts.Token);

            Assert.IsNotNull(item);
        }

        [TestMethod()]
        public async Task GetRecentSignagePoint()
        {
            using var cts = new CancellationTokenSource(15000);

            var farmer = new FarmerProxy(_theFullNode.RpcClient, _theFullNode.OriginService); // this only works with dameon's endpoint
            var signagePoints = await farmer.GetSignagePoints(cts.Token);
            var spInfo = signagePoints.FirstOrDefault();
            Assert.IsNotNull(spInfo);

            var sp = await _theFullNode.GetRecentSignagePoint(spInfo.SignagePoint.ChallengeChainSp, cts.Token);

            Assert.IsNotNull(sp);
        }

        [TestMethod()]
        public async Task GetRecentEOS()
        {
            using var cts = new CancellationTokenSource(15000);

            var farmer = new FarmerProxy(_theFullNode.RpcClient, _theFullNode.OriginService); // this only works with dameon's endpoint
            var signagePoints = await farmer.GetSignagePoints(cts.Token);
            var spInfo = signagePoints.FirstOrDefault();
            Assert.IsNotNull(spInfo);
            var eos = await _theFullNode.GetRecentEOS(spInfo.SignagePoint.ChallengeHash, cts.Token);

            Assert.IsNotNull(eos);
        }

        [TestMethod()]
        [Ignore("not sure how to easily get coin name and solution height")]
        public async Task GetPuzzleAndSolution()
        {
            using var cts = new CancellationTokenSource(15000);

            var items = await _theFullNode.GetAllMempoolItems(cts.Token);
            Assert.IsTrue(items.Any());

            var item = items.FirstOrDefault();
            Assert.IsNotNull(item);
            Assert.IsNotNull(item.Value);

            var npc = item.Value.NPCResult.NpcList.FirstOrDefault();
            Assert.IsNotNull(npc);

            var coinRecord = await _theFullNode.GetCoinRecordByName(npc.CoinName, cts.Token);
            Assert.IsNotNull(coinRecord);
            Assert.AreNotEqual(coinRecord.SpentBlockIndex, 0);

            var ps = await _theFullNode.GetPuzzleAndSolution(npc.CoinName, coinRecord.SpentBlockIndex, cts.Token);

            Assert.IsNotNull(ps);
        }
    }
}
