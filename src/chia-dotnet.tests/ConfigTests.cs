using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace chia.dotnet.tests
{
    [TestClass]
    public class ConfigTests
    {
        [TestMethod]
        [TestCategory("Integration")]
        public void CanOpenDefaultConfig()
        {
            var config = Config.Open();

            Assert.IsNotNull(config);
        }
    }
}
