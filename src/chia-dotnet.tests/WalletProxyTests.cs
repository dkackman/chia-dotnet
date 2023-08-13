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


        [Fact]

        public async Task GetLoggedInFingerprint()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);

            // Act
            var fingerprint = await Wallet.GetLoggedInFingerprint(cts.Token);

            // Assert
        }

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

        [Fact]
        public async Task GetPrivateKey()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            var fingerprint = await Wallet.GetLoggedInFingerprint(cts.Token);

            // Act
            var key = await Wallet.GetPrivateKey(fingerprint, cts.Token);

            // Assert
            Assert.NotNull(key);
        }

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
    }
}
