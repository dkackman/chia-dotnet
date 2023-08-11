using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using chia.dotnet.tests.Core;
using Xunit;

namespace chia.dotnet.tests
{
    /// <summary>
    /// This class is a test harness for interation with an actual daemon instance
    /// </summary>
    public class DaemonTests : TestBase
    {
        public DaemonTests(ChiaDotNetFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task GetVersion()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);

            // Act
            var version = await Daemon.GetVersion(cts.Token);

            // Assert
            Assert.False(string.IsNullOrEmpty(version));
        }

        [Fact]
        public async Task IsRunning()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);

            // Act
            var running = await Daemon.IsRunning(ServiceNames.FullNode, cts.Token);

            // Assert
            Assert.True(running);
        }

        [Fact]
        public async Task GetKeyringStatus()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);

            // Act
            var status = await Daemon.GetKeyringStatus(cts.Token);

            // Assert
            Assert.NotNull(status);
        }

        [Fact]
        public async Task GetFarmerIsRunning()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);

            // Act
            var running = await Daemon.IsServiceRunning(ServiceNames.Farmer, cts.Token);

            // Assert
            Assert.True(running);
        }

        [Fact]
        public async Task ValidateInvalidKeyringPassphrase()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);

            // Act

            // Assert
            Assert.ThrowsAsync<ResponseException>(
                async () => await Daemon.ValidateKeyringPassphrase("spoon", cts.Token));
        }

        [Fact]
        public async Task UnlockKeyringInvalid()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);

            // Act

            // Assert
            Assert.ThrowsAsync<ResponseException>(async () => await Daemon.UnlockKeyring("spoon", cts.Token));
        }

        [Fact]
        public async Task UnlockKeyringValid()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            var status = await Daemon.GetKeyringStatus(cts.Token);
            if (status.UserPassphraseIsSet)
            {
                await Daemon.UnlockKeyring("sp00n3!!", cts.Token);

                var locked = await Daemon.IsKeyringLocked(cts.Token);

                Assert.False(locked);
            }
        }

        [Fact(Skip = "CAUTION")]
        public async Task MigrateKeyring()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);

            // Act
            var status = await Daemon.GetKeyringStatus(cts.Token);
            if (!status.UserPassphraseIsSet)
            {
                await Daemon.MigrateKeyring("sp00n3!!", "super secure utensil", true, false, cts.Token);
            }
        }

        [Fact]
        public async Task SetKeyringPassphrase()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            var status = await Daemon.GetKeyringStatus(cts.Token);

            // Act
            if (status.UserPassphraseIsSet)
            {
                await Daemon.SetKeyringPassphrase("sp00n3!!", "sp00n3!!!", "super duper secure utensil", true,
                    cts.Token);
            }
        }

        [Fact]
        public async Task RemoveKeyringPassphrase()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            var status = await Daemon.GetKeyringStatus(cts.Token);

            // Act
            if (status.UserPassphraseIsSet)
            {
                await Daemon.RemoveKeyringPassphrase("sp00n3!!!", cts.Token);
            }

            // Assert
        }

        [Fact]
        public async Task IsKeyringLocked()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);

            // Act
            var locked = await Daemon.IsKeyringLocked(cts.Token);

            // Assert
            Assert.True(locked);
        }
        
         // [Fact]
         // public async Task GetKeyForFingerprint()
         // {
         //     // Arrange
         //     using var cts = new CancellationTokenSource(15000);
         //
         //     var proxy = Daemon.CreateProxyFrom<WalletProxy>();
         //     var prints = await proxy.GetPublicKeys(cts.Token);
         //     Assert.True(prints.Any());
         //
         //     var key = await Daemon.GetKeyForFingerprint(prints.First(), cts.Token);
         //
         //     Assert.NotNull(key);
         // }

        [Fact]
        public async Task GetAllPrivateKeys()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);

            // Act
            var keys = await Daemon.GetAllPrivateKeys(cts.Token);

            // Assert
            Assert.NotNull(keys);
            Assert.True(keys.Any());
        }

//         [TestMethod]
//         [Ignore("This seems to put the daemon out to lunch")]
//         public async Task CheckKeys()
//         {
//             using var cts = new CancellationTokenSource(30000);
//
//             await _theDaemon.CheckKeys("~/.chia/mainnet/config", cts.Token);
//         }

        [Fact]
        public async Task GetFirstPrivateKey()
        {
            // Arrange
            using var cts = new CancellationTokenSource(30000);

            // Act
            var key = await Daemon.GetFirstPrivateKey(cts.Token);

            // Assert
            Assert.NotNull(key);
        }

//         [TestMethod]
//         public async Task CreateFullNodeFrom()
//         {
//             using var cts = new CancellationTokenSource(15000);
//
//             await _theDaemon.RegisterService(cts.Token);
//             var fullNode = _theDaemon.CreateProxyFrom<FullNodeProxy>();
//             var state = await fullNode.GetBlockchainState(cts.Token);
//             Assert.IsNotNull(state);
//         }
//
        [Fact]
        public async Task GetHarvesterIsRunning()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);

            // Act
            var running = await Daemon.IsServiceRunning(ServiceNames.Harvester, cts.Token);

            // Assert
            Assert.True(running);
        }

        [Fact(Skip = "CAUTION")]
        public async Task ExitDaemon()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);

            // Act
            await Daemon.Exit(cts.Token);

            // if no exception the daemon was stopped successfully
        }

        [Fact]
        public async Task RestartFarmer()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);

            // Act
            if (await Daemon.IsServiceRunning(ServiceNames.Farmer, cts.Token))
            {
                await Daemon.StopService(ServiceNames.Farmer, cts.Token);
                Assert.False(await Daemon.IsServiceRunning(ServiceNames.Farmer, cts.Token));
            }

            await Daemon.StartService(ServiceNames.Farmer, cts.Token);

            // Assert
            Assert.True(await Daemon.IsServiceRunning(ServiceNames.Farmer, cts.Token));
        }

        [Fact]
        public async Task RegisterService()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);

            // Act
            await Daemon.RegisterService("new_service", cts.Token);

            // Assert
            // no exception we were successful
        }
    }
}
