using System.Threading;
using System.Threading.Tasks;
using System.Numerics;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace chia.dotnet.tests
{
    [TestClass]
    [TestCategory("Integration")]
    public class WalletTests
    {
        private static Daemon _theDaemon;
        private static Wallet _theWallet;

        [ClassInitialize]
        public static async Task Initialize(TestContext context)
        {
            _theDaemon = Factory.CreateDaemon();

            await _theDaemon.Connect(CancellationToken.None);
            await _theDaemon.Register(CancellationToken.None);

            _theWallet = new Wallet(1, new WalletProxy(_theDaemon));
            _ = await _theWallet.Login(CancellationToken.None);
        }

        [ClassCleanup()]
        public static void ClassCleanup()
        {
            _theDaemon?.Dispose();
        }

        [TestMethod()]
        public async Task GetWalletBalance()
        {
            var balance = await _theWallet.GetBalance(CancellationToken.None);

            Assert.IsNotNull(balance);
        }

        [TestMethod()]
        public async Task GetTransactions()
        {
            var transactions = await _theWallet.GetTransactions(CancellationToken.None);

            Assert.IsNotNull(transactions);
        }

        [TestMethod()]
        public async Task GetNextAddress()
        {
            var address = await _theWallet.GetNextAddress(true, CancellationToken.None);

            Assert.IsNotNull(address);
        }

        [TestMethod()]
        public async Task GetFarmedAmount()
        {
            var amount = await _theWallet.GetFarmedAmount(CancellationToken.None);

            Assert.IsNotNull(amount);
        }

        [TestMethod()]
        public async Task GetTransactionCount()
        {
            var count = await _theWallet.GetTransactionCount(CancellationToken.None);

            Assert.IsNotNull(count);
        }

        [TestMethod()]
        [TestCategory("CAUTION")]
        public async Task DeleteUnconfirmedTransactions()
        {
            await _theWallet.DeleteUnconfirmedTransactions(CancellationToken.None);
        }

        [TestMethod()]
        [TestCategory("CAUTION")]
        public async Task SendTransaction()
        {
            var transaction = await _theWallet.SendTransaction("txch1em43zsczg2fv79jlg00ucedl9x3atvpnfa09uuk5pgd7v9039sdsashhuq", BigInteger.One, BigInteger.One, CancellationToken.None);

            Assert.IsNotNull(transaction);
        }
    }
}
