
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
    public class DaemonTests
    {
        private static DaemonProxy _theDaemon;

        [ClassInitialize]
        public static async Task Initialize(TestContext context)
        {
            using var cts = new CancellationTokenSource(2000);
            var rpcClient = Factory.CreateRpcClientFromHardcodedLocation();
            await rpcClient.Connect(cts.Token);

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
            using var cts = new CancellationTokenSource(15000);

            var running = await _theDaemon.IsServiceRunning(ServiceNames.Farmer, cts.Token);

            Assert.IsTrue(running);
        }


        [TestMethod]
        public async Task CreateFullNodeFrom()
        {
            using var cts = new CancellationTokenSource(15000);

            await _theDaemon.RegisterService(cts.Token);
            var fullNode = _theDaemon.CreateProxyFrom<FullNodeProxy>();
            var state = await fullNode.GetBlockchainState(cts.Token);
            Assert.IsNotNull(state);
        }

        [TestMethod]
        public async Task GetHarvesterIsRunning()
        {
            using var cts = new CancellationTokenSource(15000);

            var running = await _theDaemon.IsServiceRunning(ServiceNames.Harvester, cts.Token);

            Assert.IsTrue(running);
        }

        [TestMethod]
        [Ignore("CAUTION")]
        public async Task ExitDaemon()
        {
            using var cts = new CancellationTokenSource(15000);

            await _theDaemon.Exit(cts.Token);

            // if no exception the daemon was stopped successfully
        }

        [TestMethod]
        public async Task RestartFarmer()
        {
            using var cts = new CancellationTokenSource(15000);

            if (await _theDaemon.IsServiceRunning(ServiceNames.Farmer, cts.Token))
            {
                await _theDaemon.StopService(ServiceNames.Farmer, cts.Token);
                Assert.IsFalse(await _theDaemon.IsServiceRunning(ServiceNames.Farmer, cts.Token));
            }

            await _theDaemon.StartService(ServiceNames.Farmer, cts.Token);
            Assert.IsTrue(await _theDaemon.IsServiceRunning(ServiceNames.Farmer, cts.Token));
        }

        [TestMethod]
        public async Task RegisterService()
        {
            using var cts = new CancellationTokenSource(15000);

            await _theDaemon.RegisterService("new_service", cts.Token);

            // no exception we were successful
        }
    }
}
