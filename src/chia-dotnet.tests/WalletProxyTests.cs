using System.Threading;
using System.Threading.Tasks;
using chia.dotnet.tests.Core;
using Xunit;

namespace chia.dotnet.tests
{
    public class WalletProxyTests : TestBase
    {
        public WalletProxyTests(ChiaDotNetFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task GetWallets()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);

            // Act
            var wallets = await Wallet.GetWallets(false, cts.Token);

            // Assert
            Assert.NotNull(wallets);
        }


        // [Fact]

        // public async Task GetLoggedInFingerprint()
        // {
        //     // Arrange
        //     using var cts = new CancellationTokenSource(15000);
        //
        //     // Act
        //     var fingerprint = await Wallet.GetLoggedInFingerprint(cts.Token);
        //
        //     // Assert
        //     Assert.Equal(Wallet.Fingerprint.Value, fingerprint);
        // }

        [Fact]
        public async Task GetPublicKeys()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);

            // Act
            var keys = await Wallet.GetPublicKeys(cts.Token);

            // Assert
            Assert.NotNull(keys);
        }

        // [Fact]
        // public async Task GetPrivateKey()
        // {
        //     // Arrange
        //     using var cts = new CancellationTokenSource(15000);
        //
        //     // Act
        //     var key = await Wallet.GetPrivateKey(Wallet.Fingerprint.Value, cts.Token);
        //
        //     // Assert
        //     Assert.NotNull(key);
        // }

        [Fact]
        public async Task GetSyncStatus()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);

            // Act
            var info = await Wallet.GetSyncStatus(cts.Token);

            // Assert
            Assert.NotNull(info);
        }

        [Fact]
        public async Task GetNetworkInfo()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);

            // Act
            var info = await Wallet.GetNetworkInfo(cts.Token);

            // Assert
            Assert.NotNull(info);
        }

        [Fact]
        public async Task GetHeightInfo()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);

            // Act
            var height = await Wallet.GetHeightInfo(cts.Token);

            // Assert
            Assert.True(height > 0);
        }

        [Fact]
        public async Task GenerateMnemonic()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);

            // Act
            var mnemonic = await Wallet.GenerateMnemonic(cts.Token);

            // Assert
            Assert.NotNull(mnemonic);
        }
        //
        //          [Fact]
        //         [TestCategory("CAUTION")]
        //         [Ignore("CAUTION")]
        //         public async Task FullCircleKey()
        //         {
        //             using var cts = new CancellationTokenSource(15000);
        //
        //             var mnemonic = await _theWallet.GenerateMnemonic(cts.Token);
        //             var fingerprint = await Wallet.AddKey(mnemonic, cts.Token);
        //             var key = await Wallet.GetPrivateKey(fingerprint, cts.Token);
        //             Assert.IsNotNull(key);
        //
        //             await Wallet.DeleteKey(fingerprint, cts.Token);
        //         }
        //
        //         [Fact]
        //         [TestCategory("CAUTION")]
        //         [Ignore("CAUTION")]
        //         public async Task CreateCATWallet()
        //         {
        //             using var cts = new CancellationTokenSource(20000);
        //
        //             var walletInfo = await Wallet.CreateCATWallet("dkackman.cat.1", 1, 1, cts.Token);
        //
        //             Assert.IsNotNull(walletInfo);
        //         }
        //
        //          [Fact]
        //         [TestCategory("CAUTION")]
        //         [Ignore("CAUTION")]
        //         public async Task CreateDIDWallet()
        //         {
        //             using var cts = new CancellationTokenSource(15000);
        //
        //             var backupIDs = new List<string>();
        //             var walletInfo = await _theWallet.CreateDIDWallet(backupIDs, 1, "the_name", null, 0, cts.Token);
        //
        //             Assert.IsNotNull(walletInfo);
        //         }
        //          [Fact]
        //         public async Task Login()
        //         {
        //             using var cts = new CancellationTokenSource(150000);
        //
        //             _ = await Wallet.LogIn(cts.Token);
        //         }
        //
        //          [Fact]
        //         public async Task GetTransaction()
        //         {
        //             using var cts = new CancellationTokenSource(150000);
        //
        //             _ = await _theWallet.LogIn(cts.Token);
        //             var wallet = new Wallet(1, _theWallet);
        //
        //             var transactions = await wallet.GetTransactions(cancellationToken: cts.Token);
        //             var transaction1 = transactions.FirstOrDefault();
        //             Assert.IsNotNull(transaction1);
        //
        //             var transaction2 = await _theWallet.GetTransaction(transaction1.TransactionId, cts.Token);
        //             Assert.IsNotNull(transaction2);
        //
        //             Assert.AreEqual(transaction1.TransactionId, transaction2.TransactionId);
        //         }
        //
        //          [Fact]
        //         [TestCategory("Integration")]
        //         public async Task LetsJoinAPool()
        //         {
        //             var poolUri = new Uri("https://testpool.xchpool.org");
        //             using var cts1 = new CancellationTokenSource(30000);
        //             var poolInfo = await WalletProxy.GetPoolInfo(poolUri, cts1.Token);
        //
        //             var poolState = new PoolState()
        //             {
        //                 PoolUrl = poolUri.ToString(),
        //                 State = PoolSingletonState.FARMING_TO_POOL,
        //                 TargetPuzzleHash = poolInfo.TargetPuzzleHash[2..],
        //                 RelativeLockHeight = poolInfo.RelativeLockHeight
        //             };
        //
        //             using var cts = new CancellationTokenSource(30000);
        //
        //             var (transaction, launcherId, p2SingletonHash) = await _theWallet.CreatePoolWallet(poolState, null, null, cts.Token);
        //             Console.WriteLine($"Launcher Id: {launcherId}");
        //             Console.WriteLine($"Do rchia wallet get-transaction -tx {transaction.Name} to get status");
        //         }
        //
        //          [Fact]
        //         [TestCategory("Integration")]
        //         public async Task GetPoolInfo()
        //         {
        //             using var cts = new CancellationTokenSource(15000);
        //
        //             var info = await WalletProxy.GetPoolInfo(new Uri("https://testpool.xchpool.org"), cts.Token);
        //
        //             Assert.IsNotNull(info);
        //         }
        //
        //          [Fact]
        //         public async Task GetFarmedAmount()
        //         {
        //             using var cts = new CancellationTokenSource(15000);
        //
        //             var amount = await Wallet.GetFarmedAmount(cts.Token);
        //
        //             Assert.IsNotNull(amount);
        //         }
    }
}
