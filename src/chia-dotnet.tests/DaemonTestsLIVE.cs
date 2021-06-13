using System.Threading;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace chia.dotnet.tests
{
    /// <summary>
    /// This class is a test harness for interation with an actual daemon instance
    /// </summary>
    [TestClass]
    public class DaemonTestsLIVE
    {
        [TestMethod]
        public async Task GetFarmerIsRunningOnUIDaemon()
        {
            using Daemon daemon = new Daemon(Config.Open().GetEndpoint("ui"), "unit_tests");

            await daemon.ConnectAsync(CancellationToken.None);
            bool running = await daemon.IsServiceRunning(ServiceNames.Farmer, CancellationToken.None);

            Assert.IsTrue(running);
        }

        [TestMethod]
        public async Task GetHarvesterIsRunningOnLocalDaemon()
        {
            using Daemon daemon = new Daemon(Config.Open().GetEndpoint("daemon"), "unit_tests");

            await daemon.ConnectAsync(CancellationToken.None);
            bool running = await daemon.IsServiceRunning(ServiceNames.Harvester, CancellationToken.None);

            Assert.IsTrue(running);
        }
    }
}
