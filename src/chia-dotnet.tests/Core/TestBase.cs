using Microsoft.Extensions.DependencyInjection;
using Xunit;
using System;

namespace chia.dotnet.tests.Core;

/// <summary>
/// base class that will grab the Dependencies that have been registered by fixture.
/// </summary>
/// 
public class TestBase : IClassFixture<ChiaDotNetFixture>
{
    internal ChiaDotNetFixture Fixture { get; }

    internal DaemonProxy Daemon => Fixture.TestHost.Services.GetRequiredService<DaemonProxy>();

    internal WebSocketRpcClient WebSocketClient => Fixture.TestHost.Services.GetService<WebSocketRpcClient>() ?? throw new Exception("Testbase improperly configured");

    internal HttpRpcClient HttpWebClient => Fixture.TestHost.Services.GetService<HttpRpcClient>() ?? throw new Exception("Testbase improperly configured");

    internal FullNodeProxy FullNode => Fixture.TestHost.Services.GetService<FullNodeProxy>() ?? throw new Exception("Testbase improperly configured");

    internal FarmerProxy Farmer => Fixture.TestHost.Services.GetService<FarmerProxy>() ?? throw new Exception("Testbase improperly configured");

    internal CrawlerProxy Crawler => Fixture.TestHost.Services.GetService<CrawlerProxy>() ?? throw new Exception("Testbase improperly configured");

    internal WalletProxy Wallet => Fixture.TestHost.Services.GetService<WalletProxy>() ?? throw new Exception("Testbase improperly configured");

    internal HarvesterProxy Harvester => Fixture.TestHost.Services.GetService<HarvesterProxy>() ?? throw new Exception("Testbase improperly configured");

    internal PlotterProxy Plotter => Fixture.TestHost.Services.GetService<PlotterProxy>() ?? throw new Exception("Testbase improperly configured");

    internal DataLayerProxy DataLayer => Fixture.TestHost.Services.GetService<DataLayerProxy>() ?? throw new Exception("Testbase improperly configured");

    internal Wallet StandardWallet => Fixture.TestHost.Services.GetService<Wallet>() ?? throw new Exception("Testbase improperly configured");

    internal TradeManager TradeManager => Fixture.TestHost.Services.GetService<TradeManager>() ?? throw new Exception("Testbase improperly configured");

    internal VerifiedCredentialManager VCManager => Fixture.TestHost.Services.GetService<VerifiedCredentialManager>() ?? throw new Exception("Testbase improperly configured");

    internal CATWallet CATWallet => Fixture.TestHost.Services.GetService<WalletFactory>()!.GetWallet<CATWallet>(WalletType.CAT);

    internal DIDWallet DIDWallet => Fixture.TestHost.Services.GetService<WalletFactory>()!.GetWallet<DIDWallet>(WalletType.DISTRIBUTED_ID);

    internal PoolWallet PoolWallet => Fixture.TestHost.Services.GetService<WalletFactory>()!.GetWallet<PoolWallet>(WalletType.POOLING_WALLET);

    internal DataLayerWallet DataLayerWallet => Fixture.TestHost.Services.GetService<WalletFactory>()!.GetWallet<DataLayerWallet>(WalletType.DATA_LAYER);

    internal NFTWallet NFTWallet => Fixture.TestHost.Services.GetService<WalletFactory>()!.GetWallet<NFTWallet>(WalletType.NFT);
    
    public TestBase(ChiaDotNetFixture fixture)
    {
        Fixture = fixture;
        if (Fixture.TestHost.Services.GetService<WalletFactory>() is null)
        {
            throw new Exception("Testbase improperly configured");
        }
    }
}
