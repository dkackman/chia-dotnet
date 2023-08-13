using System;
using System.Threading.Tasks;
using chia.dotnet.tests.Core;
using Xunit;
using System.Threading;

namespace chia.dotnet.tests;

public class NFTWalletTests : TestBase
{
    public NFTWalletTests(ChiaDotNetFixture fixture) : base(fixture)
    {
    }

    [Fact(Skip = "Requires review")]
    public async Task Validate()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);

        // Act
        await NFTWallet.Validate(cancellationToken: cts.Token);

        // Assert

    }

    [Fact(Skip = "Requires review")]
    public async Task AddUri()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);
        String uri = string.Empty;
        String key = string.Empty;
        String nftCoinId = string.Empty;

        // Act
        var returnValue = await NFTWallet.AddUri(uri: uri, key: key, nftCoinId: nftCoinId, cancellationToken: cts.Token);

        // Assert
        Assert.NotNull(returnValue);
    }

    [Fact(Skip = "Requires review")]
    public async Task NftCountNfts()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);

        // Act
        var returnValue = await NFTWallet.NftCountNfts(cancellationToken: cts.Token);

        // Assert
        Assert.NotNull(returnValue);
    }

    [Fact(Skip = "Requires review")]
    public async Task GetNFTs()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);

        // Act
        var returnValue = await NFTWallet.GetNFTs(cancellationToken: cts.Token);

        // Assert
        Assert.NotNull(returnValue);
    }

    [Fact(Skip = "Requires review")]
    public async Task GetDid()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);

        // Act
        var returnValue = await NFTWallet.GetDid(cancellationToken: cts.Token);

        // Assert
        Assert.NotNull(returnValue);
    }

    [Fact(Skip = "Requires review")]
    public async Task MintNFT()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);
        NFTMintingInfo info = null;

        // Act
        var returnValue = await NFTWallet.MintNFT(info: info, cancellationToken: cts.Token);

        // Assert
        Assert.NotNull(returnValue);
    }

    [Fact(Skip = "Requires review")]
    public async Task NftMintBulk()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);
        NFTBulkMintingInfo info = null;

        // Act
        var returnValue = await NFTWallet.NftMintBulk(info: info, cancellationToken: cts.Token);

        // Assert
        Assert.NotNull(returnValue);
    }

    [Fact(Skip = "Requires review")]
    public async Task SetDID()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);
        String didId = string.Empty;

        // Act
        var returnValue = await NFTWallet.SetDID(didId: didId, cancellationToken: cts.Token);

        // Assert
        Assert.NotNull(returnValue);
    }

    [Fact(Skip = "Requires review")]
    public async Task SetStatus()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);
        String coinId = string.Empty;

        // Act
        await NFTWallet.SetStatus(coinId: coinId, cancellationToken: cts.Token);

        // Assert

    }

    [Fact(Skip = "Requires review")]
    public async Task Transfer()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);
        String targetAddress = string.Empty;
        String coinId = string.Empty;

        // Act
        var returnValue = await NFTWallet.Transfer(targetAddress: targetAddress, coinId: coinId, cancellationToken: cts.Token);

        // Assert
        Assert.NotNull(returnValue);
    }

}
