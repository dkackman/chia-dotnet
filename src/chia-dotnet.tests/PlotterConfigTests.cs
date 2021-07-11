using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace chia.dotnet.tests
{
    [TestClass]
    public class PlotterConfigTests
    {
        [TestMethod()]
        public void ServiceNameIsIncluded()
        {
            var config = new PlotterConfig()
            {
                DestinationDir = "C:\tmp",
                TempDir = @"C:\tmp",
                Size = KValues.K32
            };

            dynamic serializable = config.PrepareForSerialization();

            Assert.AreEqual(serializable.service, ServiceNames.Plotter);
        }

        [TestMethod()]
        public void SizeSerializesAsInteger()
        {
            var config = new PlotterConfig()
            {
                DestinationDir = "C:\tmp",
                TempDir = @"C:\tmp",
                Size = KValues.K32
            };

            dynamic serializable = config.PrepareForSerialization();

            Assert.AreEqual(serializable.k, 32);
        }

        [TestMethod()]
        public void Number_SerializesAs_n()
        {
            var config = new PlotterConfig()
            {
                DestinationDir = "C:\tmp",
                TempDir = @"C:\tmp",
                Number = 10
            };

            dynamic serializable = config.PrepareForSerialization();

            Assert.AreEqual(serializable.n, 10);
        }

        [TestMethod()]
        public void T2DefaultsToTIfNotPresent()
        {
            var config = new PlotterConfig()
            {
                DestinationDir = "C:\tmp",
                TempDir = @"C:\tmp"
            };

            dynamic serializable = config.PrepareForSerialization();

            Assert.AreEqual(serializable.t2, @"C:\tmp");
        }
    }
}
