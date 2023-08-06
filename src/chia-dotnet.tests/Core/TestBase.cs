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
    
    private DaemonProxy Daemon => Fixture.TestHost.Services.GetRequiredService<DaemonProxy>();
    
    private WebSocketRpcClient WebSocketClient => Fixture.TestHost.Services.GetService<WebSocketRpcClient>();
    
    private HttpRpcClient HttpWebClient => Fixture.TestHost.Services.GetService<HttpRpcClient>();
    
    internal FullNodeProxy FullNode => Fixture.TestHost.Services.GetService<FullNodeProxy>();
    
    internal FarmerProxy Farmer => Fixture.TestHost.Services.GetService<FarmerProxy>();
    
    public TestBase(ChiaDotNetFixture fixture)
    {
        Fixture = fixture;
    }
}
