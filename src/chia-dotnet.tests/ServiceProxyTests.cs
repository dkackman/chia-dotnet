using System;
using System.Threading;
using System.Threading.Tasks;
using chia.dotnet.tests.Core;
using Xunit;

namespace chia.dotnet.tests;

public class ServiceProxyTests : TestBase
{
    public ServiceProxyTests(ChiaDotNetFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task HealthZ()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);

        // Act
        await Farmer.HealthZ(cts.Token);

        // Assert

    }

    [Fact(Skip = "Will stop the full node")]
    public async Task StopNode()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);

        // Act
        await Farmer.StopNode(cts.Token);

        // Assert

    }

    [Fact]
    public async Task GetConnections()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);

        // Act
        var returnValue = await Farmer.GetConnections(cts.Token);

        // Assert
        Assert.NotNull(returnValue);
    }

    [Fact(Skip = "Error coming back from rpc: get_routes() takes 1 positional argument but 2 were given")]
    public async Task GetRoutes()
    {
        // Arrange
        using var cts = new CancellationTokenSource(1500000);

        // Act
        var returnValue = await FullNode.GetRoutes(cts.Token);

        // Assert
        Assert.NotNull(returnValue);
    }

    [Fact]
    public async Task OpenConnection()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);
        String host = "testnet10-node.chia.net";
        Int32 port = 58444;

        // Act
        await Farmer.OpenConnection(host: host, port: port, cancellationToken: cts.Token);

        // Assert

    }

    [Fact(Skip = "Requires review")]
    public async Task CloseConnection()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);
        String nodeId = "";

        // Act
        await Farmer.CloseConnection(nodeId: nodeId, cancellationToken: cts.Token);

        // Assert

    }

}
