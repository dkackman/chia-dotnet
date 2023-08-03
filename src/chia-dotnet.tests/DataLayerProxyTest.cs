using System.Threading;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace chia.dotnet.tests
{
    [TestClass]
    [TestCategory("Integration")]
    public class DataLayerProxyTest
    {
        private static DataLayerProxy _theDataLayer;

        [ClassInitialize]
        public static async Task Initialize(TestContext context)
        {
            using var cts = new CancellationTokenSource(2000);
            var rpcClient = Factory.CreateWebsocketClient();
            await rpcClient.Connect(cts.Token);

            var daemon = new DaemonProxy(rpcClient, "unit_tests");
            await daemon.RegisterService(cts.Token);

            _theDataLayer = new DataLayerProxy(rpcClient, "unit_tests");
        }

        [ClassCleanup()]
        public static void ClassCleanup()
        {
            _theDataLayer.RpcClient?.Dispose();
        }

        [TestMethod()]
        [TestCategory("CAUTION")]
        public async Task CreateDataStore()
        {
            using var cts = new CancellationTokenSource(150000);

            var result = await _theDataLayer.CreateDataStore(5, cts.Token);

            Assert.IsNotNull(result);
        }
    }
}
