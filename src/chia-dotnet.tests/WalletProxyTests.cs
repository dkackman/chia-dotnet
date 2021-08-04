using System.Linq;
using System.Threading.Tasks;
using System.Numerics;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            _theDaemon = DaemonFactory.CreateDaemonFromHardcodedLocation(ServiceNames.Wallet);

            await _theDaemon.Connect();
            await _theDaemon.Register();
            _theWallet = new WalletProxy(_theDaemon);
        }

        [ClassCleanup()]
        public static void ClassCleanup()
        {
            _theDaemon?.Dispose();
        }

        [TestMethod()]
        public async Task GetWallets()
        {
            var wallets = await _theWallet.GetWallets();

            Assert.IsNotNull(wallets);
        }

        [TestMethod()]
        public async Task GetPublicKeys()
        {
            var keys = await _theWallet.GetPublicKeys();

            Assert.IsNotNull(keys);
        }

        [TestMethod()]
        public async Task Login()
        {
            var fingerprints = await _theWallet.GetPublicKeys();
            Assert.IsNotNull(fingerprints);
            Assert.IsTrue(fingerprints.Count() > 0);

            var fingerprint = await _theWallet.LogIn(fingerprints.First(), skipImport: true);

            Assert.IsFalse(fingerprint == 0);
        }

        [TestMethod()]
        public async Task GetPrivateKey()
        {
            var fingerprints = await _theWallet.GetPublicKeys();
            var key = await _theWallet.GetPrivateKey(fingerprints.First());

            Assert.IsNotNull(key);
        }

        [TestMethod()]
        public async Task GetSyncStatus()
        {
            var info = await _theWallet.GetSyncStatus();

            Assert.IsNotNull(info);
        }

        [TestMethod()]
        public async Task GetNetworkInfo()
        {
            var info = await _theWallet.GetNetworkInfo();

            Assert.IsNotNull(info);
        }

        [TestMethod()]
        public async Task GetHeightInfo()
        {
            var height = await _theWallet.GetHeightInfo();

            Assert.IsTrue(height > 0);
        }

        [TestMethod()]
        [TestCategory("CAUTION")]
        public async Task CreateBackup()
        {
            await _theWallet.CreateBackup(@"C:\tmp\b.bak");
        }

        [TestMethod()]
        public async Task GenerateMnemonic()
        {
            var mnemonic = await _theWallet.GenerateMnemonic();

            Assert.IsNotNull(mnemonic);
        }

        [TestMethod()]
        [TestCategory("CAUTION")]
        public async Task FullCircleKey()
        {
            var mnemonic = await _theWallet.GenerateMnemonic();
            var fingerprint = await _theWallet.AddKey(mnemonic, true);
            var key = await _theWallet.GetPrivateKey(fingerprint);
            Assert.IsNotNull(key);

            await _theWallet.DeleteKey(fingerprint);
        }

        [TestMethod()]
        [TestCategory("CAUTION")]
        public async Task CreateNewColourCoinWallet()
        {
            await LoginToFirstWallet();

            var walletInfo = await _theWallet.CreateColourCoinWallet(BigInteger.One, BigInteger.One, "dkackman.colouredwallet.1");

            Assert.IsNotNull(walletInfo);
        }

        [TestMethod()]
        [TestCategory("CAUTION")]
        public async Task CreateDIDWallet()
        {
            await LoginToFirstWallet();

            var backupIDs = new List<string>();

            var walletInfo = await _theWallet.CreateDIDWallet(backupIDs, BigInteger.One, 1);

            Assert.IsNotNull(walletInfo);
        }

        [TestMethod()]
        public async Task GetTransaction()
        {
            await LoginToFirstWallet();
            var transaction = await _theWallet.GetTransaction("0x03ba20de8cbaf42944277eef60ac716730721a1b253a606c5e9621541487b519");

            Assert.IsNotNull(transaction);
        }

        [TestMethod()]
        public async Task CreateOfferForIds()
        {
            var ids = new Dictionary<int, int>()
            {
                { 1, 1 }
            };

            await _theWallet.CreateOfferForIds(ids, @"C:\tmp\test.offer");
        }

        [TestMethod()]
        public async Task GetDiscrepenciesForOffer()
        {
            var discrepencies = await _theWallet.GetDiscrepenciesForOffer(@"C:\tmp\test.offer");

            Assert.IsNotNull(discrepencies);
        }

        [TestMethod()]
        public async Task GetFarmedAmount()
        {
            var amount = await _theWallet.GetFarmedAmount();

            Assert.IsNotNull(amount);
        }

        private async Task LoginToFirstWallet()
        {
            var fingerprints = await _theWallet.GetPublicKeys();

            _ = await _theWallet.LogIn(fingerprints.First(), skipImport: true);
        }
    }
}
