// using System.Threading;
// using System.Threading.Tasks;
// using System;
//
// using Microsoft.VisualStudio.TestTools.UnitTesting;
//
// namespace chia.dotnet.tests
// {
//     [TestClass]
//     [TestCategory("Integration")]
//     //[Ignore("Needs a CAT wallet")]
//     public class CATWalletTests
//     {
//         private static CATWallet _theWallet;
//
//         [ClassInitialize]
//         public static async Task Initialize(TestContext context)
//         {
//             using var cts = new CancellationTokenSource(30000);
//             var rpcClient = Factory.CreateWebsocketClient();
//             await rpcClient.Connect(cts.Token);
//
//             var daemon = new DaemonProxy(rpcClient, "unit_tests");
//             await daemon.RegisterService();
//
//             var walletProxy = new WalletProxy(rpcClient, "unit_tests");
//
//             _ = await walletProxy.LogIn(cts.Token);
//
//             // IMPORTANT
//             // SET this wallet ID to a coloroured coin wallet 
//             _theWallet = new CATWallet(2, walletProxy);
//         }
//
//         [ClassCleanup()]
//         public static void ClassCleanup()
//         {
//             _theWallet.WalletProxy.RpcClient?.Dispose();
//         }
//
//         [TestMethod()]
//         public async Task GetName()
//         {
//             using var cts = new CancellationTokenSource(20000);
//
//             var name = await _theWallet.GetName(cts.Token);
//
//             Assert.IsNotNull(name);
//         }
//
//         [TestMethod()]
//         public async Task SetName()
//         {
//             using var cts = new CancellationTokenSource(20000);
//
//             var originalName = await _theWallet.GetName(cts.Token);
//
//             var newName = Guid.NewGuid().ToString();
//             await _theWallet.SetName(newName, cts.Token);
//
//             var name = await _theWallet.GetName(cts.Token);
//
//             Assert.AreEqual(newName, name);
//             await _theWallet.SetName(originalName, cts.Token);
//         }
//
//
//         [TestMethod()]
//         public async Task GetAssetId()
//         {
//             using var cts = new CancellationTokenSource(20000);
//
//             var id = await _theWallet.GetAssetId(cts.Token);
//
//             Assert.IsNotNull(id);
//         }
//     }
// }
