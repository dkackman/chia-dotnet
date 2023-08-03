﻿// using System;
// using System.Linq;
// using System.Threading;
// using System.Threading.Tasks;
//
// using Microsoft.VisualStudio.TestTools.UnitTesting;
//
// namespace chia.dotnet.tests
// {
//     /// <summary>
//     /// This class is a test harness for interation with an actual daemon instance
//     /// </summary>
//     [TestClass]
//     [TestCategory("Integration")]
//     public class FarmerProxyTests
//     {
//         private static FarmerProxy _theFarmer;
//
//         [ClassInitialize]
//         public static async Task Initialize(TestContext context)
//         {
//             using var cts = new CancellationTokenSource(2000);
//
//             var rpcClient = Factory.CreateWebsocketClient();
//             await rpcClient.Connect(cts.Token);
//
//             var daemon = new DaemonProxy(rpcClient, "unit_tests");
//             await daemon.RegisterService(cts.Token);
//
//             _theFarmer = new FarmerProxy(rpcClient, "unit_tests");
//         }
//
//         [ClassCleanup()]
//         public static void ClassCleanup()
//         {
//             _theFarmer.RpcClient?.Dispose();
//         }
//
//         [TestMethod]
//         public async Task GetRewardTargets()
//         {
//             using var cts = new CancellationTokenSource(15000);
//
//             var targets = await _theFarmer.GetRewardTargets(cts.Token);
//
//             Assert.IsNotNull(targets);
//             Assert.IsFalse(string.IsNullOrEmpty(targets.FarmerTarget));
//             Assert.IsFalse(string.IsNullOrEmpty(targets.PoolTarget));
//         }
//
//         [TestMethod]
//         [TestCategory("CAUTION")]
//         [Ignore("CAUTION")]
//         public async Task SetRewardTargets()
//         {
//             using var cts = new CancellationTokenSource(15000);
//
//             // this will change the state of the farmer - make sure you want to do this
//             // fill in addresses for target and pool as appropriate
//             await _theFarmer.SetRewardTargets("txch1pacgsfkngcrw50pnuvgvak0qpt8mx9pmey2uxl6p65c9727lhc0sgnklt4", "txch1pacgsfkngcrw50pnuvgvak0qpt8mx9pmey2uxl6p65c9727lhc0sgnklt4");
//         }
//
//         [TestMethod]
//         public async Task GetSignagePoints()
//         {
//             using var cts = new CancellationTokenSource(15000);
//
//             var signagePoints = await _theFarmer.GetSignagePoints(cts.Token);
//
//             foreach (var sp in signagePoints)
//             {
//                 if (sp.Proofs.Any())
//                 {
//                     System.Diagnostics.Debug.WriteLine("here");
//                 }
//             }
//
//             Assert.IsNotNull(signagePoints);
//         }
//
//         [TestMethod]
//         public async Task GetHarvestersSummary()
//         {
//             using var cts = new CancellationTokenSource(15000);
//
//             var summaries = await _theFarmer.GetHarvestersSummary(cts.Token);
//
//             Assert.IsNotNull(summaries);
//         }
//
//         [TestMethod]
//         public async Task GetHarvesterPlotsValid()
//         {
//             using var cts = new CancellationTokenSource(15000);
//             var summaries = await _theFarmer.GetHarvestersSummary(cts.Token);
//             var nodeId = summaries.First().Connection.NodeId;
//             var requestDatata = new PlotInfoRequestData()
//             {
//                 NodeId = nodeId,
//                 PageSize = 10,
//             };
//
//             var plotInfo = await _theFarmer.GetHarvesterPlotsValid(requestDatata, cts.Token);
//
//             Assert.IsNotNull(plotInfo);
//         }
//
//         [TestMethod]
//         public async Task GetHarvesterPlotsKeysMissing()
//         {
//             using var cts = new CancellationTokenSource(15000);
//             var summaries = await _theFarmer.GetHarvestersSummary(cts.Token);
//             var nodeId = summaries.First().Connection.NodeId;
//             var requestDatata = new PlotPathRequestData()
//             {
//                 NodeId = nodeId,
//                 PageSize = 10,
//             };
//
//             var plotInfo = await _theFarmer.GetHarvesterPlotsKeysMissing(requestDatata, cts.Token);
//
//             Assert.IsNotNull(plotInfo);
//         }
//
//         [TestMethod]
//         public async Task GetSignagePoint()
//         {
//             using var cts = new CancellationTokenSource(1005000);
//
//             var signagePoints = await _theFarmer.GetSignagePoints(cts.Token);
//
//             try
//             {
//                 foreach (var spInfo in signagePoints)
//                 {
//                     var sp = await _theFarmer.GetSignagePoint(spInfo.SignagePoint.ChallengeChainSp, cts.Token);
//                     if (sp.Proofs.Any())
//                     {
//                         break;
//                     }
//                     Assert.IsNotNull(sp);
//                 }
//             }
//             catch (InvalidOperationException)
//             {
//                 Assert.Inconclusive("Node has no signage points");
//             }
//         }
//
//         [TestMethod]
//         public async Task GetPoolState()
//         {
//             using var cts = new CancellationTokenSource(150000);
//
//             var state = await _theFarmer.GetPoolState(cts.Token);
//
//             Assert.IsNotNull(state);
//         }
//
//         [TestMethod]
//         public async Task GetPoolLoginLink()
//         {
//             using var cts = new CancellationTokenSource(15000);
//
//             var state = await _theFarmer.GetPoolState(cts.Token);
//             var pool = state.FirstOrDefault();
//             if (pool is not null)
//             {
//                 Assert.IsNotNull(pool.PoolConfig);
//
//                 var link = await _theFarmer.GetPoolLoginLink(pool.PoolConfig.LauncherId, cts.Token);
//
//                 Assert.IsNotNull(link);
//             }
//             else
//             {
//                 Assert.Inconclusive("This node isn't part of a pool");
//             }
//         }
//
//         [TestMethod]
//         public async Task GetHarvesters()
//         {
//             using var cts = new CancellationTokenSource(15000);
//
//             var harvesters = await _theFarmer.GetHarvesters(cts.Token);
//
//             Assert.IsNotNull(harvesters);
//         }
//
//         [TestMethod]
//         public async Task Ping()
//         {
//             using var cts = new CancellationTokenSource(15000);
//
//             await _theFarmer.HealthZ(cts.Token);
//         }
//
//         [TestMethod]
//         public async Task GetConnections()
//         {
//             using var cts = new CancellationTokenSource(15000);
//
//             var connections = await _theFarmer.GetConnections(cts.Token);
//
//             Assert.IsNotNull(connections);
//         }
//
//         [TestMethod]
//         public async Task OpenConnection()
//         {
//             using var cts = new CancellationTokenSource(15000);
//
//             await _theFarmer.OpenConnection("testnet-node.chia.net", 58444, cts.Token);
//         }
//     }
// }
