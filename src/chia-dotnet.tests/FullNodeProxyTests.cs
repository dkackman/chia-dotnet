using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using chia.dotnet.tests.Core;
using Xunit;


namespace chia.dotnet.tests
{
    public class FullNodeProxyTests : TestBase
    {
        public FullNodeProxyTests(ChiaDotNetFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task GetBlockChainState()
        {
            // arrange
            var cts = new CancellationTokenSource(12000);

            // act
            var state = await FullNode.GetBlockchainState(cts.Token);

            // assert
            Assert.NotNull(state);
        }

        [Fact]
        public async Task GetBlock()
        {
            // Arrange
            using var cts = new CancellationTokenSource(2000);
            var state = await FullNode.GetBlockchainState(cts.Token);

            // Act
            var block = await FullNode.GetBlock(state.Peak.HeaderHash, cts.Token);

            // Assert
            Assert.NotNull((block));
        }

        [Fact]
        public async Task GetBlockRecord()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            var state = await FullNode.GetBlockchainState(cts.Token);

            // Act
            var record = await FullNode.GetBlockRecord(state.Peak.HeaderHash, cts.Token);

            // Assert
            Assert.NotNull(record);
        }


        [Fact]
        public async Task GetBlocks()
        {
            // Arrange
            using var cts = new CancellationTokenSource(30000);
            var state = await FullNode.GetBlockchainState(cts.Token);

            // Act
            var blocks = await FullNode.GetBlocks(state.Peak.Height - 5, state.Peak.Height - 1, cancellationToken: cts.Token);

            // Assert
            Assert.NotNull(blocks);
        }


        [Fact]
        public async Task GetNetworkInfo()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);

            // Act
            var info = await FullNode.GetNetworkInfo(cts.Token);

            //Assert
            Assert.NotNull(info);
        }

        [Fact]
        public async Task GetBlockCountMetrics()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);

            // Act
            var counts = await FullNode.GetBlockCountMetrics(cts.Token);

            // Assert
            Assert.NotNull(counts);
        }

        [Fact]
        public async Task GetNetworkSpace()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);

            // Act
            var state = await FullNode.GetBlockchainState(cts.Token);
            var newerBlock = await FullNode.GetBlockRecordByHeight(state.Peak.Height, cts.Token);
            var olderBlock = await FullNode.GetBlockRecordByHeight(state.Peak.Height - 5, cts.Token);
            var space = await FullNode.GetNetworkSpace(newerBlock.HeaderHash, olderBlock.HeaderHash, cts.Token);

            // Assert
            Assert.NotNull(space);
        }

        [Fact]
        public async Task Ping()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);

            // Act
            await FullNode.HealthZ(cts.Token);
        }

        [Fact]
        public async Task GetConnections()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);

            // Act
            var connections = await FullNode.GetConnections(cts.Token);

            // Assert
            Assert.NotNull(connections);
        }

        // [Fact]
        // public async Task OpenConnection()
        // {
        //     // Arrange
        //     using var cts = new CancellationTokenSource(15000);
        //
        //     // Act
        //     await FullNode.OpenConnection("testnet-node.chia.net", 58444, cts.Token);
        // }

        [Fact]
        public async Task GetBlockRecordByHeight()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);

            // Act
            var state = await FullNode.GetBlockchainState(cts.Token);
            var blockRecord = await FullNode.GetBlockRecordByHeight(state.Peak.Height - 1, cts.Token);

            // Assert
            Assert.NotNull(blockRecord);
        }

        [Fact]
        public async Task GetBlockRecords()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);

            // Act
            var state = await FullNode.GetBlockchainState(cts.Token);
            var blockRecords = await FullNode.GetBlockRecords(state.Peak.Height - 5, state.Peak.Height - 1, cts.Token);

            // Assert
            Assert.NotNull(blockRecords);
        }

        [Fact]
        public async Task GetUnfinishedBlockHeaders()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);

            // Act
            var blockHeaders = await FullNode.GetUnfinishedBlockHeaders(cts.Token);

            // Assert
            Assert.NotNull(blockHeaders);
        }

        [Fact]
        public async Task GetCoinRecordsByPuzzleHash()
        {
            // Arrange
            using var cts = new CancellationTokenSource(35000);

            // Act
            var state = await FullNode.GetBlockchainState(cts.Token);
            var records =
                await FullNode.GetCoinRecordsByPuzzleHash(state.Peak.FarmerPuzzleHash, true, null, null, cts.Token);

            // Assert
            Assert.NotNull(records);
        }

        //broken - investigating
        // [Fact]
        // public async Task GetCoinRecordByName()
        // {
        //     // Arrange
        //     using var cts = new CancellationTokenSource(35000);
        //     var items = await FullNode.GetAllMempoolItems(cts.Token);
        //     var item = items.FirstOrDefault();
        //     var npc = item.Value.NPCResult.NpcList.FirstOrDefault();
        //
        //     // Act
        //     // this call can take a long time - notice longer timeout in cts
        //     var coinRecord = await FullNode.GetCoinRecordByName(npc.CoinName, cts.Token);
        //
        //     // Assert
        //     Assert.NotNull(npc);
        //     Assert.NotNull(item.Key);
        //     Assert.NotNull(item.Value);
        //     Assert.NotNull(coinRecord);
        // }

        [Fact]
        public async Task GetAdditionsAndRemovals()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            var state = await FullNode.GetBlockchainState(cts.Token);
            var blockRecord = await FullNode.GetBlockRecordByHeight(state.Peak.Height - 10, cts.Token);

            // Act
            var additionsAndRemovals = await FullNode.GetAdditionsAndRemovals(blockRecord.HeaderHash, cts.Token);

            // Assert
            Assert.NotNull(additionsAndRemovals);
        }

        [Fact]
        public async Task GetBlockSpends()
        {
            // Arrange
            var header = "0xaa8425c198253b96a15c40e32ef4c1fd36e751f0ff9a90199e8751df381eac71"; //hash from today

            // Act
            using var cts = new CancellationTokenSource(15000);
            var spends = await FullNode.GetBlockSpends(header, cts.Token);

            // Assert
            Assert.NotNull(spends);
        }

        [Fact]
        public async Task GetAllMempoolItems()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);

            // Act
            var items = await FullNode.GetAllMempoolItems(cts.Token);

            // Assert
            Assert.NotNull(items);
        }

        [Fact]
        public async Task GetAllMemmpoolTxIds()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);

            // Act
            var ids = await FullNode.GetAllMemmpoolTxIds(cts.Token);

            // Assert
            Assert.NotNull(ids);
        }

        //[Fact]
        // public async Task GetMemmpooItemByTxId()
        // {
        //     // Arrange
        //     using var cts = new CancellationTokenSource(15000);
        //     var ids = await FullNode.GetAllMemmpoolTxIds(cts.Token);
        //     Assert.NotNull(ids);
        //     Assert.True(ids.Any());
        //
        //     // Act
        //     var item = await FullNode.GetMemmpooItemByTxId(ids.First(), cts.Token);
        //
        //     // Assert
        //     Assert.NotNull(item);
        // }

        // [Fact]
        // public async Task GetRecentSignagePoint()
        // {
        //     // Arrange
        //     using var cts = new CancellationTokenSource(15000);
        //     var signagePoints = await Farmer.GetSignagePoints(cts.Token);
        //     var spInfo = signagePoints.FirstOrDefault();
        //
        //     // Act
        //     var sp = await FullNode.GetRecentSignagePoint(spInfo.SignagePoint.ChallengeChainSp, cts.Token);
        //
        //     // Assert
        //     Assert.NotNull(spInfo);
        //     Assert.NotNull(sp);
        // }

        //broken - investigating
        // [Fact]
        // public async Task GetRecentEOS()
        // {
        //     // Arrange
        //     using var cts = new CancellationTokenSource(35000);
        //     var signagePoints = await Farmer.GetSignagePoints(cts.Token);
        //     var spInfo = signagePoints.FirstOrDefault();
        //
        //     // act
        //     var eos = await FullNode.GetRecentEOS(spInfo.SignagePoint.ChallengeHash, cts.Token);
        //
        //     // Assert
        //     Assert.NotNull(spInfo);
        //     Assert.NotNull(eos);
        // }

        [Fact]
        public async Task GetAverageBlockTime()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);

            // Act
            var timeSpan = await FullNode.GetAverageBlockTime(cts.Token);

            // Assert
            Assert.NotNull(timeSpan);
        }

        [Fact(Skip = "not sure how to easily get coin name and solution height")]
        public async Task GetPuzzleAndSolution()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            var items = await FullNode.GetAllMempoolItems(cts.Token);
            var item = items.FirstOrDefault();
            var npc = item.Value.NPCResult.NpcList.FirstOrDefault();
            var coinRecord = await FullNode.GetCoinRecordByName(npc.CoinName, cts.Token);

            // Act
            var ps = await FullNode.GetPuzzleAndSolution(npc.CoinName, coinRecord.SpentBlockIndex, cts.Token);

            // Assert
            Assert.NotNull(coinRecord);
            Assert.NotSame(coinRecord.SpentBlockIndex, 0);
            Assert.True(items.Any());
            Assert.NotNull(item);
            Assert.NotNull(item.Value);
            Assert.NotNull(npc);
            Assert.NotNull(ps);
        }
    }
}
