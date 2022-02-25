using System;
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
            using var cts = new CancellationTokenSource();

            var rpcClient = Factory.CreateRpcClientFromHardcodedLocation();
            await rpcClient.Connect(cts.Token);

            var daemon = new DaemonProxy(rpcClient, "unit_tests");
            await daemon.RegisterService(cts.Token);

            _theWallet = new WalletProxy(rpcClient, "unit_tests");
            _ = await _theWallet.LogIn(1531304830, cts.Token);
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
        public async Task GetPrivateKey()
        {
            using var cts = new CancellationTokenSource(15000);

            var key = await _theWallet.GetPrivateKey(1531304830, cts.Token);

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

            var walletInfo = await _theWallet.CreateColourCoinWallet(1, 1, "dkackman.colouredwallet.1", cts.Token);

            Assert.IsNotNull(walletInfo);
        }

        [TestMethod()]
        [TestCategory("CAUTION")]
        [Ignore("CAUTION")]
        public async Task CreateDIDWallet()
        {
            using var cts = new CancellationTokenSource(15000);

            var backupIDs = new List<string>();
            var walletInfo = await _theWallet.CreateDIDWallet(backupIDs, 1, 1, cts.Token);

            Assert.IsNotNull(walletInfo);
        }
        [TestMethod()]
        public async Task Login()
        {
            using var cts = new CancellationTokenSource(150000);

            _ = await _theWallet.LogIn(cts.Token);
        }

        [TestMethod()]
        public async Task GetTransaction()
        {
            using var cts = new CancellationTokenSource(150000);

            _ = await _theWallet.LogIn(cts.Token);
            var wallet = new Wallet(1, _theWallet);

            var transactions = await wallet.GetTransactions(cts.Token);
            var transaction1 = transactions.FirstOrDefault();
            Assert.IsNotNull(transaction1);

            var transaction2 = await _theWallet.GetTransaction(transaction1.TransactionId, cts.Token);
            Assert.IsNotNull(transaction2);

            Assert.AreEqual(transaction1.TransactionId, transaction2.TransactionId);
        }

        [TestMethod]
        [TestCategory("Integration")]
        public async Task LetsJoinAPool()
        {
            var poolUri = new Uri("https://testpool.xchpool.org");
            using var cts1 = new CancellationTokenSource(30000);
            var poolInfo = await WalletProxy.GetPoolInfo(poolUri, cts1.Token);

            var poolState = new PoolState()
            {
                PoolUrl = poolUri.ToString(),
                State = PoolSingletonState.FARMING_TO_POOL,
                TargetPuzzleHash = poolInfo.TargetPuzzleHash[2..],
                RelativeLockHeight = poolInfo.RelativeLockHeight
            };

            using var cts = new CancellationTokenSource(30000);

            var (transaction, launcherId, p2SingletonHash) = await _theWallet.CreatePoolWallet(poolState, null, null, cts.Token);
            Console.WriteLine($"Launcher Id: {launcherId}");
            Console.WriteLine($"Do rchia wallet get-transaction -tx {transaction.Name} to get status");
        }

        [TestMethod]
        [TestCategory("Integration")]
        public async Task GetPoolInfo()
        {
            using var cts = new CancellationTokenSource(15000);

            var info = await WalletProxy.GetPoolInfo(new Uri("https://testpool.xchpool.org"), cts.Token);

            Assert.IsNotNull(info);
        }

        [TestMethod()]
        public async Task GetTrade()
        {
            using var cts = new CancellationTokenSource(15000);

            var wallet = new Wallet(1, _theWallet);

            var transactions = await wallet.GetTransactions(cts.Token);
            var transaction1 = transactions.FirstOrDefault(tx => tx.TradeId is not null);
            if (transaction1 is null)
            {
                Assert.Inconclusive("This wallet has no trades");
            }
            else
            {
                var trade = await _theWallet.GetTrade(transaction1.TradeId, cts.Token);
                Assert.IsNotNull(trade);
            }
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
    }
}
