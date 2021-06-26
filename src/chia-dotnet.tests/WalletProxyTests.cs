using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Threading;
using System.Threading.Tasks;

namespace chia.dotnet.tests
{
    [TestClass]
    [TestCategory("Integration")]
    public class WalletProxyTests
    {
        private static Daemon _theDaemon;
        private static WalletProxy _theWallet;

        [ClassInitialize]
        public static async Task Initialize(TestContext context)
        {
            _theDaemon = new Daemon(Config.Open().GetEndpoint("ui"), "unit_tests");

            await _theDaemon.Connect(CancellationToken.None);
            await _theDaemon.Register(CancellationToken.None);
            _theWallet = new WalletProxy(_theDaemon);
        }

        [ClassCleanup()]
        public static void ClassCleanup()
        {
            _theDaemon?.Dispose();
        }

        [TestMethod()]
        public async Task GetWalletsTest()
        {
            var wallets = await _theWallet.GetWallets(CancellationToken.None);

            Assert.IsNotNull(wallets);
        }

        [TestMethod()]
        public async Task GetPublicKeys()
        {
            var keys = await _theWallet.GetPublicKeys(CancellationToken.None);

            Assert.IsNotNull(keys);
        }

        [TestMethod()]
        public async Task GetPrivateKey()
        {
            var key = await _theWallet.GetPrivateKey(2287630151, CancellationToken.None);

            Assert.IsNotNull(key);
        }

        [TestMethod()]
        public async Task GetWalletBalanceTest()
        {
            var balance = await _theWallet.GetWalletBalance(1, CancellationToken.None);

            Assert.IsNotNull(balance);
        }

        [TestMethod()]
        public async Task GetSyncStatusTest()
        {
            var info = await _theWallet.GetSyncStatus(CancellationToken.None);

            Assert.IsNotNull(info);
        }

        [TestMethod()]
        public async Task GetNetworkInfoTest()
        {
            var info = await _theWallet.GetNetworkInfo(CancellationToken.None);

            Assert.IsNotNull(info);
        }

        [TestMethod()]
        public async Task GetHeightInfoTest()
        {
            var height = await _theWallet.GetHeightInfo(CancellationToken.None);

            Assert.IsTrue(height > 0);
        }
    }
}
