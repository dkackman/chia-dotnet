using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using chia.dotnet.tests.Core;
using Xunit;
using System.Collections;
using System.Threading;

namespace chia.dotnet.tests;

public class DIDWalletTests : TestBase
{
    public DIDWalletTests(ChiaDotNetFixture fixture) : base(fixture)
    {
    }

    [Fact(Skip = "Requires review")]
    public async Task Validate()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);

        // Act
        await DIDWallet.Validate(cancellationToken: cts.Token);

        // Assert

    }

    [Fact(Skip = "Requires review")]
    public async Task UpdateRecoveryIds()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);
        IEnumerable<string> newList = null;

        // Act
        await DIDWallet.UpdateRecoveryIds(newList: newList, cancellationToken: cts.Token);

        // Assert

    }

    [Fact(Skip = "Requires review")]
    public async Task Spend()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);
        String puzzlehash = string.Empty;

        // Act
        await DIDWallet.Spend(puzzlehash: puzzlehash, cancellationToken: cts.Token);

        // Assert

    }

    [Fact(Skip = "Requires review")]
    public async Task GetDid()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);

        // Act
        var returnValue = await DIDWallet.GetDid(cancellationToken: cts.Token);

        // Assert
        Assert.NotNull(returnValue);
    }

    [Fact(Skip = "Requires review")]
    public async Task GetCurrentCoinInfo()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);

        // Act
        var returnValue = await DIDWallet.GetCurrentCoinInfo(cancellationToken: cts.Token);

        // Assert
        Assert.NotNull(returnValue);
    }

    [Fact(Skip = "Requires review")]
    public async Task GetPubKey()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);

        // Act
        var returnValue = await DIDWallet.GetPubKey(cancellationToken: cts.Token);

        // Assert
        Assert.NotNull(returnValue);
    }

    [Fact(Skip = "Requires review")]
    public async Task GetName()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);

        // Act
        var returnValue = await DIDWallet.GetName(cancellationToken: cts.Token);

        // Assert
        Assert.NotNull(returnValue);
    }

    [Fact(Skip = "Requires review")]
    public async Task MessageSpend()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);
        IEnumerable<string> puzzleAnnouncements = null;
        IEnumerable<string> coinAnnouncements = null;

        // Act
        var returnValue = await DIDWallet.MessageSpend(puzzleAnnouncements: puzzleAnnouncements, coinAnnouncements: coinAnnouncements, cancellationToken: cts.Token);

        // Assert
        Assert.NotNull(returnValue);
    }

    [Fact(Skip = "Requires review")]
    public async Task SetName()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);
        String name = string.Empty;

        // Act
        await DIDWallet.SetName(name: name, cancellationToken: cts.Token);

        // Assert

    }

    [Fact(Skip = "Requires review")]
    public async Task GetMetadata()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);

        // Act
        var returnValue = await DIDWallet.GetMetadata(cancellationToken: cts.Token);

        // Assert
        Assert.NotNull(returnValue);
    }

    [Fact(Skip = "Requires review")]
    public async Task UpdateMetadata()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);
        String metadata = string.Empty;

        // Act
        var returnValue = await DIDWallet.UpdateMetadata(metadata: metadata, cancellationToken: cts.Token);

        // Assert
        Assert.NotNull(returnValue);
    }

    [Fact(Skip = "Requires review")]
    public async Task RecoverySpend()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);
        IEnumerable<string> attestData = null;
        String pubkey = string.Empty;
        String puzzlehash = string.Empty;

        // Act
        await DIDWallet.RecoverySpend(attestData: attestData, pubkey: pubkey, puzzlehash: puzzlehash, cancellationToken: cts.Token);

        // Assert

    }

    [Fact(Skip = "Requires review")]
    public async Task GetRecoveryList()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);

        // Act
        var returnValue = await DIDWallet.GetRecoveryList(cancellationToken: cts.Token);

        // Assert
        Assert.NotNull(returnValue);
    }

    [Fact(Skip = "Requires review")]
    public async Task CreateAttest()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);
        String coinName = string.Empty;
        String pubkey = string.Empty;
        String puzHash = string.Empty;

        // Act
        var returnValue = await DIDWallet.CreateAttest(coinName: coinName, pubkey: pubkey, puzHash: puzHash, cancellationToken: cts.Token);

        // Assert
        Assert.NotNull(returnValue);
    }

    [Fact(Skip = "Requires review")]
    public async Task GetInformationNeededForRecovery()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);

        // Act
        var returnValue = await DIDWallet.GetInformationNeededForRecovery(cancellationToken: cts.Token);

        // Assert
        Assert.NotNull(returnValue);
    }

    [Fact(Skip = "Requires review")]
    public async Task CreateBackupFile()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);

        // Act
        var returnValue = await DIDWallet.CreateBackupFile(cancellationToken: cts.Token);

        // Assert
        Assert.NotNull(returnValue);
    }

    [Fact(Skip = "Requires review")]
    public async Task Transfer()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);
        String innerAddress = string.Empty;

        // Act
        var returnValue = await DIDWallet.Transfer(innerAddress: innerAddress, cancellationToken: cts.Token);

        // Assert
        Assert.NotNull(returnValue);
    }

}
