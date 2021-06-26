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
    public class DaemonTests
    {
        [TestMethod]
        public async Task GetFarmerIsRunningOnUIDaemon()
        {
            using var daemon = Factory.CreateDaemon();

            await daemon.Connect(CancellationToken.None);
            var running = await daemon.IsServiceRunning(ServiceNames.Farmer, CancellationToken.None);

            Assert.IsTrue(running);
        }

        [TestMethod]
        public async Task GetHarvesterIsRunningOnLocalDaemon()
        {
            using var daemon = Factory.CreateDaemon();

            await daemon.Connect(CancellationToken.None);
            var running = await daemon.IsServiceRunning(ServiceNames.Harvester, CancellationToken.None);

            Assert.IsTrue(running);
        }

        [TestMethod]
        [Ignore]
        public async Task ExitLocalDaemon()
        {
            using var daemon = Factory.CreateDaemon();

            await daemon.Connect(CancellationToken.None);
            await daemon.Exit(CancellationToken.None);

            // if no exception the daemon was stopped successfully
        }

        [TestMethod]
        [Ignore]
        public async Task StartAndStopFarmerOnLocalDaemon()
        {
            using var daemon = Factory.CreateDaemon();

            await daemon.Connect(CancellationToken.None);

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
            using var daemon = Factory.CreateDaemon();

            await daemon.Connect(CancellationToken.None);

            await daemon.RegisterService(daemon.OriginService, CancellationToken.None);

            // no exception we were successful
        }
    }
}
