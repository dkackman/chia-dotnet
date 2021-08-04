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
            _theDaemon = DaemonFactory.CreateDaemonFromHardcodedLocation(ServiceNames.Farmer);

            await _theDaemon.Connect();
            await _theDaemon.Register();
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
            var targets = await _theFarmer.GetRewardTargets(false);

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
            await _theFarmer.SetRewardTargets("txch1pacgsfkngcrw50pnuvgvak0qpt8mx9pmey2uxl6p65c9727lhc0sgnklt4", "txch1pacgsfkngcrw50pnuvgvak0qpt8mx9pmey2uxl6p65c9727lhc0sgnklt4");
        }

        [TestMethod]
        public async Task GetSignagePoints()
        {
            var signagePoints = await _theFarmer.GetSignagePoints();

            Assert.IsNotNull(signagePoints);
        }

        [TestMethod]
        public async Task GetSignagePoint()
        {
            var signagePoint = await _theFarmer.GetSignagePoint("0x01a076953bef8eae24634a723d83593c23fa0a444fe0fc19138d49cdea440b6a");

            Assert.IsNotNull(signagePoint);
        }

        [TestMethod]
        public async Task GetHarvesters()
        {
            var harvesters = await _theFarmer.GetHarvesters();
            Assert.IsNotNull(harvesters);
        }

        [TestMethod]
        public async Task Ping()
        {
            await _theFarmer.Ping();
        }

        [TestMethod]
        public async Task GetConnections()
        {
            var connections = await _theFarmer.GetConnections();
            Assert.IsNotNull(connections);
        }

        [TestMethod]
        [Ignore] // only works on mainnet
        public async Task OpenConnection()
        {
            await _theFarmer.OpenConnection("node.chia.net", 8444);
        }
    }
}
