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
    }
}
