using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using chia.dotnet.tests.Core;
using Xunit;

namespace chia.dotnet.tests
{
    public class FarmerProxyTests : TestBase
    {
        public FarmerProxyTests(ChiaDotNetFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task GetRewardTargets()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);

            // Act
            var targets = await Farmer.GetRewardTargets(cts.Token);

            // Assert
            Assert.False(string.IsNullOrEmpty(targets.FarmerTarget));
            Assert.False(string.IsNullOrEmpty(targets.PoolTarget));
        }

        [Fact]
        public async Task SetRewardTargets()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);

            // Act
            // this will change the state of the farmer - make sure you want to do this
            // fill in addresses for target and pool as appropriate
            await Farmer.SetRewardTargets("txch1pacgsfkngcrw50pnuvgvak0qpt8mx9pmey2uxl6p65c9727lhc0sgnklt4", "txch1pacgsfkngcrw50pnuvgvak0qpt8mx9pmey2uxl6p65c9727lhc0sgnklt4");
        }

        [Fact]
        public async Task GetSignagePoints()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);

            // Act
            var signagePoints = await Farmer.GetSignagePoints(cts.Token);

            // Assert
            Assert.NotNull(signagePoints);
        }

        [Fact]
        public async Task GetHarvestersSummary()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);

            // Act
            var summaries = await Farmer.GetHarvestersSummary(cts.Token);

            // Assert
            Assert.NotNull(summaries);
        }

        [Fact]
        public async Task GetHarvesterPlotsValid()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            var summaries = await Farmer.GetHarvestersSummary(cts.Token);
            var nodeId = summaries.First().Connection.NodeId;
            var requestData = new PlotInfoRequestData()
            {
                NodeId = nodeId,
                PageSize = 10,
            };

            // Act
            var plotInfo = await Farmer.GetHarvesterPlotsValid(requestData, cts.Token);

            // Assert
            Assert.NotNull(plotInfo);
        }

        [Fact]
        public async Task GetHarvesterPlotsKeysMissing()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            var summaries = await Farmer.GetHarvestersSummary(cts.Token);
            var nodeId = summaries.First().Connection.NodeId;
            var requestData = new PlotPathRequestData()
            {
                NodeId = nodeId,
                PageSize = 10,
            };

            // Act
            var plotInfo = await Farmer.GetHarvesterPlotsKeysMissing(requestData, cts.Token);

            // Assert
            Assert.NotNull(plotInfo);
        }

        [Fact]
        public async Task GetSignagePoint()
        {
            // Arrange
            using var cts = new CancellationTokenSource(1005000);

            // Act
            var signagePoints = await Farmer.GetSignagePoints(cts.Token);

            foreach (var spInfo in signagePoints)
            {
                _ = await Farmer.GetSignagePoint(spInfo.SignagePoint.ChallengeChainSp, cts.Token);
            }
        }

        [Fact]
        public async Task GetPoolState()
        {
            // Arrange
            using var cts = new CancellationTokenSource(150000);

            // Act
            var state = await Farmer.GetPoolState(cts.Token);

            // Assert
            Assert.NotNull(state);
        }

        [Fact]
        public async Task GetPoolLoginLink()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);

            var state = await Farmer.GetPoolState(cts.Token);
            var pool = state.FirstOrDefault();
            if (pool is not null)
            {
                // Act
                var link = await Farmer.GetPoolLoginLink(pool.PoolConfig.LauncherId, cts.Token);

                // Assert
                Assert.NotNull(pool.PoolConfig);
                Assert.NotNull(link);
            }
            else
            {
                Debug.WriteLine("This node isn't part of a pool");
            }
        }

        [Fact]
        public async Task GetHarvesters()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);

            // Act
            var harvesters = await Farmer.GetHarvesters(cts.Token);

            // Assert
            Assert.NotNull(harvesters);
        }

        [Fact]
        public async Task GetHarvesterPlotsInvalid()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            var summaries = await Farmer.GetHarvestersSummary(cts.Token);
            var nodeId = summaries.First().Connection.NodeId;
            var requestData = new PlotPathRequestData()
            {
                NodeId = nodeId,
                PageSize = 10,
            };

            // Act
            var returnValue = await Farmer.GetHarvesterPlotsInvalid(requestData: requestData, cancellationToken: cts.Token);

            // Assert
            Assert.NotNull(returnValue);
        }

        [Fact(Skip = "Requires review")]
        public async Task SetPayoutInstructions()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            var launcherID = string.Empty;
            var payoutInstructions = string.Empty;

            // Act
            await Farmer.SetPayoutInstructions(launcherID: launcherID, payoutInstructions: payoutInstructions, cancellationToken: cts.Token);

            // Assert

        }

        [Fact]
        public async Task GetHarvesterPlotsDuplicates()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            var summaries = await Farmer.GetHarvestersSummary(cts.Token);
            var nodeId = summaries.First().Connection.NodeId;
            var requestData = new PlotPathRequestData()
            {
                NodeId = nodeId,
                PageSize = 10,
            };

            // Act
            var returnValue = await Farmer.GetHarvesterPlotsDuplicates(requestData: requestData, cancellationToken: cts.Token);

            // Assert
            Assert.NotNull(returnValue);
        }
    }
}
