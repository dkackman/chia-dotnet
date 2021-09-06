using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace chia.dotnet.tests
{
    [TestClass]
    public class ConversionTests
    {
        [TestMethod]
        public void OnTrillion()
        {
            Assert.AreEqual(Extensions.OneTrillion, Convert.ToDecimal(Math.Pow(10, 12)));
        }

        [TestMethod]
        public void MojoToChiaToMojo()
        {
            ulong mojo = 100;
            var chia = mojo.ToChia();
            var newMojo = chia.ToMojo();

            Assert.AreEqual(mojo, newMojo);
        }

        [TestMethod]
        public void MojoToChiaToMojoFractional()
        {
            ulong mojo = 1001;
            var chia = mojo.ToChia();
            var newMojo = chia.ToMojo();

            Assert.AreEqual(mojo, newMojo);
        }

        [TestMethod]
        public void ChiaToMojoToChia()
        {
            decimal chia = 100;
            var mojo = chia.ToMojo();
            var newChia = mojo.ToChia();

            Assert.AreEqual(chia, newChia);
        }


        [TestMethod]
        public void ChiaToMojoToChiaFractional()
        {
            var chia = 100.1M;
            var mojo = chia.ToMojo();
            var newChia = mojo.ToChia();

            Assert.AreEqual(chia, newChia);
        }
    }
}
