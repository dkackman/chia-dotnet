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
            var rpcClient = Factory.CreateRpcClientFromHardcodedLocation();
            await rpcClient.Connect();

            var daemon = new DaemonProxy(rpcClient, "unit_tests");            
            await daemon.RegisterService();

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
            var plots = await _theHarvester.GetPlots();

            Assert.IsNotNull(plots);
        }

        [TestMethod()]
        [TestCategory("CAUTION")]
        public async Task DeletePlot()
        {
            await _theHarvester.DeletePlot("<plot name>");
        }

        [TestMethod()]
        public async Task GetPlotDirectories()
        {
            var directories = await _theHarvester.GetPlotDirectories();

            Assert.IsNotNull(directories);
        }

        [TestMethod()]
        [TestCategory("CAUTION")]
        public async Task AddPlotDirectory()
        {
            await _theHarvester.AddPlotDirectory("/home/don/plots");
        }

        [TestMethod]
        public async Task Ping()
        {
            await _theHarvester.Ping();
        }

        [TestMethod()]
        [TestCategory("CAUTION")]
        public async Task RemovePlotDirectory()
        {
            await _theHarvester.RemovePlotDirectory("/home/don/plots");
        }

        [TestMethod()]
        public async Task RefreshPlots()
        {
            await _theHarvester.RefreshPlots();
        }
    }
}
