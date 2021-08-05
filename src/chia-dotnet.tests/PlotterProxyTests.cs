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
        private static DaemonProxy _theDaemon;       
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

            await _thePlotter.StartPlotting(config);

            // this seems like the only way to get the plot queue
            var q = await _thePlotter.RegisterPlotter();

            Assert.IsNotNull(q);
        }

        private static void _theDaemon_BroadcastMessageReceived(object sender, Message e)
        {

        }
    }
}
