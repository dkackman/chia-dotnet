using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using chia.dotnet.tests.Core;
using Xunit;
using System.Threading;
using System.Linq;

namespace chia.dotnet.tests;

public class DataLayerProxyTests : TestBase
{
    public DataLayerProxyTests(ChiaDotNetFixture fixture) : base(fixture)
    {
    }

    [Fact(Skip = "Requires review")]
    public async Task AddMirror()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);
        var id = string.Empty;
        ulong amount = 0;
        IEnumerable<string> urls = null;

        // Act
        await DataLayer.AddMirror(id: id, amount: amount, urls: urls, cancellationToken: cts.Token);

        // Assert

    }

    [Fact(Skip = "Requires review")]
    public async Task AddMissingFiles()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);
        string[] ids = null;
        var foldername = string.Empty;

        // Act
        await DataLayer.AddMissingFiles(ids: ids, foldername: foldername, cancellationToken: cts.Token);

        // Assert

    }

    [Fact(Skip = "Requires review")]
    public async Task BatchUpdate()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);
        var id = string.Empty;
        IDictionary<string, string> changeList = null;

        // Act
        var returnValue = await DataLayer.BatchUpdate(id: id, changeList: changeList, cancellationToken: cts.Token);

        // Assert
        Assert.NotNull(returnValue);
    }

    [Fact(Skip = "Requires review")]
    public async Task CancelOffer()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);
        var tradeId = string.Empty;

        // Act
        await DataLayer.CancelOffer(tradeId: tradeId, cancellationToken: cts.Token);

        // Assert

    }

    [Fact(Skip = "Requires review")]
    public async Task CheckPlugins()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);

        // Act
        var returnValue = await DataLayer.CheckPlugins(cancellationToken: cts.Token);

        // Assert
        Assert.NotNull(returnValue);
    }

    [Fact(Skip = "Requires review")]
    public async Task CreateDataStore()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);

        // Act
        var (id, txs) = await DataLayer.CreateDataStore(cancellationToken: cts.Token);

        // Assert
        Assert.NotNull(txs.ToList());
    }

    [Fact(Skip = "Requires review")]
    public async Task DeleteKey()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);
        var key = string.Empty;
        var id = string.Empty;

        // Act
        var returnValue = await DataLayer.DeleteKey(key: key, id: id, cancellationToken: cts.Token);

        // Assert
        Assert.NotNull(returnValue);
    }

    [Fact(Skip = "Requires review")]
    public async Task DeleteMirror()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);
        var coinId = string.Empty;

        // Act
        await DataLayer.DeleteMirror(coinId: coinId, cancellationToken: cts.Token);

        // Assert

    }

    [Fact(Skip = "Requires review")]
    public async Task GetAncestors()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);
        var id = string.Empty;
        var hash = string.Empty;

        // Act
        var returnValue = await DataLayer.GetAncestors(id: id, hash: hash, cancellationToken: cts.Token);

        // Assert
        Assert.NotNull(returnValue);
    }

    [Fact(Skip = "Requires review")]
    public async Task GetKeys()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);
        var id = string.Empty;
        var rootHash = string.Empty;

        // Act
        var returnValue = await DataLayer.GetKeys(id: id, rootHash: rootHash, cancellationToken: cts.Token);

        // Assert
        Assert.NotNull(returnValue);
    }

    [Fact(Skip = "Requires review")]
    public async Task GetKeysValues()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);
        var id = string.Empty;
        var rootHash = string.Empty;

        // Act
        var returnValue = await DataLayer.GetKeysValues(id: id, rootHash: rootHash, cancellationToken: cts.Token);

        // Assert
        Assert.NotNull(returnValue);
    }

    [Fact(Skip = "Requires review")]
    public async Task GetKVDiff()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);
        var id = string.Empty;
        var hash1 = string.Empty;
        var hash2 = string.Empty;

        // Act
        var returnValue = await DataLayer.GetKVDiff(id: id, hash1: hash1, hash2: hash2, cancellationToken: cts.Token);

        // Assert
        Assert.NotNull(returnValue);
    }

    [Fact(Skip = "Requires review")]
    public async Task GetLocalRoot()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);
        var id = string.Empty;

        // Act
        var returnValue = await DataLayer.GetLocalRoot(id: id, cancellationToken: cts.Token);

        // Assert
        Assert.NotNull(returnValue);
    }

    [Fact(Skip = "Requires review")]
    public async Task GetMirrors()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);
        var id = string.Empty;

        // Act
        var returnValue = await DataLayer.GetMirrors(id: id, cancellationToken: cts.Token);

        // Assert
        Assert.NotNull(returnValue);
    }

    [Fact(Skip = "Requires review")]
    public async Task GetOwnedStores()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);

        // Act
        var returnValue = await DataLayer.GetOwnedStores(cancellationToken: cts.Token);

        // Assert
        Assert.NotNull(returnValue);
    }

    [Fact(Skip = "Requires review")]
    public async Task GetRoot()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);
        var id = string.Empty;

        // Act
        var returnValue = await DataLayer.GetRoot(id: id, cancellationToken: cts.Token);

        // Assert
        Assert.NotNull(returnValue);
    }

    [Fact(Skip = "Requires review")]
    public async Task GetRootHistory()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);
        var id = string.Empty;

        // Act
        var returnValue = await DataLayer.GetRootHistory(id: id, cancellationToken: cts.Token);

        // Assert
        Assert.NotNull(returnValue);
    }

    [Fact(Skip = "Requires review")]
    public async Task GetRoots()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);
        IEnumerable<string> ids = null;

        // Act
        var returnValue = await DataLayer.GetRoots(ids: ids, cancellationToken: cts.Token);

        // Assert
        Assert.NotNull(returnValue);
    }

    [Fact(Skip = "Requires review")]
    public async Task GetSyncStatus()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);
        var id = string.Empty;

        // Act
        var returnValue = await DataLayer.GetSyncStatus(id: id, cancellationToken: cts.Token);

        // Assert
        Assert.NotNull(returnValue);
    }

    [Fact(Skip = "Requires review")]
    public async Task GetValue()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);
        var rootHash = string.Empty;
        var key = string.Empty;
        var id = string.Empty;

        // Act
        var returnValue = await DataLayer.GetValue(rootHash: rootHash, key: key, id: id, cancellationToken: cts.Token);

        // Assert
        Assert.NotNull(returnValue);
    }

    [Fact(Skip = "Requires review")]
    public async Task Insert()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);
        var id = string.Empty;
        var value = string.Empty;

        // Act
        var returnValue = await DataLayer.Insert(id: id, value: value, cancellationToken: cts.Token);

        // Assert
        Assert.NotNull(returnValue);
    }

    [Fact(Skip = "Requires review")]
    public async Task MakeOffer()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);
        IEnumerable<OfferStore> maker = null;
        IEnumerable<OfferStore> taker = null;

        // Act
        var returnValue = await DataLayer.MakeOffer(maker: maker, taker: taker, cancellationToken: cts.Token);

        // Assert
        Assert.NotNull(returnValue);
    }

    [Fact(Skip = "Requires review")]
    public async Task RemoveSubscriptions()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);
        var id = string.Empty;
        IEnumerable<string> urls = null;

        // Act
        await DataLayer.RemoveSubscriptions(id: id, urls: urls, cancellationToken: cts.Token);

        // Assert

    }

    [Fact(Skip = "Requires review")]
    public async Task Subscribe()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);
        var id = string.Empty;
        IEnumerable<string> urls = null;

        // Act
        await DataLayer.Subscribe(id: id, urls: urls, cancellationToken: cts.Token);

        // Assert

    }

    [Fact(Skip = "Requires review")]
    public async Task Subscriptions()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);

        // Act
        var returnValue = await DataLayer.Subscriptions(cancellationToken: cts.Token);

        // Assert
        Assert.NotNull(returnValue);
    }

    [Fact(Skip = "Requires review")]
    public async Task TakeOffer()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);
        Object offer = null;

        // Act
        var returnValue = await DataLayer.TakeOffer(offer: offer, cancellationToken: cts.Token);

        // Assert
        Assert.NotNull(returnValue);
    }

    [Fact(Skip = "Requires review")]
    public async Task Unsubscribe()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);
        var id = string.Empty;

        // Act
        await DataLayer.Unsubscribe(id: id, cancellationToken: cts.Token);

        // Assert

    }

    [Fact(Skip = "Requires review")]
    public async Task VerifyOffer()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);
        DataLayerOffer offer = null;

        // Act
        var (Valid, Fee) = await DataLayer.VerifyOffer(offer: offer, cancellationToken: cts.Token);

        // Assert
        Assert.True(Valid);
    }

}
