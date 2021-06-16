using System.Dynamic;
using System.Diagnostics;
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
    public class FarmerProxyIntergrationTests
    {
        private static Daemon _theDaemon;
        private static FarmerProxy _theFarmer;

        [ClassInitialize]
        public static async Task Initialize(TestContext context)
        {
            _theDaemon = new Daemon(Config.Open().GetEndpoint("ui"), "unit_tests");

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
            string s = targets.ToString();
            Debug.WriteLine(s);
        }

        [TestMethod]
        [TestCategory("CAUTION")]
        [Ignore]
        public async Task SetRewardTargets()
        {
            // this will change the state of the farmer - make sure you want to do this
            // fill in addresses for target and pool as appropriate
            await _theFarmer.SetRewardTargets("", "", CancellationToken.None);
        }

        [TestMethod]
        public async Task GetSignagePoints()
        {
            var signagePoints = await _theFarmer.GetSignagePoints(CancellationToken.None);

            Assert.IsNotNull(signagePoints);
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
