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

    internal Wallet StandardWallet => Fixture.TestHost.Services.GetService<Wallet>() ?? throw new Exception("Testbase improperly configured");

    internal TradeManager TradeManager => Fixture.TestHost.Services.GetService<TradeManager>() ?? throw new Exception("Testbase improperly configured");

    public TestBase(ChiaDotNetFixture fixture)
    {
        Fixture = fixture;
    }
}
