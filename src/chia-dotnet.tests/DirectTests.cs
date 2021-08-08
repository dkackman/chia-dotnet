using System;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace chia.dotnet.tests
{
    [TestClass]
    public class DirectTests
    {
        [TestMethod]
        [TestCategory("Integration")]
        public async Task GetConnectionsDirect()
        {
            try
            {
                using var rpcClient = Factory.CreateDirectRpcClientFromHardcodedLocation();
                var fullNode = new FullNodeProxy(rpcClient, "unit_tests");

                var connections = await fullNode.GetConnections();
                Assert.IsNotNull(connections);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [TestMethod]
        [TestCategory("Integration")]
        public async Task GetBlockchainStateDirect()
        {
            try
            {
                using var rpcClient = Factory.CreateDirectRpcClientFromHardcodedLocation();
                var fullNode = new FullNodeProxy(rpcClient, "unit_tests");

                var state = await fullNode.GetBlockchainState();
                Assert.IsNotNull(state);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [TestMethod]
        [TestCategory("Integration")]
        [Ignore]
        public async Task ConnectDirectlyUsingConfigEndpoint()
        {
            try
            {
                var config = Config.Open();
                var endpoint = config.GetEndpoint("full_node");

                using var rpcClient = new HttpRpcClient(endpoint);
                var fullNode = new FullNodeProxy(rpcClient, "unit_tests");

                var state = await fullNode.GetBlockchainState();
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }
    }
}
