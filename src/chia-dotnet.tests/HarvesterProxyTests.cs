using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using chia.dotnet.tests.Core;
using Xunit;

namespace chia.dotnet.tests
{
    public class HarvesterProxyTests : TestBase
    {
        public HarvesterProxyTests(ChiaDotNetFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task GetPlots()
        {
            using var cts = new CancellationTokenSource(15000);

            var (FailedToOpenFilenames, NotFoundFileNames, Plots) = await Harvester.GetPlots(cts.Token);

            Assert.NotNull(FailedToOpenFilenames);
            Assert.NotNull(NotFoundFileNames);
            Assert.NotNull(Plots);
        }

        [Fact(Skip = "Destructive")]
        public async Task DeletePlot()
        {
            using var cts = new CancellationTokenSource(15000);

            await Harvester.DeletePlot("<plot name>", cts.Token);
        }

        [Fact]
        public async Task GetPlotDirectories()
        {
            using var cts = new CancellationTokenSource(15000);

            var directories = await Harvester.GetPlotDirectories(cts.Token);

            Assert.NotNull(directories.ToList());
        }

        [Fact]
        public async Task AddPlotDirectory()
        {
            using var cts = new CancellationTokenSource(15000);

            await Harvester.AddPlotDirectory(System.IO.Directory.GetCurrentDirectory(), cts.Token);
        }


        [Fact]
        public async Task RemovePlotDirectory()
        {
            using var cts = new CancellationTokenSource(15000);

            await Harvester.RemovePlotDirectory(System.IO.Directory.GetCurrentDirectory(), cts.Token);
        }

        [Fact]
        public async Task RefreshPlots()
        {
            using var cts = new CancellationTokenSource(15000);

            await Harvester.RefreshPlots(cts.Token);
        }
    }
}
