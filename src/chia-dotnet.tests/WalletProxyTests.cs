using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace chia.dotnet.tests
{
    [TestClass]
    [TestCategory("Integration")]
    public class WalletProxyTests
    {
        private static WalletProxy _theWallet;

        [ClassInitialize]
        public static async Task Initialize(TestContext context)
        {
            using var cts = new CancellationTokenSource(15000);

            var rpcClient = Factory.CreateRpcClientFromHardcodedLocation();
            await rpcClient.Connect(cts.Token);

            var daemon = new DaemonProxy(rpcClient, "unit_tests");
            await daemon.RegisterService(cts.Token);

            _theWallet = new WalletProxy(rpcClient, "unit_tests");
        }

        [ClassCleanup()]
        public static void ClassCleanup()
        {
            _theWallet.RpcClient.Dispose();
        }

        [TestMethod()]
        public async Task GetWallets()
        {
            using var cts = new CancellationTokenSource(15000);

            var wallets = await _theWallet.GetWallets(cts.Token);

            Assert.IsNotNull(wallets);
        }

        [TestMethod()]
        public async Task GetPublicKeys()
        {
            using var cts = new CancellationTokenSource(15000);

            var keys = await _theWallet.GetPublicKeys(cts.Token);

            Assert.IsNotNull(keys);
        }

        [TestMethod()]
        public async Task Login()
        {
            using var cts = new CancellationTokenSource(15000);

            var fingerprints = await _theWallet.GetPublicKeys(cts.Token);
            Assert.IsNotNull(fingerprints);
            Assert.IsTrue(fingerprints.Count() > 0);

            var fingerprint = await _theWallet.LogIn(fingerprints.First(), true, cts.Token);

            Assert.IsFalse(fingerprint == 0);
        }

        [TestMethod()]
        public async Task GetPrivateKey()
        {
            using var cts = new CancellationTokenSource(15000);

            var fingerprints = await _theWallet.GetPublicKeys(cts.Token);
            var key = await _theWallet.GetPrivateKey(fingerprints.First(), cts.Token);

            Assert.IsNotNull(key);
        }

        [TestMethod()]
        public async Task GetSyncStatus()
        {
            using var cts = new CancellationTokenSource(15000);

            var info = await _theWallet.GetSyncStatus(cts.Token);

            Assert.IsNotNull(info);
        }

        [TestMethod()]
        public async Task GetNetworkInfo()
        {
            using var cts = new CancellationTokenSource(15000);

            var info = await _theWallet.GetNetworkInfo(cts.Token);

            Assert.IsNotNull(info);
        }

        [TestMethod()]
        public async Task GetAllTrades()
        {
            using var cts = new CancellationTokenSource(15000);

            var trades = await _theWallet.GetAllTrades(cts.Token);

            Assert.IsNotNull(trades);
        }

        [TestMethod()]
        public async Task GetHeightInfo()
        {
            using var cts = new CancellationTokenSource(15000);

            var height = await _theWallet.GetHeightInfo(cts.Token);

            Assert.IsTrue(height > 0);
        }

        [TestMethod()]
        [TestCategory("CAUTION")]
        public async Task CreateBackup()
        {
            using var cts = new CancellationTokenSource(15000);

            await _theWallet.CreateBackup(@"C:\tmp\b.bak", cts.Token);
        }

        [TestMethod()]
        public async Task GenerateMnemonic()
        {
            using var cts = new CancellationTokenSource(15000);

            var mnemonic = await _theWallet.GenerateMnemonic(cts.Token);

            Assert.IsNotNull(mnemonic);
        }

        [TestMethod()]
        [TestCategory("CAUTION")]
        [Ignore("CAUTION")]
        public async Task FullCircleKey()
        {
            using var cts = new CancellationTokenSource(15000);

            var mnemonic = await _theWallet.GenerateMnemonic(cts.Token);
            var fingerprint = await _theWallet.AddKey(mnemonic, true, cts.Token);
            var key = await _theWallet.GetPrivateKey(fingerprint, cts.Token);
            Assert.IsNotNull(key);

            await _theWallet.DeleteKey(fingerprint, cts.Token);
        }

        [TestMethod()]
        [TestCategory("CAUTION")]
        [Ignore("CAUTION")]
        public async Task CreateNewColourCoinWallet()
        {
            using var cts = new CancellationTokenSource(15000);

            await LoginToFirstWallet();
            var walletInfo = await _theWallet.CreateColourCoinWallet(1, 1, "dkackman.colouredwallet.1", cts.Token);

            Assert.IsNotNull(walletInfo);
        }

        [TestMethod()]
        [TestCategory("CAUTION")]
        [Ignore("CAUTION")]
        public async Task CreateDIDWallet()
        {
            using var cts = new CancellationTokenSource(15000);

            await LoginToFirstWallet();
            var backupIDs = new List<string>();
            var walletInfo = await _theWallet.CreateDIDWallet(backupIDs, 1, 1, cts.Token);

            Assert.IsNotNull(walletInfo);
        }

        [TestMethod()]
        public async Task GetTransaction()
        {
            using var cts = new CancellationTokenSource(150000);

            var wallet = new Wallet(1, _theWallet);
            _ = await wallet.Login(cts.Token);

            var transactions = await wallet.GetTransactions(cts.Token);
            var transaction1 = transactions.FirstOrDefault();
            Assert.IsNotNull(transaction1);

            var transaction2 = await _theWallet.GetTransaction(transaction1.TransactionId, cts.Token);
            Assert.IsNotNull(transaction2);

            Assert.AreEqual(transaction1.TransactionId, transaction2.TransactionId);
        }

        [TestMethod()]
        [Ignore("CAUTION")]
        public async Task CreateOfferForIds()
        {
            using var cts = new CancellationTokenSource(15000);

            var ids = new Dictionary<int, int>()
            {
                { 1, 1 }
            };
            await _theWallet.CreateOfferForIds(ids, @"C:\tmp\test.offer", cts.Token);
        }

        [TestMethod()]
        [Ignore("CAUTION")]
        public async Task GetDiscrepenciesForOffer()
        {
            using var cts = new CancellationTokenSource(15000);

            var discrepencies = await _theWallet.GetDiscrepenciesForOffer(@"C:\tmp\test.offer", cts.Token);

            Assert.IsNotNull(discrepencies);
        }

        [TestMethod()]
        public async Task GetFarmedAmount()
        {
            using var cts = new CancellationTokenSource(15000);

            var amount = await _theWallet.GetFarmedAmount(cts.Token);

            Assert.IsNotNull(amount);
        }

        private async Task LoginToFirstWallet()
        {
            using var cts = new CancellationTokenSource(15000);

            var fingerprints = await _theWallet.GetPublicKeys();

            _ = await _theWallet.LogIn(fingerprints.First(), true, cts.Token);
        }
    }
}
