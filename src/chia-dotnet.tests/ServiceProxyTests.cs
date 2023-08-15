using System.Threading;
using System.Threading.Tasks;
using chia.dotnet.tests.Core;
using System.Linq;
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
        Assert.NotNull(returnValue.ToList());
    }

    [Fact(Skip = "Fails over websocket. bug entered on chia github")]
    public async Task GetRoutes()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);

        // Act
        var returnValue = await FullNode.GetRoutes(cts.Token);

        // Assert
        Assert.NotNull(returnValue.ToList());
    }

    [Fact]
    public async Task OpenConnection()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);
        var host = "testnet10-node.chia.net";
        var port = 58444;

        // Act
        await Farmer.OpenConnection(host: host, port: port, cancellationToken: cts.Token);

        // Assert

    }

    [Fact]
    public async Task CloseConnection()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);
        var connections = await Farmer.GetConnections(cts.Token);
        var connection = connections.First();
        var nodeId = connection.NodeId;

        // Act
        await Farmer.CloseConnection(nodeId: nodeId, cancellationToken: cts.Token);

        // Assert

    }

}
