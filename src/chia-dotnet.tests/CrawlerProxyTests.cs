// using System;
// using System.Threading;
// using System.Threading.Tasks;
//
// using Microsoft.VisualStudio.TestTools.UnitTesting;
//
// namespace chia.dotnet.tests
// {
//     [TestClass]
//     [TestCategory("Integration")]
//     public class CrawlerProxyTests
//     {
//         private static CrawlerProxy _theCrawler;
//
//         [ClassInitialize]
//         public static async Task Initialize(TestContext context)
//         {
//             try
//             {
//                 var rpcClient = Factory.CreateDirectRpcClientFromHardcodedLocation(8561, "crawler");
//
//                 await Task.CompletedTask;
//                 _theCrawler = new CrawlerProxy(rpcClient, "unit_tests");
//             }
//             catch (Exception e)
//             {
//                 Assert.Fail(e.Message);
//             }
//         }
//
//         [ClassCleanup()]
//         public static void ClassCleanup()
//         {
//             _theCrawler.RpcClient?.Dispose();
//         }
//
//         [TestMethod()]
//         public async Task GetPeerCounts()
//         {
//             using var cts = new CancellationTokenSource(20000);
//
//             var counts = await _theCrawler.GetPeerCounts(cts.Token);
//
//             Assert.IsNotNull(counts);
//         }
//
//         [TestMethod()]
//         public async Task GetIPs()
//         {
//             using var cts = new CancellationTokenSource(20000);
//
//             var ips = await _theCrawler.GetIPs(DateTime.Now - TimeSpan.FromDays(2), cancellationToken: cts.Token);
//
//             Assert.IsNotNull(ips);
//         }
//     }
// }
