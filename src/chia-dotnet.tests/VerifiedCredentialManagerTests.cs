using System.Threading.Tasks;
using chia.dotnet.tests.Core;
using Xunit;
using System.Threading;
using System.Linq;

namespace chia.dotnet.tests;

public class VerifiedCredentialManagerTests : TestBase
{
    public VerifiedCredentialManagerTests(ChiaDotNetFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task Get()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);
        var vcs = await VCManager.GetList(cancellationToken: cts.Token);
        var vc  = vcs.First();
        var vcId = vc.VC.LauncherId;

        // Act
        var returnValue = await VCManager.Get(vcId: vcId, cancellationToken: cts.Token);

        // Assert
        Assert.NotNull(returnValue);
    }

    [Fact]
    public async Task GetList()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);

        // Act
        var returnValue = await VCManager.GetList(cancellationToken: cts.Token);

        // Assert
        Assert.NotNull(returnValue.ToList());
    }

    [Fact]
    public async Task Mint()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);
        var targetAddress = await StandardWallet.GetNextAddress(false, cts.Token);
        var (MyDid, CoinID) = await DIDWallet.GetDid(cts.Token);

        // Act
        var returnValue = await VCManager.Mint(targetAddress: targetAddress, didId: MyDid, cancellationToken: cts.Token);

        // Assert
        Assert.NotNull(returnValue.VCRecord);
    }

    [Fact(Skip = "Requires review")]
    public async Task Spend()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);
        var vcId = string.Empty;

        // Act
        var returnValue = await VCManager.Spend(vcId: vcId, cancellationToken: cts.Token);

        // Assert
        Assert.NotNull(returnValue.ToList());
    }

    [Fact(Skip = "Requires review")]
    public async Task AddProofs()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);
        VCProofs proofs = null;

        // Act
        await VCManager.AddProofs(proofs: proofs, cancellationToken: cts.Token);

        // Assert

    }

    [Fact(Skip = "Requires review")]
    public async Task GetProofsForRoot()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);
        var root = string.Empty;

        // Act
        var returnValue = await VCManager.GetProofsForRoot(root: root, cancellationToken: cts.Token);

        // Assert
        Assert.NotNull(returnValue.ToList());
    }

    [Fact(Skip = "Requires review")]
    public async Task Revoke()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);
        var vcParentId = string.Empty;

        // Act
        var returnValue = await VCManager.Revoke(vcParentId: vcParentId, cancellationToken: cts.Token);

        // Assert
        Assert.NotNull(returnValue.ToList());
    }

}
