using System;
using System.Diagnostics;
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
        }

        [TestMethod]
        public async Task GetBlock()
        {
            using Daemon daemon = new Daemon(Config.Open().GetEndpoint("ui"), "unit_tests");

            await daemon.Connect(CancellationToken.None);
            await daemon.Register(CancellationToken.None);

            var fullNode = new FullNodeProxy(daemon);
            var block = await fullNode.GetBlock("0xc5d6292aaf50c3cdc3f8481a30b2e9f12babf274c0488ab24db3dd9b1dd41364", CancellationToken.None);

            Assert.IsNotNull(block);
        }

        [TestMethod]
        public async Task GetBlockRecord()
        {
            using Daemon daemon = new Daemon(Config.Open().GetEndpoint("ui"), "unit_tests");

            await daemon.Connect(CancellationToken.None);
            await daemon.Register(CancellationToken.None);

            var fullNode = new FullNodeProxy(daemon);
            var record = await fullNode.GetBlockRecord("0xc5d6292aaf50c3cdc3f8481a30b2e9f12babf274c0488ab24db3dd9b1dd41364", CancellationToken.None);

            Assert.IsNotNull(record);
        }

        [TestMethod]
        public async Task GetBlocks()
        {
            using Daemon daemon = new Daemon(Config.Open().GetEndpoint("ui"), "unit_tests");

            await daemon.Connect(CancellationToken.None);
            await daemon.Register(CancellationToken.None);

            var fullNode = new FullNodeProxy(daemon);
            var blocks = await fullNode.GetBlocks(435160, 435167, CancellationToken.None);

            Assert.IsNotNull(blocks);
        }

        [TestMethod]
        public async Task GetNetworkSpace()
        {
            using Daemon daemon = new Daemon(Config.Open().GetEndpoint("ui"), "unit_tests");

            await daemon.Connect(CancellationToken.None);
            await daemon.Register(CancellationToken.None);

            var fullNode = new FullNodeProxy(daemon);
            var space = await fullNode.GetNetworkSpace("0x457649e7e6dabb5660f8c3cd9e08534522361d97cb237bdfa341bce01e91c3f5", "0x1353f4d1a01d5393cb7f8f0631487774e11125f47af487426fd9cbcd24151a15", CancellationToken.None);
            Assert.IsNotNull(space);

            Debug.WriteLine(space);
        }

        [TestMethod]
        public async Task Ping()
        {
            using Daemon daemon = new Daemon(Config.Open().GetEndpoint("ui"), "unit_tests");

            await daemon.Connect(CancellationToken.None);
            await daemon.Register(CancellationToken.None);

            var fullNode = new FullNodeProxy(daemon);
            await fullNode.Ping(CancellationToken.None);
        }

        [TestMethod]
        public async Task GetConnections()
        {
            using Daemon daemon = new Daemon(Config.Open().GetEndpoint("ui"), "unit_tests");

            await daemon.Connect(CancellationToken.None);
            await daemon.Register(CancellationToken.None);

            var fullNode = new FullNodeProxy(daemon);
            var connections = await fullNode.GetConnections(CancellationToken.None);
            Assert.IsNotNull(connections);
        }
    }
}
