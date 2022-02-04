using System.Linq;
using System.Threading;
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
            using var cts = new CancellationTokenSource(15000);

            var rpcClient = Factory.CreateRpcClientFromHardcodedLocation();
            await rpcClient.Connect(cts.Token);

            var daemon = new DaemonProxy(rpcClient, "unit_tests");
            await daemon.RegisterService(cts.Token);

            var walletProxy = new WalletProxy(rpcClient, "unit_tests");
            _ = await walletProxy.LogIn(false, cts.Token);
            _theWallet = new Wallet(1, walletProxy);
        }

        [ClassCleanup()]
        public static void ClassCleanup()
        {
            _theWallet.WalletProxy.RpcClient?.Dispose();
        }

        [TestMethod()]
        public async Task GetWalletBalance()
        {
            using var cts = new CancellationTokenSource(15000);

            var balance = await _theWallet.GetBalance(cts.Token);

            Assert.IsNotNull(balance);
        }

        [TestMethod()]
        public async Task GetTransactions()
        {
            using var cts = new CancellationTokenSource(15000);
            var count = await _theWallet.GetTransactionCount(cts.Token);
            if (count == 0)
            {
                Assert.Inconclusive("no transactions");
            }
            else
            {
                var transactions = await _theWallet.GetTransactions(cts.Token);

                Assert.AreEqual((int)count, transactions.Count());
            }
        }

        [TestMethod()]
        public async Task GetTransactionsIncremental()
        {
            using var cts = new CancellationTokenSource(150000);

            var count = await _theWallet.GetTransactionCount(cts.Token);
            Assert.IsTrue(count > 4);

            var transactions1 = await _theWallet.GetTransactions(0, 2, cts.Token);
            var transactions2 = await _theWallet.GetTransactions(3, 5, cts.Token);

            Assert.IsNotNull(transactions1);
            Assert.AreEqual(transactions1.Count(), 2);

            Assert.IsNotNull(transactions2);
            Assert.AreEqual(transactions2.Count(), 2);

            var list1 = transactions1.ToList();
            var start1 = list1[0];
            var end1 = list1[1];

            var list2 = transactions2.ToList();
            var start2 = list2[0];
            var end2 = list2[1];

            Assert.AreNotEqual(start1.TransactionId, start2.TransactionId); //FAILS
            Assert.AreNotEqual(end1.TransactionId, end2.TransactionId);
        }

        [TestMethod()]
        public async Task GetWalletAddress()
        {
            using var cts = new CancellationTokenSource(15000);

            var address = await _theWallet.GetNextAddress(false, cts.Token);

            Assert.IsNotNull(address);
        }

        [TestMethod()]
        [TestCategory("CAUTION")]
        public async Task CreateNewWalletAddress()
        {
            using var cts = new CancellationTokenSource(15000);

            var address = await _theWallet.GetNextAddress(false, cts.Token);
            var newAddress = await _theWallet.GetNextAddress(true, cts.Token);

            Assert.AreNotEqual(address, newAddress);
        }

        [TestMethod()]
        public async Task GetTransactionCount()
        {
            using var cts = new CancellationTokenSource(15000);

            var count = await _theWallet.GetTransactionCount(cts.Token);

            Assert.IsNotNull(count);
        }

        [TestMethod()]
        [TestCategory("CAUTION")]
        public async Task DeleteUnconfirmedTransactions()
        {
            using var cts = new CancellationTokenSource(15000);

            await _theWallet.DeleteUnconfirmedTransactions(cts.Token);
        }

        [TestMethod()]
        [TestCategory("CAUTION")]
        public async Task SendTransaction()
        {
            using var cts = new CancellationTokenSource(15000);

            var transaction = await _theWallet.SendTransaction("txch1em43zsczg2fv79jlg00ucedl9x3atvpnfa09uuk5pgd7v9039sdsashhuq", 1, 1, cts.Token);

            Assert.IsNotNull(transaction);
        }


        [TestMethod()]
        public async Task ValidateTwo()
        {
            using var cts = new CancellationTokenSource(15000);

            var wallet = new PoolWallet(2, _theWallet.WalletProxy);
            await wallet.Validate();

        }
    }
}
