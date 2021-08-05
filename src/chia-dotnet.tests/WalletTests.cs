using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace chia.dotnet.tests
{
    [TestClass]
    [TestCategory("Integration")]
    public class WalletTests
    {
        private static Wallet _theWallet;

        [ClassInitialize]
        public static async Task Initialize(TestContext context)
        {
            var rpcClient = Factory.CreateRpcClientFromHardcodedLocation();
            await rpcClient.Connect();

            var daemon = new DaemonProxy(rpcClient, "unit_tests");            
            await daemon.RegisterService();

            var walletProxy = new WalletProxy(rpcClient, "unit_tests");

            _theWallet = new Wallet(1, walletProxy);
            _ = await _theWallet.Login();
        }

        [ClassCleanup()]
        public static void ClassCleanup()
        {
            _theWallet.WalletProxy.RpcClient?.Dispose();
        }

        [TestMethod()]
        public async Task GetWalletBalance()
        {
            var balance = await _theWallet.GetBalance();

            Assert.IsNotNull(balance);
        }

        [TestMethod()]
        public async Task GetTransactions()
        {
            var transactions = await _theWallet.GetTransactions();

            Assert.IsNotNull(transactions);
        }

        [TestMethod()]
        public async Task GetWalletAddress()
        {
            var address = await _theWallet.GetNextAddress(false);

            Assert.IsNotNull(address);
        }

        [TestMethod()]
        [TestCategory("CAUTION")]
        public async Task CreateNewWalletAddress()
        {
            var address = await _theWallet.GetNextAddress(false);
            var newAddress = await _theWallet.GetNextAddress(true);

            Assert.AreNotEqual(address, newAddress);
        }

        [TestMethod()]
        public async Task GetTransactionCount()
        {
            var count = await _theWallet.GetTransactionCount();

            Assert.IsNotNull(count);
        }

        [TestMethod()]
        [TestCategory("CAUTION")]
        public async Task DeleteUnconfirmedTransactions()
        {
            await _theWallet.DeleteUnconfirmedTransactions();
        }

        [TestMethod()]
        [TestCategory("CAUTION")]
        public async Task SendTransaction()
        {
            var transaction = await _theWallet.SendTransaction("txch1em43zsczg2fv79jlg00ucedl9x3atvpnfa09uuk5pgd7v9039sdsashhuq", 1, 1);

            Assert.IsNotNull(transaction);
        }
    }
}
