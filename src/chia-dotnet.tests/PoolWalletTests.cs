using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using chia.dotnet.tests.Core;
using Xunit;
using System.Threading;

namespace chia.dotnet.tests;

public class PoolWalletTests : TestBase
{
    public PoolWalletTests(ChiaDotNetFixture fixture) : base(fixture)
    {
    }

    [Fact(Skip = "Requires review")]
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
        String targetPuzzlehash = string.Empty;
        String poolUrl = string.Empty;
        UInt32 relativeLockHeight = 0;

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

    [Fact(Skip = "Requires review")]
    public async Task AbsorbRewards()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);

        // Act
        var returnValue = await PoolWallet.AbsorbRewards(cancellationToken: cts.Token);

        // Assert
        Assert.NotNull(returnValue);
    }

    [Fact(Skip = "Requires review")]
    public async Task Status()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);

        // Act
        var returnValue = await PoolWallet.Status(cancellationToken: cts.Token);

        // Assert
        Assert.NotNull(returnValue);
    }

}
