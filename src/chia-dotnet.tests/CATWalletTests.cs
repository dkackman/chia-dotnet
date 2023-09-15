using System;
using System.Threading;
using System.Threading.Tasks;
using chia.dotnet.tests.Core;
using Xunit;

namespace chia.dotnet.tests
{
    public class CATWalletTests : TestBase
    {
        public CATWalletTests(ChiaDotNetFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task Validate()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);

            // Act
            await CATWallet.Validate(cancellationToken: cts.Token);

            // Assert

        }

        [Fact]
        public async Task GetName()
        {
            using var cts = new CancellationTokenSource(20000);

            var name = await CATWallet.GetName(cts.Token);

            Assert.NotNull(name);
        }

        [Fact]
        public async Task SetName()
        {
            using var cts = new CancellationTokenSource(20000);

            var originalName = await CATWallet.GetName(cts.Token);

            var newName = Guid.NewGuid().ToString();
            await CATWallet.SetName(newName, cts.Token);

            var name = await CATWallet.GetName(cts.Token);

            Assert.Equal(newName, name);
            await CATWallet.SetName(originalName, cts.Token);
        }

        [Fact]
        public async Task GetAssetId()
        {
            using var cts = new CancellationTokenSource(20000);

            var id = await CATWallet.GetAssetId(cts.Token);

            Assert.NotNull(id);
        }

        [Fact(Skip = "Requires review")]
        public async Task Spend()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);
            var innerAddress = string.Empty;
            ulong amount = 0;

            // Act
            var returnValue = await CATWallet.Spend(innerAddress: innerAddress, amount: amount, cancellationToken: cts.Token);

            // Assert
            Assert.NotNull(returnValue);
        }
    }
}
