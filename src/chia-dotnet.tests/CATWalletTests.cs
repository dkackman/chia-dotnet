using System.Threading;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace chia.dotnet.tests
{
    [TestClass]
    [TestCategory("Integration")]
    //[Ignore("Needs a CAT wallet")]
    public class CATWalletTests
    {
        private static CATWallet _theWallet;

        [ClassInitialize]
        public static async Task Initialize(TestContext context)
        {
            using var cts = new CancellationTokenSource(30000);
            var rpcClient = Factory.CreateDaemon();
            await rpcClient.Connect(cts.Token);

            var daemon = new DaemonProxy(rpcClient, "unit_tests");
            await daemon.RegisterService();

            var walletProxy = new WalletProxy(rpcClient, "unit_tests");

            _ = await walletProxy.LogIn(cts.Token);
            // SET this wallet ID to a coloroured coin wallet 
            _theWallet = new CATWallet(2, walletProxy);
        }

        [ClassCleanup()]
        public static void ClassCleanup()
        {
            _theWallet.WalletProxy.RpcClient?.Dispose();
        }

        [TestMethod()]
        public async Task GetName()
        {
            var name = await _theWallet.GetName();

            Assert.IsNotNull(name);
        }
    }
}
