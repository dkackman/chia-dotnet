using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace chia.dotnet.tests
{
    [TestClass]
    [TestCategory("Integration")]
    public class ColouredCoinWalletTests
    {
        private static Daemon _theDaemon;
        private static ColouredCoinWallet _theWallet;

        [ClassInitialize]
        public static async Task Initialize(TestContext context)
        {
            _theDaemon = Factory.CreateDaemon();

            await _theDaemon.Connect(CancellationToken.None);
            await _theDaemon.Register(CancellationToken.None);

            // SET this wallet ID to a coloroured coin wallet 
            _theWallet = new ColouredCoinWallet(2, new WalletProxy(_theDaemon));
            _ = await _theWallet.Login(CancellationToken.None);
        }

        [ClassCleanup()]
        public static void ClassCleanup()
        {
            _theDaemon?.Dispose();
        }

        [TestMethod()]
        public async Task GetColouredCoinName()
        {
            var name = await _theWallet.GetName(CancellationToken.None);

            Assert.IsNotNull(name);
        }

        [TestMethod()]
        public async Task GetColouredCoinColour()
        {
            var colour = await _theWallet.GetColour(CancellationToken.None);

            Assert.IsNotNull(colour);
        }

        [TestMethod()]
        public async Task CreateOfferForIds()
        {
            var ids = new Dictionary<int, int>()
            {
                { 1, 1 }
            };

            await _theWallet.CreateOfferForIds(ids, @"C:\tmp\test.offer", CancellationToken.None);
        }

        [TestMethod()]
        public async Task GetDiscrepenciesForOffer()
        {
            var discrepencies = await _theWallet.GetDiscrepenciesForOffer(@"C:\tmp\test.offer", CancellationToken.None);

            Assert.IsNotNull(discrepencies);
        }
    }
}
