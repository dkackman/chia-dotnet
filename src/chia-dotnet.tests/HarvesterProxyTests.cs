using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Threading;
using System.Threading.Tasks;

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
            _theDaemon = Factory.CreateDaemon();

            await _theDaemon.Connect(CancellationToken.None);
            await _theDaemon.Register(CancellationToken.None);
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
            var plots = await _theHarvester.GetPlots(CancellationToken.None);

            Assert.IsNotNull(plots);
        }

        [TestMethod()]
        [TestCategory("CAUTION")]
        public async Task DeletePlot()
        {
            await _theHarvester.DeletePlot("<plot name>", CancellationToken.None);
        }

        [TestMethod()]
        public async Task GetPlotDirectories()
        {
            var directories = await _theHarvester.GetPlotDirectories(CancellationToken.None);

            Assert.IsNotNull(directories);
        }

        [TestMethod()]
        [TestCategory("CAUTION")]
        public async Task AddPlotDirectory()
        {
            await _theHarvester.AddPlotDirectory("/home/don/plots", CancellationToken.None);
        }

        [TestMethod]
        public async Task Ping()
        {
            await _theHarvester.Ping(CancellationToken.None);
        }

        [TestMethod()]
        [TestCategory("CAUTION")]
        public async Task RemovePlotDirectory()
        {
            await _theHarvester.RemovePlotDirectory("/home/don/plots", CancellationToken.None);
        }

        [TestMethod()]
        public async Task RefreshPlots()
        {
            await _theHarvester.RefreshPlots(CancellationToken.None);
        }
    }
}
