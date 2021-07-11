using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace chia.dotnet.tests
{
    [TestClass]
    [TestCategory("Integration")]
    public class PlotterProxyTests
    {
        private static Daemon _theDaemon;
        private static PlotterProxy _thePlotter;

        [ClassInitialize]
        public static async Task Initialize(TestContext context)
        {
            _theDaemon = Factory.CreateDaemon();

            await _theDaemon.Connect(CancellationToken.None);
            await _theDaemon.Register(CancellationToken.None);
            _thePlotter = new PlotterProxy(_theDaemon);
        }

        [ClassCleanup()]
        public static void ClassCleanup()
        {
            _theDaemon?.Dispose();
        }

        [TestMethod()]
        [ExpectedException(typeof(TaskCanceledException))]
        public async Task FailOnInvaliddConfig()
        {
            var config = new PlotterConfig();
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));

            await _thePlotter.StartPlotting(config, cts.Token);
        }

        [TestMethod()]
        public async Task StartPlotting()
        {
            var config = new PlotterConfig()
            {
                Size = KValues.K25,
                OverrideK = true,
                TempDir = "/home/don/plots",
                DestinationDir = "/home/don/plots"
            };

            await _thePlotter.StartPlotting(config, CancellationToken.None);

            var q = await _theDaemon.RegisterPlotter(CancellationToken.None);

            Assert.IsNotNull(q);
        }
    }
}
