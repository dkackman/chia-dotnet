using System.Threading;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace chia.dotnet.tests
{
    [TestClass]
    [TestCategory("Integration")]
    public class DIDWalletTests
    {
        private static Daemon _theDaemon;
        private static DIDWallet _theWallet;

        [ClassInitialize]
        public static async Task Initialize(TestContext context)
        {
            _theDaemon = Factory.CreateDaemon();

            await _theDaemon.Connect(CancellationToken.None);
            await _theDaemon.Register(CancellationToken.None);

            // SET this wallet ID to a coloroured coin wallet 
            _theWallet = new DIDWallet(2, new WalletProxy(_theDaemon));
            _ = await _theWallet.Login(CancellationToken.None);
        }

        [ClassCleanup()]
        public static void ClassCleanup()
        {
            _theDaemon?.Dispose();
        }

        [TestMethod()]
        public async Task GetPubKey()
        {
            var pubkey = await _theWallet.GetPubKey(CancellationToken.None);

            Assert.IsNotNull(pubkey);
        }

    }
}
