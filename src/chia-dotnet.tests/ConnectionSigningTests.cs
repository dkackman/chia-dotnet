using System;
using System.IO;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace chia.dotnet.tests
{
    [TestClass]
    public class ConnectionSigningTests
    {
        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public async Task InvalidCertPathThrowsFileNotFound()
        {
            var endpoint = new EndpointInfo()
            {
                Uri = new Uri("wss://localhost:58444"),
                CertPath = "",
                KeyPath = ""
            };
            using var daemon = new Daemon(endpoint, "unit_tests");

            await daemon.Connect();
        }
    }
}
