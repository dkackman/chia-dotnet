// using chia.dotnet.bech32;
//
// using Microsoft.VisualStudio.TestTools.UnitTesting;
//
// namespace chia.dotnet.tests
// {
//     [TestClass]
//     public class Bech32Tests
//     {
//         [TestMethod]
//         public void PuzzleHashToAddress()
//         {
//             var bech32m = new Bech32M("xch");
//             var address = bech32m.PuzzleHashToAddress("0xdb96f26f5245baec3c4e06da21d9d50c8718f9920b5e9354c3a936fc0ee49a66");
//
//             Assert.AreEqual("xch1mwt0ym6jgkawc0zwqmdzrkw4pjr337vjpd0fx4xr4ym0crhynfnq96pztp", address);
//         }
//
//         [TestMethod]
//         public void AddressToPuzzleHash()
//         {
//             var hash = Bech32M.AddressToPuzzleHashString("xch1mwt0ym6jgkawc0zwqmdzrkw4pjr337vjpd0fx4xr4ym0crhynfnq96pztp");
//
//             Assert.AreEqual("db96f26f5245baec3c4e06da21d9d50c8718f9920b5e9354c3a936fc0ee49a66", hash);
//         }
//
//         [TestMethod]
//         public void ZeroXPrefixIsIgnored()
//         {
//             var bech32m = new Bech32M("xch");
//             var addressWithPrefix = bech32m.PuzzleHashToAddress("0xdb96f26f5245baec3c4e06da21d9d50c8718f9920b5e9354c3a936fc0ee49a66");
//             var addressWithoutPrefix = bech32m.PuzzleHashToAddress("db96f26f5245baec3c4e06da21d9d50c8718f9920b5e9354c3a936fc0ee49a66");
//
//             Assert.AreEqual(addressWithPrefix, addressWithoutPrefix);
//         }
//     }
// }
