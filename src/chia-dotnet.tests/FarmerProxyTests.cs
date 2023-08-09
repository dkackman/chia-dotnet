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

            // this will change the state of the farmer - make sure you want to do this
            // fill in addresses for target and pool as appropriate
            // act
            await Farmer.SetRewardTargets("txch1pacgsfkngcrw50pnuvgvak0qpt8mx9pmey2uxl6p65c9727lhc0sgnklt4", "txch1pacgsfkngcrw50pnuvgvak0qpt8mx9pmey2uxl6p65c9727lhc0sgnklt4");
        }

        //    [Fact]
        //    public async Task GetSignagePoints()
        //    {
        //        using var cts = new CancellationTokenSource(15000);

        //        var signagePoints = await _theFarmer.GetSignagePoints(cts.Token);

        //        foreach (var sp in signagePoints)
        //        {
        //            if (sp.Proofs.Any())
        //            {
        //                System.Diagnostics.Debug.WriteLine("here");
        //            }
        //        }

        //        Assert.IsNotNull(signagePoints);
        //    }

        //    [Fact]
        //    public async Task GetHarvestersSummary()
        //    {
        //        using var cts = new CancellationTokenSource(15000);

        //        var summaries = await _theFarmer.GetHarvestersSummary(cts.Token);

        //        Assert.IsNotNull(summaries);
        //    }

        //    [Fact]
        //    public async Task GetHarvesterPlotsValid()
        //    {
        //        using var cts = new CancellationTokenSource(15000);
        //        var summaries = await _theFarmer.GetHarvestersSummary(cts.Token);
        //        var nodeId = summaries.First().Connection.NodeId;
        //        var requestDatata = new PlotInfoRequestData()
        //        {
        //            NodeId = nodeId,
        //            PageSize = 10,
        //        };

        //        var plotInfo = await _theFarmer.GetHarvesterPlotsValid(requestDatata, cts.Token);

        //        Assert.IsNotNull(plotInfo);
        //    }

        //    [Fact]
        //    public async Task GetHarvesterPlotsKeysMissing()
        //    {
        //        using var cts = new CancellationTokenSource(15000);
        //        var summaries = await _theFarmer.GetHarvestersSummary(cts.Token);
        //        var nodeId = summaries.First().Connection.NodeId;
        //        var requestDatata = new PlotPathRequestData()
        //        {
        //            NodeId = nodeId,
        //            PageSize = 10,
        //        };

        //        var plotInfo = await _theFarmer.GetHarvesterPlotsKeysMissing(requestDatata, cts.Token);

        //        Assert.IsNotNull(plotInfo);
        //    }

        //    [Fact]
        //    public async Task GetSignagePoint()
        //    {
        //        using var cts = new CancellationTokenSource(1005000);

        //        var signagePoints = await _theFarmer.GetSignagePoints(cts.Token);

        //        try
        //        {
        //            foreach (var spInfo in signagePoints)
        //            {
        //                var sp = await _theFarmer.GetSignagePoint(spInfo.SignagePoint.ChallengeChainSp, cts.Token);
        //                if (sp.Proofs.Any())
        //                {
        //                    break;
        //                }
        //                Assert.IsNotNull(sp);
        //            }
        //        }
        //        catch (InvalidOperationException)
        //        {
        //            Assert.Inconclusive("Node has no signage points");
        //        }
        //    }

        //    [Fact]
        //    public async Task GetPoolState()
        //    {
        //        using var cts = new CancellationTokenSource(150000);

        //        var state = await _theFarmer.GetPoolState(cts.Token);

        //        Assert.IsNotNull(state);
        //    }

        //    [Fact]
        //    public async Task GetPoolLoginLink()
        //    {
        //        using var cts = new CancellationTokenSource(15000);

        //        var state = await _theFarmer.GetPoolState(cts.Token);
        //        var pool = state.FirstOrDefault();
        //        if (pool is not null)
        //        {
        //            Assert.IsNotNull(pool.PoolConfig);

        //            var link = await _theFarmer.GetPoolLoginLink(pool.PoolConfig.LauncherId, cts.Token);

        //            Assert.IsNotNull(link);
        //        }
        //        else
        //        {
        //            Assert.Inconclusive("This node isn't part of a pool");
        //        }
        //    }

        //    [Fact]
        //    public async Task GetHarvesters()
        //    {
        //        using var cts = new CancellationTokenSource(15000);

        //        var harvesters = await _theFarmer.GetHarvesters(cts.Token);

        //        Assert.IsNotNull(harvesters);
        //    }

        //    [Fact]
        //    public async Task Ping()
        //    {
        //        using var cts = new CancellationTokenSource(15000);

        //        await _theFarmer.HealthZ(cts.Token);
        //    }

        //    [Fact]
        //    public async Task GetConnections()
        //    {
        //        using var cts = new CancellationTokenSource(15000);

        //        var connections = await _theFarmer.GetConnections(cts.Token);

        //        Assert.IsNotNull(connections);
        //    }

        //    [Fact]
        //    public async Task OpenConnection()
        //    {
        //        using var cts = new CancellationTokenSource(15000);

        //        await _theFarmer.OpenConnection("testnet-node.chia.net", 58444, cts.Token);
        //    }
        //}
    }
}
