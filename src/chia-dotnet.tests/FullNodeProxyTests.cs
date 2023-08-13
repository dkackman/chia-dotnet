using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using chia.dotnet.tests.Core;
using Xunit;
using System.Security.Cryptography;

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
            Assert.NotNull(state.Peak);

            var block = await FullNode.GetBlock(state.Peak.HeaderHash, cts.Token);

            // Assert
            Assert.NotNull(block);
        }

        [Fact]
        public async Task GetBlockRecord()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            var state = await FullNode.GetBlockchainState(cts.Token);

            // Act
            Assert.NotNull(state.Peak);

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
            Assert.NotNull(state.Peak);

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
            var (NetworkName, NetworkPrefix) = await FullNode.GetNetworkInfo(cts.Token);

            //Assert
            Assert.NotNull(NetworkName);
            Assert.NotNull(NetworkPrefix);
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
            Assert.NotNull(state.Peak);
            var newerBlock = await FullNode.GetBlockRecordByHeight(state.Peak.Height, cts.Token);
            var olderBlock = await FullNode.GetBlockRecordByHeight(state.Peak.Height - 5, cts.Token);
            var space = await FullNode.GetNetworkSpace(newerBlock.HeaderHash, olderBlock.HeaderHash, cts.Token);

            // Assert
            Assert.NotEqual(System.Numerics.BigInteger.Zero, space);
        }

        [Fact]
        public async Task GetBlockRecordByHeight()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            var state = await FullNode.GetBlockchainState(cts.Token);

            // Act
            Assert.NotNull(state.Peak);

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
            Assert.NotNull(state.Peak);

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
            using var cts = new CancellationTokenSource(15000);
            var state = await FullNode.GetBlockchainState(cts.Token);
            Assert.NotNull(state.Peak);

            // Act
            var records = await FullNode.GetCoinRecordsByPuzzleHash(state.Peak.FarmerPuzzleHash, false, state.Peak.Height - 10, state.Peak.Height, cts.Token);
            // Assert
            Assert.NotNull(records);
        }

        //broken - investigating
        [Fact(Skip = "Needs data")]
        public async Task GetCoinRecordByName()
        {
            // Arrange
            using var cts = new CancellationTokenSource(35000);
            var state = await FullNode.GetBlockchainState(cts.Token);
            Assert.NotNull(state.Peak);

            var records = await FullNode.GetCoinRecordsByPuzzleHash(state.Peak.FarmerPuzzleHash, false, state.Peak.Height - 10, state.Peak.Height, cts.Token);
            var coin = records.First().Coin;
            var inputString = coin.ParentCoinInfo + coin.PuzzleHash + coin.Amount.ToString(); // this isn't right
            var inputBytes = Encoding.UTF8.GetBytes(inputString);
            using var sha256 = SHA256.Create();
            var hashBytes = sha256.ComputeHash(inputBytes);
            var coinName = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

            // Act
            // this call can take a long time - notice longer timeout in cts
            var coinRecord = await FullNode.GetCoinRecordByName(coinName, cts.Token);

            // Assert
            Assert.NotNull(coinRecord);
        }

        [Fact]
        public async Task GetAdditionsAndRemovals()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            var state = await FullNode.GetBlockchainState(cts.Token);
            Assert.NotNull(state.Peak);

            var blockRecord = await FullNode.GetBlockRecordByHeight(state.Peak.Height - 10, cts.Token);

            // Act
            var (additions, removals) = await FullNode.GetAdditionsAndRemovals(blockRecord.HeaderHash, cts.Token);

            // Assert
            Assert.NotNull(additions);
            Assert.NotNull(removals);
        }

        [Fact]
        public async Task GetBlockSpends()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            var state = await FullNode.GetBlockchainState(cts.Token);
            Assert.NotNull(state.Peak);

            // Act
            var spends = await FullNode.GetBlockSpends(state.Peak.HeaderHash, cts.Token);

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
            var result = await FullNode.GetAverageBlockTime(cts.Token);

            // Assert
            Assert.NotEqual(TimeSpan.Zero, result);
        }

        [Fact(Skip = "not sure how to easily get coin name and solution height")]
        public async Task GetPuzzleAndSolution()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            var items = await FullNode.GetAllMempoolItems(cts.Token);
            var item = items.FirstOrDefault();
            var npc = item.Value.NPCResult.NpcList.First();
            var coinRecord = await FullNode.GetCoinRecordByName(npc.CoinName, cts.Token);

            // Act
            var ps = await FullNode.GetPuzzleAndSolution(npc.CoinName, coinRecord.SpentBlockIndex, cts.Token);

            // Assert
            Assert.NotNull(coinRecord);
            Assert.NotEqual((uint)0, coinRecord.SpentBlockIndex);
            Assert.True(items.Any());
            Assert.NotNull(item.Value);
            Assert.NotNull(npc);
            Assert.NotNull(ps);
        }

        [Fact(Skip = "Requires review")]
        public async Task GetBlockchainState()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);

            // Act
            var returnValue = await FullNode.GetBlockchainState(cts.Token);

            // Assert
            Assert.NotNull(returnValue);
        }

        [Fact(Skip = "Requires review")]
        public async Task GetFeeEstimate()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            SpendBundle spendBundle = null;
            IEnumerable<int> targetTimes = null;

            // Act
            var returnValue = await FullNode.GetFeeEstimate(spendBundle: spendBundle, targetTimes: targetTimes, cancellationToken: cts.Token);

            // Assert
            Assert.NotNull(returnValue);
        }

        [Fact(Skip = "Requires review")]
        public async Task GetCoinRecordsByNames()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            IEnumerable<string> names = null;
            Boolean includeSpentCoins = false;

            // Act
            var returnValue = await FullNode.GetCoinRecordsByNames(names: names, includeSpentCoins: includeSpentCoins, cancellationToken: cts.Token);

            // Assert
            Assert.NotNull(returnValue);
        }

        [Fact(Skip = "Requires review")]
        public async Task GetCoinRecordsByPuzzleHashes()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            IEnumerable<string> puzzlehashes = null;
            Boolean includeSpentCoins = false;

            // Act
            var returnValue = await FullNode.GetCoinRecordsByPuzzleHashes(puzzlehashes: puzzlehashes, includeSpentCoins: includeSpentCoins, cancellationToken: cts.Token);

            // Assert
            Assert.NotNull(returnValue);
        }

        [Fact(Skip = "Requires review")]
        public async Task GetCoinRecordsByParentIds()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            IEnumerable<string> parentIds = null;
            Boolean includeSpentCoins = false;

            // Act
            var returnValue = await FullNode.GetCoinRecordsByParentIds(parentIds: parentIds, includeSpentCoins: includeSpentCoins, cancellationToken: cts.Token);

            // Assert
            Assert.NotNull(returnValue);
        }



        [Fact(Skip = "Requires review")]
        public async Task GetCoinRecordsByHint()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            String hint = string.Empty;
            Boolean includeSpentCoins = false;

            // Act
            var returnValue = await FullNode.GetCoinRecordsByHint(hint: hint, includeSpentCoins: includeSpentCoins, cancellationToken: cts.Token);

            // Assert
            Assert.NotNull(returnValue);
        }

        [Fact(Skip = "Requires review")]
        public async Task GetMemmpooItemByTxId()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            String txId = string.Empty;

            // Act
            var returnValue = await FullNode.GetMemmpooItemByTxId(txId: txId, cancellationToken: cts.Token);

            // Assert
            Assert.NotNull(returnValue);
        }

        [Fact(Skip = "Requires review")]
        public async Task PushTx()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            SpendBundle spendBundle = null;

            // Act
            var returnValue = await FullNode.PushTx(spendBundle: spendBundle, cancellationToken: cts.Token);

            // Assert
            Assert.NotNull(returnValue);
        }

        [Fact(Skip = "Requires review")]
        public async Task GetRecentSignagePoint()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            String spHash = string.Empty;

            // Act
            var returnValue = await FullNode.GetRecentSignagePoint(spHash: spHash, cancellationToken: cts.Token);

            // Assert
            Assert.NotNull(returnValue);
        }

        [Fact(Skip = "Requires review")]
        public async Task GetRecentEOS()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            String challengeHash = string.Empty;

            // Act
            var returnValue = await FullNode.GetRecentEOS(challengeHash: challengeHash, cancellationToken: cts.Token);

            // Assert
            Assert.NotNull(returnValue);
        }
    }
}
