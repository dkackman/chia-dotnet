using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using chia.dotnet.tests.Core;
using Xunit;

namespace chia.dotnet.tests;

public class NFTWalletTests : TestBase
{
    public NFTWalletTests(ChiaDotNetFixture fixture) : base(fixture)
    {
    }

    [Fact]
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
        var uri = string.Empty;
        var key = string.Empty;
        var nftCoinId = string.Empty;

        // Act
        var returnValue = await NFTWallet.AddUri(uri: uri, key: key, nftCoinId: nftCoinId, cancellationToken: cts.Token);

        // Assert
        Assert.NotNull(returnValue);
    }

    [Fact]
    public async Task NftCountNfts()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);

        // Act
        var returnValue = await NFTWallet.CountNFTs(cancellationToken: cts.Token);

        // Assert
        Assert.True(returnValue >= 0);
    }

    [Fact]
    public async Task GetNFTs()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);

        // Act
        var returnValue = await NFTWallet.GetNFTs(cancellationToken: cts.Token);

        // Assert
        Assert.NotNull(returnValue.ToList());
    }

    [Fact]
    public async Task GetDid()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);

        // Act
        var returnValue = await NFTWallet.GetDID(cancellationToken: cts.Token);

        // Assert
        Assert.NotNull(returnValue);
    }

    [Fact]
    public async Task MintNFT()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);
        NFTMintingInfo info = new()
        {
            Uris = new string[] {
                "https://nftstorage.link/ipfs/bafybeif37wgxiwsddi7iyovgbjdhskaw2kl2o2gym4zg3x5dsygvmhvs2y/01971..jpg",
                "ipfs://bafybeif37wgxiwsddi7iyovgbjdhskaw2kl2o2gym4zg3x5dsygvmhvs2y/01971..jpg"
              },
            Hash = "cc02ca7623bf675638c34a8b28eb762a7b88ca2eb315462fac0b850b4145f9b9",
            MetaUris = new string[] {
              "https://nftstorage.link/ipfs/bafybeif37wgxiwsddi7iyovgbjdhskaw2kl2o2gym4zg3x5dsygvmhvs2y/metadata.json",
                 "ipfs://bafybeif37wgxiwsddi7iyovgbjdhskaw2kl2o2gym4zg3x5dsygvmhvs2y/metadata.json"
            },
            MetaHash = "c47bdeec776869aed7239929c7568fcf15ea808132a93dbf987b0712afd5e661",
            LicenseUris = new string[] {
                "https://raw.githubusercontent.com/CompVis/stable-diffusion/main/LICENSE",
              },
            LicenseHash = "be351ebe7ac01bcdbb018639aadcfd38f136b7dc3f2a3d4d3a24db51d1b210ef",
        };

        // Act
        var returnValue = await NFTWallet.Mint(info: info, cancellationToken: cts.Token);

        // Assert
        Assert.NotNull(returnValue.SpendBundle);
    }

    [Fact]
    public async Task NftMintBulk()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);
        NFTBulkMintingInfo info = new()
        {
            MetadataList = new List<NFTMintEntry>
            {
                new NFTMintEntry
                {
                    Uris = new string[] {
                        "https://nftstorage.link/ipfs/bafybeif37wgxiwsddi7iyovgbjdhskaw2kl2o2gym4zg3x5dsygvmhvs2y/01971..jpg",
                        "ipfs://bafybeif37wgxiwsddi7iyovgbjdhskaw2kl2o2gym4zg3x5dsygvmhvs2y/01971..jpg"
                      },
                    Hash = "cc02ca7623bf675638c34a8b28eb762a7b88ca2eb315462fac0b850b4145f9b9",
                    MetaUris = new string[] {
                      "https://nftstorage.link/ipfs/bafybeif37wgxiwsddi7iyovgbjdhskaw2kl2o2gym4zg3x5dsygvmhvs2y/metadata.json",
                         "ipfs://bafybeif37wgxiwsddi7iyovgbjdhskaw2kl2o2gym4zg3x5dsygvmhvs2y/metadata.json"
                    },
                    MetaHash = "c47bdeec776869aed7239929c7568fcf15ea808132a93dbf987b0712afd5e661",
                    LicenseUris = new string[] {
                        "https://raw.githubusercontent.com/CompVis/stable-diffusion/main/LICENSE",
                      },
                    LicenseHash = "be351ebe7ac01bcdbb018639aadcfd38f136b7dc3f2a3d4d3a24db51d1b210ef",
                }
            }
        };

        // Act
        var returnValue = await NFTWallet.MintBulk(info: info, cancellationToken: cts.Token);

        // Assert
        Assert.NotNull(returnValue.SpendBundle);
    }

    [Fact]
    public async Task SetDID()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);
        var (MyDid, CoinID) = await DIDWallet.GetDid(cts.Token);

        var nfts = await NFTWallet.GetNFTs(startIndex: 0, num: 2, cancellationToken: cts.Token);
        var nft = nfts.First();
        var nftCoinId = nft.NFTCoinID;

        // Act
        var returnValue = await NFTWallet.SetDID(didId: MyDid, nftCoinId: nftCoinId, cancellationToken: cts.Token);

        // Assert
        Assert.NotNull(returnValue);
    }

    [Fact]
    public async Task SetStatus()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);
        var nfts = await NFTWallet.GetNFTs(startIndex: 0, num: 2, cancellationToken: cts.Token);
        var nft = nfts.First();
        var nftCoinId = nft.NFTCoinID;

        // Act
        await NFTWallet.SetStatus(coinId: nftCoinId, cancellationToken: cts.Token);

        // Assert

    }

    [Fact(Skip = "Requires review")]
    public async Task Transfer()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);
        var targetAddress = string.Empty;
        var coinId = string.Empty;

        // Act
        var returnValue = await NFTWallet.Transfer(targetAddress: targetAddress, coinId: coinId, cancellationToken: cts.Token);

        // Assert
        Assert.NotNull(returnValue);
    }

}
