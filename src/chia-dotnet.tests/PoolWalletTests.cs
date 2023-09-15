using System;
using System.Threading;
using System.Threading.Tasks;
using chia.dotnet.tests.Core;
using Xunit;

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

    [Fact]
    public async Task JoinPool()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);
        var poolUri = new Uri("http://testnet.spacefarmers.io:8080/");
        var poolInfo = await WalletProxy.GetPoolInfo(poolUri, cts.Token);
        Assert.NotNull(poolInfo.TargetPuzzleHash);
        var targetPuzzlehash = poolInfo.TargetPuzzleHash;
        var poolUrl = poolUri.ToString();
        var relativeLockHeight = poolInfo.RelativeLockHeight;

        // Act
        var returnValue = await PoolWallet.JoinPool(targetPuzzlehash: targetPuzzlehash, poolUrl: poolUrl, relativeLockHeight: relativeLockHeight, cancellationToken: cts.Token);

        // Assert
        Assert.NotNull(returnValue);
    }

    [Fact(Skip = "Requires data")]
    public async Task SelfPool()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);

        // Act
        var returnValue = await PoolWallet.SelfPool(cancellationToken: cts.Token);

        // Assert
        Assert.NotNull(returnValue);
    }

    [Fact(Skip = "Requires data")]
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
