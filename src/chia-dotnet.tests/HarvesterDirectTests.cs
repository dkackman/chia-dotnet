using System;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace chia.dotnet.tests
{
    [TestClass]
    public class HarvesterDirectTests
    {
        [TestMethod]
        [TestCategory("Integration")]
        [Ignore]
        public async Task ConnectDirectlyToHarvester()
        {
            try
            {
                var endpoint = new EndpointInfo()
                {
                    Uri = new System.Uri("https://172.26.208.64:8555"),
                    CertPath = @"\\wsl$/Ubuntu-20.04/home/don/.chia/mainnet/config/ssl/full_node/private_full_node.crt",
                    KeyPath = @"\\wsl$/Ubuntu-20.04/home/don/.chia/mainnet/config/ssl/full_node/private_full_node.key",
                };

                using var rpcClient = new HttpRpcClient(endpoint);
                
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }
    }
}
