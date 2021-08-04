using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace chia.dotnet.tests
{
    [TestClass]
    [TestCategory("Integration")]
    public class HarvesterProxyTests
    {
        private static Daemon _theDaemon;
        private static HarvesterProxy _theHarvester;

        [ClassInitialize]
        public static async Task Initialize(TestContext context)
        {
            _theDaemon = DaemonFactory.CreateDaemonFromHardcodedLocation(ServiceNames.Harvester);

            await _theDaemon.Connect();
            await _theDaemon.Register();
            _theHarvester = new HarvesterProxy(_theDaemon);
        }

        [ClassCleanup()]
        public static void ClassCleanup()
        {
            _theDaemon?.Dispose();
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
