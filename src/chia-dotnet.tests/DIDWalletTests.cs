using System.Threading;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace chia.dotnet.tests
{
    [TestClass]
    [TestCategory("Integration")]
    [Ignore("Needs DID Wallet")]
    public class DIDWalletTests
    {
        private static DIDWallet _theWallet;

        [ClassInitialize]
        public static async Task Initialize(TestContext context)
        {
            using var cts = new CancellationTokenSource(30000);
            var rpcClient = Factory.CreateDaemon();
            await rpcClient.Connect(cts.Token);

            var daemon = new DaemonProxy(rpcClient, "unit_tests");
            await daemon.RegisterService(cts.Token);

            var walletProxy = new WalletProxy(rpcClient, "unit_tests");
            _ = await walletProxy.LogIn(cts.Token);
            // SET this wallet ID to a coloroured coin wallet 
            _theWallet = new DIDWallet(2, walletProxy);
        }

        [ClassCleanup()]
        public static void ClassCleanup()
        {
            _theWallet.WalletProxy.RpcClient?.Dispose();
        }

        [TestMethod()]
        public async Task GetPubKey()
        {
            using var cts = new CancellationTokenSource(15000);

            var pubkey = await _theWallet.GetPubKey(cts.Token);

            Assert.IsNotNull(pubkey);
        }

    }
}
