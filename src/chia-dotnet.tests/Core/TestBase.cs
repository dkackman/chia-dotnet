using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace chia.dotnet.tests.Core;

/// <summary>
/// base class that will grab the Dependencies that have been registered by fixture.
/// </summary>
/// 
public class TestBase : IClassFixture<ChiaDotNetFixture>
{
    internal ChiaDotNetFixture Fixture { get; }
    
    internal DaemonProxy Daemon => Fixture.TestHost.Services.GetRequiredService<DaemonProxy>();
    
    internal WebSocketRpcClient WebSocketClient => Fixture.TestHost.Services.GetService<WebSocketRpcClient>();
    
    internal HttpRpcClient HttpWebClient => Fixture.TestHost.Services.GetService<HttpRpcClient>();
    
    internal FullNodeProxy FullNode => Fixture.TestHost.Services.GetService<FullNodeProxy>();
    
    internal FarmerProxy Farmer => Fixture.TestHost.Services.GetService<FarmerProxy>();
    
    internal WalletProxy Wallet => Fixture.TestHost.Services.GetService<WalletProxy>();
    
    internal HarvesterProxy Harvester => Fixture.TestHost.Services.GetService<HarvesterProxy>();
    
    public TestBase(ChiaDotNetFixture fixture)
    {
        Fixture = fixture;
    }
}
