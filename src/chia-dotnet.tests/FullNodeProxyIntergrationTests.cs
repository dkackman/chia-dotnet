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
    public class FullNodeProxyIntergrationTests
    {
        [TestMethod]
        public async Task GetBlockChainState()
        {
            using Daemon daemon = new Daemon(Config.Open().GetEndpoint("ui"), "unit_tests");

            await daemon.Connect(CancellationToken.None);
            await daemon.Register(CancellationToken.None);

            var fullNode = new FullNodeProxy(daemon);
            var state = await fullNode.GetBlockchainState(CancellationToken.None);

            Assert.IsNotNull(state);

            // no exception we were successful
        }
    }
}
