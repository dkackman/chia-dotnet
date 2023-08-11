using System;
using System.Threading;
using System.Threading.Tasks;
using chia.dotnet.tests.Core;
using Xunit;

namespace chia.dotnet.tests;

public class CrawlerProxyTests : TestBase
{
    public CrawlerProxyTests(ChiaDotNetFixture fixture) : base(fixture)
    {
    }

    [Fact]
    public async Task GetPeerCounts()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);

        // Act
        var returnValue = await Crawler.GetPeerCounts(cts.Token);

        // Assert
        Assert.NotNull(returnValue);
    }

    [Fact]
    public async Task GetIPs()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);
        var after = DateTime.Now - TimeSpan.FromDays(2);

        // Act
        var (ips, total) = await Crawler.GetIPs(after: after, cancellationToken: cts.Token);

        // Assert
        Assert.NotNull(ips);
    }

}
