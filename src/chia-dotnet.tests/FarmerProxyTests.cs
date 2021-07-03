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
        private static Daemon _theDaemon;
        private static FarmerProxy _theFarmer;

        [ClassInitialize]
        public static async Task Initialize(TestContext context)
        {
            _theDaemon = Factory.CreateDaemon();

            await _theDaemon.Connect(CancellationToken.None);
            await _theDaemon.Register(CancellationToken.None);
            _theFarmer = new FarmerProxy(_theDaemon);
        }

        [ClassCleanup()]
        public static void ClassCleanup()
        {
            _theDaemon?.Dispose();
        }

        [TestMethod]
        public async Task GetRewardTargets()
        {
            var targets = await _theFarmer.GetRewardTargets(false, CancellationToken.None);

            Assert.IsNotNull(targets);
            Assert.IsFalse(string.IsNullOrEmpty(targets.FarmerTarget));
            Assert.IsFalse(string.IsNullOrEmpty(targets.PoolTarget));
        }

        [TestMethod]
        [TestCategory("CAUTION")]
        public async Task SetRewardTargets()
        {
            // this will change the state of the farmer - make sure you want to do this
            // fill in addresses for target and pool as appropriate
            await _theFarmer.SetRewardTargets("txch1pacgsfkngcrw50pnuvgvak0qpt8mx9pmey2uxl6p65c9727lhc0sgnklt4", "txch1pacgsfkngcrw50pnuvgvak0qpt8mx9pmey2uxl6p65c9727lhc0sgnklt4", CancellationToken.None);
        }

        [TestMethod]
        public async Task GetSignagePoints()
        {
            var signagePoints = await _theFarmer.GetSignagePoints(CancellationToken.None);

            Assert.IsNotNull(signagePoints);
        }

        [TestMethod]
        public async Task GetSignagePoint()
        {
            var signagePoint = await _theFarmer.GetSignagePoint("0xb040c8e61a875249736136734467f505cd404f7803892e70fea2bac3cc0a6f0a", CancellationToken.None);

            Assert.IsNotNull(signagePoint);
        }

        [TestMethod]
        public async Task GetPlots()
        {
            var plots = await _theFarmer.GetPlots(CancellationToken.None);
            Assert.IsNotNull(plots);
        }

        [TestMethod]
        public async Task Ping()
        {
            await _theFarmer.Ping(CancellationToken.None);
        }

        [TestMethod]
        public async Task GetConnections()
        {
            var connections = await _theFarmer.GetConnections(CancellationToken.None);
            Assert.IsNotNull(connections);
        }

        [TestMethod]
        public async Task OpenConnection()
        {
            await _theFarmer.OpenConnection("node.chia.net", 8444, CancellationToken.None);
        }
    }
}
