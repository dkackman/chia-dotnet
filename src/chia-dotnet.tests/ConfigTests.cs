using System.IO;

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

        [TestMethod]
        [ExpectedException(typeof(DirectoryNotFoundException))]
        public void InvalidFolderThrowsDirectoryNotFound()
        {
            _ = Config.Open(@"C:\this\path\does\not\exist\config.yaml");
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void InvalidFilenameThrowsFileNotFound()
        {
            _ = Config.Open(@"C:\windows\config.yaml");
        }
    }
}
