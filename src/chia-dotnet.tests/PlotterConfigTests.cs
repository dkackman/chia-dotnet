using Xunit;

namespace chia.dotnet.tests
{
    public class PlotterConfigTests
    {
        [Fact]
        public void ServiceNameIsIncluded()
        {
            var config = new PlotterConfig()
            {
                DestinationDir = "C:\tmp",
                TempDir = @"C:\tmp",
                Size = KSize.K32
            };

            dynamic serializable = config.PrepareForSerialization();

            Assert.Equal(serializable.service, ServiceNames.Plotter);
        }

        [Fact]
        public void SizeSerializesAsInteger()
        {
            var config = new PlotterConfig()
            {
                DestinationDir = "C:\tmp",
                TempDir = @"C:\tmp",
                Size = KSize.K32
            };

            dynamic serializable = config.PrepareForSerialization();

            Assert.Equal(serializable.k, 32);
        }

        [Fact]
        public void Number_SerializesAs_n()
        {
            var config = new PlotterConfig()
            {
                DestinationDir = "C:\tmp",
                TempDir = @"C:\tmp",
                Number = 10
            };

            dynamic serializable = config.PrepareForSerialization();

            Assert.Equal(serializable.n, 10);
        }

        [Fact]
        public void T2DefaultsToTIfNotPresent()
        {
            var config = new PlotterConfig()
            {
                DestinationDir = "C:\tmp",
                TempDir = @"C:\tmp"
            };

            dynamic serializable = config.PrepareForSerialization();

            Assert.Equal(serializable.t2, @"C:\tmp");
        }
    }
}
