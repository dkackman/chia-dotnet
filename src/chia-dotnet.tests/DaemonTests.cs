using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
        public async Task GetStatus()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);

            // Act
            var returnValue = await Daemon.GetStatus(cts.Token);

            // Assert
            Assert.True(returnValue);
        }

        [Fact]
        public async Task GetWalletAddresses()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);

            // Act
            var returnValue = await Daemon.GetWalletAddresses(cancellationToken: cts.Token);

            // Assert
            Assert.NotNull(returnValue);
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
        public async Task ValidateInvalidKeyringPassphrase()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);

            // Act

            // Assert
            _ = await Assert.ThrowsAsync<ResponseException>(
                async () => await Daemon.ValidateKeyringPassphrase("spoon", cts.Token));
        }

        [Fact]
        public async Task UnlockKeyringInvalid()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);

            // Act

            // Assert
            _ = await Assert.ThrowsAsync<ResponseException>(async () => await Daemon.UnlockKeyring("spoon", cts.Token));
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

        [Fact]
        public async Task IsServiceRunning()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            var service = ServiceNames.Farmer;

            // Act
            var returnValue = await Daemon.IsServiceRunning(service: service, cancellationToken: cts.Token);

            // Assert
            Assert.True(returnValue);
        }

        [Fact]
        public async Task GetKeyForFingerprint()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            var (Wallets, Fingerprint) = await Wallet.GetWallets(false, cts.Token);

            // Act
            var returnValue = await Daemon.GetKeyForFingerprint(fingerprint: Fingerprint, cancellationToken: cts.Token);

            // Assert
            Assert.NotNull(returnValue);
        }

        [Fact]
        public async Task DeleteKeyByFingerprint()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            var fingerprint = 3449571893;

            // Act
            await Daemon.DeleteKeyByFingerprint(fingerprint: fingerprint, cancellationToken: cts.Token);

            // Assert

        }

        [Fact(Skip = "Destructive")]
        public async Task DeleteAllKeys()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);

            // Act
            await Daemon.DeleteAllKeys(cts.Token);

            // Assert

        }

        [Fact]
        public async Task AddDeletePrivateKey()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            var mnemonic = await Wallet.GenerateMnemonic(cts.Token);
            var passphrase = "spoons!!!";

            // Act
            var fingerprint = await Daemon.AddPrivateKey(mnemonic: string.Join(' ', mnemonic), passphrase: passphrase, cancellationToken: cts.Token);

            // Assert
            Assert.True(fingerprint > 0);
            await Daemon.DeleteKeyByFingerprint(fingerprint: fingerprint, cancellationToken: cts.Token);
        }

        [Fact(Skip = "Requires review")]
        public async Task CheckKeys()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            var rootPath = "~/.chia/mainnet/config";

            // Act
            await Daemon.CheckKeys(rootPath: rootPath, cancellationToken: cts.Token);

            // Assert

        }

        [Fact]
        public async Task GetKey()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            var (Wallets, Fingerprint) = await Wallet.GetWallets(false, cts.Token);

            // Act
            var returnValue = await Daemon.GetKey(fingerprint: Fingerprint, cancellationToken: cts.Token);

            // Assert
            Assert.NotNull(returnValue);
        }

        [Fact]
        public async Task GetKeys()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            var (Wallets, Fingerprint) = await Wallet.GetWallets(false, cts.Token);

            // Act
            var returnValue = await Daemon.GetKeys(fingerprint: Fingerprint, cancellationToken: cts.Token);

            // Assert
            Assert.NotNull(returnValue.ToList());
        }

        [Fact]
        public async Task SetDeleteLabel()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            var (Wallets, Fingerprint) = await Wallet.GetWallets(false, cts.Token);
            var label = "spoon";

            // Act
            await Daemon.SetLabel(fingerprint: Fingerprint, label: label, cancellationToken: cts.Token);
            await Daemon.DeleteLabel(fingerprint: Fingerprint, cancellationToken: cts.Token);

            // Assert

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
        public async Task Ping()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);

            // Act
            await Daemon.Ping(cts.Token);

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
            Assert.False(locked);
        }

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

        [Fact]
        public async Task CreateProxyFrom()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);

            // Act
            var fullNode = Daemon.CreateProxyFrom<FullNodeProxy>();
            var state = await fullNode.GetBlockchainState(cts.Token);

            // Assert
            Assert.NotNull(state);
        }

        [Fact(Skip = "CAUTION")]
        public async Task Exit()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);

            // Act
            await Daemon.Exit(cts.Token);

            // Assert

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

        [Fact]
        public async Task RunningServices()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);

            // Act
            var result = await Daemon.RunningServices(cts.Token);

            // Assert
            Assert.NotNull(result.ToList());
        }
    }
}
