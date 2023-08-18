using System;
using System.IO;
using System.Threading.Tasks;

using Xunit;

namespace chia.dotnet.tests
{
    public class ConnectionSigningTests
    {
        [Fact]
        public async Task InvalidCertPathThrowsFileNotFound()
        {
            // Arrange
            var endpoint = new EndpointInfo()
            {
                Uri = new Uri("wss://localhost:58444"),
                CertPath = "",
                KeyPath = ""
            };
            using var rpc = new WebSocketRpcClient(endpoint);

            // Assert
            _ = await Assert.ThrowsAsync<FileNotFoundException>(
                async () => await rpc.Connect());
        }
    }
}
