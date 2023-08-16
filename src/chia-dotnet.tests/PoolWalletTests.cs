using System.Threading.Tasks;
using chia.dotnet.tests.Core;
using Xunit;
using System.Threading;

namespace chia.dotnet.tests;

public class PoolWalletTests : TestBase
{
    public PoolWalletTests(ChiaDotNetFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task Validate()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);

        // Act
        await PoolWallet.Validate(cancellationToken: cts.Token);

        // Assert

    }

    [Fact(Skip = "Requires review")]
    public async Task JoinPool()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);
        var targetPuzzlehash = string.Empty;
        var poolUrl = string.Empty;
        uint relativeLockHeight = 0;

        // Act
        var returnValue = await PoolWallet.JoinPool(targetPuzzlehash: targetPuzzlehash, poolUrl: poolUrl, relativeLockHeight: relativeLockHeight, cancellationToken: cts.Token);

        // Assert
        Assert.NotNull(returnValue);
    }

    [Fact(Skip = "Requires review")]
    public async Task SelfPool()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);

        // Act
        var returnValue = await PoolWallet.SelfPool(cancellationToken: cts.Token);

        // Assert
        Assert.NotNull(returnValue);
    }

    [Fact]
    public async Task AbsorbRewards()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);

        // Act
        var (State, Transaction) = await PoolWallet.AbsorbRewards(cancellationToken: cts.Token);

        // Assert
        Assert.NotNull(State);
    }

    [Fact]
    public async Task Status()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);

        // Act
        var (State, UnconfirmedTransactions) = await PoolWallet.Status(cancellationToken: cts.Token);

        // Assert
        Assert.NotNull(State);
    }

}
