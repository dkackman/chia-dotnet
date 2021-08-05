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
    public class FarmerProxyTests
    {
        private static FarmerProxy _theFarmer;

        [ClassInitialize]
        public static async Task Initialize(TestContext context)
        {
            using var cts = new CancellationTokenSource(15000);
            var rpcClient = Factory.CreateRpcClientFromHardcodedLocation();
            await rpcClient.Connect(cts.Token);

            var daemon = new DaemonProxy(rpcClient, "unit_tests");
            await daemon.RegisterService(cts.Token);

            _theFarmer = new FarmerProxy(rpcClient, "unit_tests");
        }

        [ClassCleanup()]
        public static void ClassCleanup()
        {
            _theFarmer.RpcClient?.Dispose();
        }

        [TestMethod]
        public async Task GetRewardTargets()
        {
            using var cts = new CancellationTokenSource(15000);

            var targets = await _theFarmer.GetRewardTargets(false);

            Assert.IsNotNull(targets);
            Assert.IsFalse(string.IsNullOrEmpty(targets.FarmerTarget));
            Assert.IsFalse(string.IsNullOrEmpty(targets.PoolTarget));
        }

        [TestMethod]
        [TestCategory("CAUTION")]
        public async Task SetRewardTargets()
        {
            using var cts = new CancellationTokenSource(15000);

            // this will change the state of the farmer - make sure you want to do this
            // fill in addresses for target and pool as appropriate
            await _theFarmer.SetRewardTargets("txch1pacgsfkngcrw50pnuvgvak0qpt8mx9pmey2uxl6p65c9727lhc0sgnklt4", "txch1pacgsfkngcrw50pnuvgvak0qpt8mx9pmey2uxl6p65c9727lhc0sgnklt4");
        }

        [TestMethod]
        public async Task GetSignagePoints()
        {
            using var cts = new CancellationTokenSource(15000);

            var signagePoints = await _theFarmer.GetSignagePoints(cts.Token);

            Assert.IsNotNull(signagePoints);
        }

        [TestMethod]
        [ExpectedException(typeof(ResponseException))]
        public async Task GetSignagePoint()
        {
            using var cts = new CancellationTokenSource(15000);

            _ = await _theFarmer.GetSignagePoint("fake", cts.Token);
        }

        [TestMethod]
        public async Task GetHarvesters()
        {
            using var cts = new CancellationTokenSource(15000);

            var harvesters = await _theFarmer.GetHarvesters(cts.Token);
            Assert.IsNotNull(harvesters);
        }

        [TestMethod]
        public async Task Ping()
        {
            using var cts = new CancellationTokenSource(15000);

            await _theFarmer.Ping(cts.Token);
        }

        [TestMethod]
        public async Task GetConnections()
        {
            using var cts = new CancellationTokenSource(15000);

            var connections = await _theFarmer.GetConnections(cts.Token);
            Assert.IsNotNull(connections);
        }

        [TestMethod]
        [Ignore] // only works on mainnet
        public async Task OpenConnection()
        {
            using var cts = new CancellationTokenSource(15000);

            await _theFarmer.OpenConnection("node.chia.net", 8444, cts.Token);
        }
    }
}
