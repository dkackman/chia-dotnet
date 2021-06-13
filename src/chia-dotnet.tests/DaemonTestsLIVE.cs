using System.Threading;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace chia.dotnet.tests
{
    /// <summary>
    /// This class is a test harness for interation with an actual daemon instance
    /// </summary>
    [TestClass]
    [TestCategory("Integration")]
    //[Ignore] // uncomment to suppress completely
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

        [TestMethod]
        public async Task ExitLocalDaemon()
        {
            using Daemon daemon = new Daemon(Config.Open().GetEndpoint("daemon"), "unit_tests");

            await daemon.ConnectAsync(CancellationToken.None);
            await daemon.Exit(CancellationToken.None);       

            // if no exception the daemon was stopped successfully
        }

        [TestMethod]
        public async Task StartAndStopFarmerOnLocalDaemon()
        {
            using Daemon daemon = new Daemon(Config.Open().GetEndpoint("daemon"), "unit_tests");

            await daemon.ConnectAsync(CancellationToken.None);

            Assert.IsFalse(await daemon.IsServiceRunning(ServiceNames.Farmer, CancellationToken.None));

            await daemon.StartService(ServiceNames.Farmer, CancellationToken.None);
            Assert.IsTrue(await daemon.IsServiceRunning(ServiceNames.Farmer, CancellationToken.None));
            
            await daemon.StopService(ServiceNames.Farmer, CancellationToken.None);
            Assert.IsFalse(await daemon.IsServiceRunning(ServiceNames.Farmer, CancellationToken.None));

            // if no exception the daemon was stopped successfully
        }


        [TestMethod]
        public async Task RegisterService()
        {
            using Daemon daemon = new Daemon(Config.Open().GetEndpoint("daemon"), "unit_tests");

            await daemon.ConnectAsync(CancellationToken.None);

            await daemon.RegisterService("unit_tests", CancellationToken.None);

            // no exception we were successful
        }
    }
}
