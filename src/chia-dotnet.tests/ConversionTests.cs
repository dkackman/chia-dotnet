using System;
using chia.dotnet.tests.Core;
using Xunit;


namespace chia.dotnet.tests
{
    public class ConversionTests : TestBase
    {
        public ConversionTests(ChiaDotNetFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public void OneTrillion()
        {
            // Arrange 

            // Act

            // Assert
            Assert.Equal(Extensions.OneTrillion, Convert.ToDecimal(Math.Pow(10, 12)));
        }

        [Fact]
        public void MojoToChiaToMojo()
        {
            // Arrange
            ulong mojo = 100;

            // Act
            var chia = mojo.ToChia();
            var newMojo = chia.ToMojo();

            // Assert
            Assert.Equal(mojo, newMojo);
        }

        [Fact]
        public void MojoToChiaToMojoFractional()
        {
            // Arrange
            ulong mojo = 1001;

            // Act
            var chia = mojo.ToChia();
            var newMojo = chia.ToMojo();

            // Assert
            Assert.Equal(mojo, newMojo);
        }

        [Fact]
        public void ChiaToMojoToChia()
        {
            // Arrange
            decimal chia = 100;

            // Act
            var mojo = chia.ToMojo();
            var newChia = mojo.ToChia();

            // Assert
            Assert.Equal(chia, newChia);
        }

        [Fact]
        public void ChiaToMojoToChiaFractional()
        {
            // Arrange
            var chia = 100.1M;

            // Act
            var mojo = chia.ToMojo();
            var newChia = mojo.ToChia();

            // Assert
            Assert.Equal(chia, newChia);
        }

        [Fact]
        public void CalculateCoinName()
        {
            // https://developers.chia.net/t/create-coin-id-using-c/487
            // Arrange
            var coin = new Coin()
            {
                ParentCoinInfo = "0101a581331ee609668229b75a08fb6f3bb32abf15349acc11c17c2072643a8d",
                PuzzleHash = "eff07522495060c066f66f32acc2a77e3a3e737aca8baea4d1a64ea4cdc13da9",
                Amount = 1023,
            };

            // Act
            var coinName = coin.Name;

            // Assert
            Assert.Equal("2DEFA04144FEF9C90598CBA104F36E2922C1B0C99220223780D6DA561B874A7A", coinName);
        }
    }
}
