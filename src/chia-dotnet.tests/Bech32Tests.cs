using chia.dotnet.bech32;
using chia.dotnet.tests.Core;
using Xunit;

namespace chia.dotnet.tests
{
    public class Bech32Tests : TestBase
    {
        public Bech32Tests(ChiaDotNetFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public void PuzzleHashToAddress()
        {
            // Arrange
            var bech32m = new Bech32M("xch");

            // Act
            var address = bech32m.PuzzleHashToAddress("0xdb96f26f5245baec3c4e06da21d9d50c8718f9920b5e9354c3a936fc0ee49a66");

            // Assert
            Assert.Equal("xch1mwt0ym6jgkawc0zwqmdzrkw4pjr337vjpd0fx4xr4ym0crhynfnq96pztp", address);
        }

        [Fact]
        public void AddressToPuzzleHash()
        {
            // Arrange

            // Act
            var hash = Bech32M.AddressToPuzzleHashString("xch1mwt0ym6jgkawc0zwqmdzrkw4pjr337vjpd0fx4xr4ym0crhynfnq96pztp");

            // Assert
            Assert.Equal("db96f26f5245baec3c4e06da21d9d50c8718f9920b5e9354c3a936fc0ee49a66", hash);
        }

        [Fact]
        public void ZeroXPrefixIsIgnored()
        {
            // Arrange
            var bech32m = new Bech32M("xch");

            // Act
            var addressWithPrefix = bech32m.PuzzleHashToAddress("0xdb96f26f5245baec3c4e06da21d9d50c8718f9920b5e9354c3a936fc0ee49a66");
            var addressWithoutPrefix = bech32m.PuzzleHashToAddress("db96f26f5245baec3c4e06da21d9d50c8718f9920b5e9354c3a936fc0ee49a66");

            // Assert
            Assert.Equal(addressWithPrefix, addressWithoutPrefix);
        }
    }
}
