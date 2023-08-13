using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using chia.dotnet.tests.Core;
using Xunit;
using System.Collections;
using System.Threading;

namespace chia.dotnet.tests;

public class DataLayerWalletTests : TestBase
{
    public DataLayerWalletTests(ChiaDotNetFixture fixture) : base(fixture)
    {
    }

    [Fact(Skip = "Requires review")]
    public async Task Validate()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);

        // Act
        await DataLayerWallet.Validate(cancellationToken: cts.Token);

        // Assert

    }

    [Fact(Skip = "Requires review")]
    public async Task DeleteMirror()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);
        String coinId = string.Empty;

        // Act
        var returnValue = await DataLayerWallet.DeleteMirror(coinId: coinId, cancellationToken: cts.Token);

        // Assert
        Assert.NotNull(returnValue);
    }

    [Fact(Skip = "Requires review")]
    public async Task GetMirrors()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);
        String launcherId = string.Empty;

        // Act
        var returnValue = await DataLayerWallet.GetMirrors(launcherId: launcherId, cancellationToken: cts.Token);

        // Assert
        Assert.NotNull(returnValue);
    }

    [Fact(Skip = "Requires review")]
    public async Task History()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);
        String launcherId = string.Empty;

        // Act
        var returnValue = await DataLayerWallet.History(launcherId: launcherId, cancellationToken: cts.Token);

        // Assert
        Assert.NotNull(returnValue);
    }

    [Fact(Skip = "Requires review")]
    public async Task LatestSingleton()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);
        String root = string.Empty;
        String launcherId = string.Empty;

        // Act
        var returnValue = await DataLayerWallet.LatestSingleton(root: root, launcherId: launcherId, cancellationToken: cts.Token);

        // Assert
        Assert.NotNull(returnValue);
    }

    [Fact(Skip = "Requires review")]
    public async Task NewMirror()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);
        String launcherId = string.Empty;
        UInt64 amount = 0;
        IEnumerable<string> urls = null;

        // Act
        var returnValue = await DataLayerWallet.NewMirror(launcherId: launcherId, amount: amount, urls: urls, cancellationToken: cts.Token);

        // Assert
        Assert.NotNull(returnValue);
    }

    [Fact(Skip = "Requires review")]
    public async Task OwnedSingletons()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);

        // Act
        var returnValue = await DataLayerWallet.OwnedSingletons(cancellationToken: cts.Token);

        // Assert
        Assert.NotNull(returnValue);
    }

    [Fact(Skip = "Requires review")]
    public async Task SingletonsByRoot()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);
        String root = string.Empty;
        String launcherId = string.Empty;

        // Act
        var returnValue = await DataLayerWallet.SingletonsByRoot(root: root, launcherId: launcherId, cancellationToken: cts.Token);

        // Assert
        Assert.NotNull(returnValue);
    }

    [Fact(Skip = "Requires review")]
    public async Task StopTracking()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);
        String launcherId = string.Empty;

        // Act
        await DataLayerWallet.StopTracking(launcherId: launcherId, cancellationToken: cts.Token);

        // Assert

    }

    [Fact(Skip = "Requires review")]
    public async Task TrackNew()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);
        String launcherId = string.Empty;

        // Act
        await DataLayerWallet.TrackNew(launcherId: launcherId, cancellationToken: cts.Token);

        // Assert

    }

    [Fact(Skip = "Requires review")]
    public async Task UpdateMultiple()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);
        IEnumerable<SingletonInfo> updates = null;

        // Act
        var returnValue = await DataLayerWallet.UpdateMultiple(updates: updates, cancellationToken: cts.Token);

        // Assert
        Assert.NotNull(returnValue);
    }

    [Fact(Skip = "Requires review")]
    public async Task UpdateRoot()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);
        String newRoot = string.Empty;
        String launcherId = string.Empty;

        // Act
        var returnValue = await DataLayerWallet.UpdateRoot(newRoot: newRoot, launcherId: launcherId, cancellationToken: cts.Token);

        // Assert
        Assert.NotNull(returnValue);
    }

}
