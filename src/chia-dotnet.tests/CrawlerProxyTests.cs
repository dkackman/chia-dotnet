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

    [Fact(Skip = "Requires review")]
    public async Task GetPeerCounts()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);

        // Act
        var returnValue = await Crawler.GetPeerCounts(cts.Token);

        // Assert
        Assert.NotNull(returnValue);
    }

    [Fact(Skip = "Requires review")]
    public async Task GetIPs()
    {
        // Arrange
        using var cts = new CancellationTokenSource(15000);
        DateTime after = DateTime.Now;
        Int32 offset = 0;
        Int32 limit = 0;

        // Act
        var (ips, total) = await Crawler.GetIPs(after: after, offset: offset, limit: limit, cancellationToken: cts.Token);

        // Assert
        Assert.NotNull(ips);
    }

}
