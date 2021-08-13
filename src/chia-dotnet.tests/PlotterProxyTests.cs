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
        private static PlotterProxy _thePlotter;

        [ClassInitialize]
        public static async Task Initialize(TestContext context)
        {
            using var cts = new CancellationTokenSource(15000);

            var rpcClient = Factory.CreateRpcClientFromHardcodedLocation();
            await rpcClient.Connect(cts.Token);

            var daemon = new DaemonProxy(rpcClient, "unit_tests");
            await daemon.RegisterService(cts.Token);

            _thePlotter = new PlotterProxy(rpcClient, "unit_tests");
        }

        [ClassCleanup()]
        public static void ClassCleanup()
        {
            _thePlotter.RpcClient?.Dispose();
        }

        [TestMethod()]
        [ExpectedException(typeof(InvalidOperationException))]
        public async Task FailOnInvalidConfig()
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));

            var config = new PlotterConfig();
            await _thePlotter.StartPlotting(config, cts.Token);
        }

        [TestMethod()]
        [TestCategory("CAUTION")]
        public async Task StartPlotting()
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));

            var config = new PlotterConfig()
            {
                Size = KValues.K25,
                OverrideK = true,
                TempDir = "/home/don/plots",
                DestinationDir = "/home/don/plots"
            };

            await _thePlotter.StartPlotting(config);
            var q = await _thePlotter.RegisterPlotter(cts.Token);

            Assert.IsNotNull(q);
        }

        [TestMethod()]
        public async Task RegisterPlotter()
        {
            using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));

            var q = await _thePlotter.RegisterPlotter(cts.Token);

            Assert.IsNotNull(q);
        }
    }
}
