using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using chia.dotnet.tests.Core;
using Xunit;

namespace chia.dotnet.tests
{
    public class PlotterProxyTests : TestBase
    {
        public PlotterProxyTests(ChiaDotNetFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task FailOnInvalidConfig()
        {
            using var cts = new CancellationTokenSource(15000);

            var config = new PlotterConfig();

            _ = await Assert.ThrowsAsync<InvalidOperationException>(
                async () => await Plotter.StartPlotting(config, cts.Token));
        }

        [Fact]
        public async Task StartStopPlotting()
        {
            using var cts = new CancellationTokenSource(15000);

            var config = new PlotterConfig()
            {
                Size = KSize.K25,
                OverrideK = true,
                TempDir = System.IO.Path.GetTempPath(),
                DestinationDir = System.IO.Path.GetTempPath()
            };

            var ids = await Plotter.StartPlotting(config);
            Assert.NotNull(ids.ToList());

            await Plotter.StopPlotting(id: ids.First(), cancellationToken: cts.Token);
        }

        [Fact]
        public async Task RegisterPlotter()
        {
            using var cts = new CancellationTokenSource(15000);

            var q = await Plotter.RegisterPlotter(cts.Token);

            Assert.NotNull(q.ToList());
        }

        [Fact]
        public async Task GetPlotters()
        {
            using var cts = new CancellationTokenSource(15000);

            var plotters = await Plotter.GetPlotters(cts.Token);

            Assert.NotNull(plotters);
        }
    }
}
