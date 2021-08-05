
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
    public class DaemonTests
    {
        private static DaemonProxy _theDaemon;

        [ClassInitialize]
        public static async Task Initialize(TestContext context)
        {
            var rpcClient = Factory.CreateRpcClientFromHardcodedLocation();
            await rpcClient.Connect();

            _theDaemon = new DaemonProxy(rpcClient, "unit_tests");
        }

        [ClassCleanup()]
        public static void ClassCleanup()
        {
            _theDaemon.RpcClient?.Dispose();
        }

        [TestMethod]
        public async Task GetFarmerIsRunning()
        {
            var running = await _theDaemon.IsServiceRunning(ServiceNames.Farmer);

            Assert.IsTrue(running);
        }

        [TestMethod]
        public async Task GetHarvesterIsRunning()
        {
            var running = await _theDaemon.IsServiceRunning(ServiceNames.Harvester);

            Assert.IsTrue(running);
        }

        [TestMethod]
        [Ignore]
        public async Task ExitDaemon()
        {
            await _theDaemon.Exit();

            // if no exception the daemon was stopped successfully
        }

        [TestMethod]
        public async Task RestartFarmer()
        {
            if (await _theDaemon.IsServiceRunning(ServiceNames.Farmer))
            {
                await _theDaemon.StopService(ServiceNames.Farmer);
                Assert.IsFalse(await _theDaemon.IsServiceRunning(ServiceNames.Farmer));
            }

            await _theDaemon.StartService(ServiceNames.Farmer);
            Assert.IsTrue(await _theDaemon.IsServiceRunning(ServiceNames.Farmer));
        }

        [TestMethod]
        public async Task RegisterService()
        {
            await _theDaemon.RegisterService("new_service");

            // no exception we were successful
        }
    }
}
