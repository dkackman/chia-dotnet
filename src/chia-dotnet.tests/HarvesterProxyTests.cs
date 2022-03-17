using System.Threading;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace chia.dotnet.tests
{
    [TestClass]
    [TestCategory("Integration")]
    public class HarvesterProxyTests
    {
        private static HarvesterProxy _theHarvester;

        [ClassInitialize]
        public static async Task Initialize(TestContext context)
        {
            using var cts = new CancellationTokenSource(2000);
            var rpcClient = Factory.CreateDaemon();
            await rpcClient.Connect(cts.Token);

            var daemon = new DaemonProxy(rpcClient, "unit_tests");
            await daemon.RegisterService(cts.Token);

            _theHarvester = new HarvesterProxy(rpcClient, "unit_tests");
        }

        [ClassCleanup()]
        public static void ClassCleanup()
        {
            _theHarvester.RpcClient?.Dispose();
        }

        [TestMethod()]
        public async Task GetPlots()
        {
            using var cts = new CancellationTokenSource(15000);

            var plots = await _theHarvester.GetPlots(cts.Token);

            Assert.IsNotNull(plots);
        }

        [TestMethod()]
        [TestCategory("CAUTION")]
        [Ignore("CAUTION")]
        public async Task DeletePlot()
        {
            using var cts = new CancellationTokenSource(15000);

            await _theHarvester.DeletePlot("<plot name>", cts.Token);
        }

        [TestMethod()]
        public async Task GetPlotDirectories()
        {
            using var cts = new CancellationTokenSource(15000);

            var directories = await _theHarvester.GetPlotDirectories(cts.Token);

            Assert.IsNotNull(directories);
        }

        [TestMethod()]
        [TestCategory("CAUTION")]
        [Ignore("CAUTION")]
        public async Task AddPlotDirectory()
        {
            using var cts = new CancellationTokenSource(15000);

            await _theHarvester.AddPlotDirectory("/home/don/plots", cts.Token);
        }

        [TestMethod]
        public async Task Ping()
        {
            using var cts = new CancellationTokenSource(15000);

            await _theHarvester.Ping(cts.Token);
        }

        [TestMethod()]
        [TestCategory("CAUTION")]
        [Ignore("CAUTION")]
        public async Task RemovePlotDirectory()
        {
            using var cts = new CancellationTokenSource(15000);

            await _theHarvester.RemovePlotDirectory("/home/don/plots", cts.Token);
        }

        [TestMethod()]
        public async Task RefreshPlots()
        {
            using var cts = new CancellationTokenSource(15000);

            await _theHarvester.RefreshPlots(cts.Token);
        }
    }
}
