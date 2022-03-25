using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace chia.dotnet.tests
{
    [TestClass]
    [TestCategory("Integration")]
    public class DirectTests
    {
        [TestMethod]
        public async Task GetConnectionsDirect()
        {
            try
            {
                using var cts = new CancellationTokenSource(2000);

                using var rpcClient = Factory.CreateDirectRpcClientFromHardcodedLocation(8555, "full_node");
                var fullNode = new FullNodeProxy(rpcClient, "unit_tests");

                var connections = await fullNode.GetConnections(cts.Token);

                Assert.IsNotNull(connections);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [TestMethod]
        public async Task GetBlockchainStateDirect()
        {
            try
            {
                using var cts = new CancellationTokenSource(15000);

                using var rpcClient = Factory.CreateDirectRpcClientFromHardcodedLocation(8555, "full_node");
                var fullNode = new FullNodeProxy(rpcClient, "unit_tests");

                var state = await fullNode.GetBlockchainState(cts.Token);

                Assert.IsNotNull(state);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [TestMethod]
        [Ignore]
        public async Task ConnectDirectlyUsingConfigEndpoint()
        {
            try
            {
                var config = Config.Open();
                var endpoint = config.GetEndpoint("full_node");

                using var cts = new CancellationTokenSource(15000);

                using var rpcClient = new HttpRpcClient(endpoint);
                var fullNode = new FullNodeProxy(rpcClient, "unit_tests");

                var state = await fullNode.GetBlockchainState(cts.Token);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }
    }
}
