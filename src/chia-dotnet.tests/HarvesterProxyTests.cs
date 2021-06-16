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
            _theDaemon = new Daemon(Config.Open().GetEndpoint("ui"), "unit_tests");

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
        public async Task GetPlotsTest()
        {
            var plots = await _theHarvester.GetPlots(CancellationToken.None);

            Assert.IsNotNull(plots);
        }

        [TestMethod()]
        [TestCategory("CAUTION")]
        [Ignore]
        public async Task DeletePlotTest()
        {
            await _theHarvester.DeletePlot("<plot name", CancellationToken.None);
        }

        [TestMethod()]
        public async Task GetPlotDirectoriesTest()
        {
            var directories = await _theHarvester.GetPlotDirectories(CancellationToken.None);

            Assert.IsNotNull(directories);
        }

        [TestMethod()]
        [TestCategory("CAUTION")]
        [Ignore]
        public async Task AddPlotDirectoryTest()
        {
            await _theHarvester.AddPlotDirectory("/mnt/plots/", CancellationToken.None);
        }

        [TestMethod]
        public async Task PingTest()
        {
            await _theHarvester.Ping(CancellationToken.None);
        }

        [TestMethod()]
        [TestCategory("CAUTION")]
        [Ignore]
        public async Task RemovePlotDirectoryTest()
        {
            await _theHarvester.RemovePlotDirectory("/mnt/plots/", CancellationToken.None);
        }

        [TestMethod()]
        public async Task RefreshPlotsTest()
        {
            await _theHarvester.RefreshPlots(CancellationToken.None);
        }
    }
}
