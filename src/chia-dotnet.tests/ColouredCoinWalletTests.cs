using System.Threading;
using System.Threading.Tasks;

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
            _theDaemon = DaemonFactory.CreateDaemonFromHardcodedLocation();

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
    }
}
