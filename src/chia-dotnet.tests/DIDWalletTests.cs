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
            var rpcClient = Factory.CreateRpcClientFromHardcodedLocation();
            await rpcClient.Connect();

            var daemon = new DaemonProxy(rpcClient, "unit_tests");            
            await daemon.RegisterService();

            var walletProxy = new WalletProxy(rpcClient, "unit_tests");

            // SET this wallet ID to a coloroured coin wallet 
            _theWallet = new DIDWallet(2, walletProxy);
            _ = await _theWallet.Login();
        }

        [ClassCleanup()]
        public static void ClassCleanup()
        {
            _theWallet.WalletProxy.RpcClient?.Dispose();
        }

        [TestMethod()]
        public async Task GetPubKey()
        {
            var pubkey = await _theWallet.GetPubKey();

            Assert.IsNotNull(pubkey);
        }

    }
}
