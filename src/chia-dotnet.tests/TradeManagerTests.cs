using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace chia.dotnet.tests
{
    [TestClass]
    [TestCategory("Integration")]
    public class TradeManagerTest
    {
        private static TradeManager _theTradeManager;

        [ClassInitialize]
        public static async Task Initialize(TestContext context)
        {
            using var cts = new CancellationTokenSource();

            var rpcClient = Factory.CreateRpcClientFromHardcodedLocation();
            await rpcClient.Connect(cts.Token);

            var daemon = new DaemonProxy(rpcClient, "unit_tests");
            await daemon.RegisterService(cts.Token);

            var wallet = new WalletProxy(rpcClient, "unit_tests");
            _ = await wallet.LogIn(1531304830, cts.Token);

            _theTradeManager = new TradeManager(wallet);
        }

        [ClassCleanup()]
        public static void ClassCleanup()
        {
            _theTradeManager.WalletProxy.RpcClient.Dispose();
        }

        [TestMethod()]
        public async Task GetCATList()
        {
            var cats = await _theTradeManager.GetCATList();
            Assert.IsNotNull(cats);
        }

        [TestMethod()]
        //[Ignore("CAUTION")]
        public async Task CreateOfferForIds()
        {
            using var cts = new CancellationTokenSource(15000);

            var ids = new Dictionary<string, int>()
            {
                { "1", 1 }
            };
            var offer = await _theTradeManager.CreateOfferForIds(ids, 1, true, cts.Token);

            Assert.IsNotNull(offer);
        }

        [TestMethod()]
        [Ignore("CAUTION")]
        public async Task GetDiscrepenciesForOffer()
        {
            using var cts = new CancellationTokenSource(15000);

            var discrepencies = await _theTradeManager.GetDiscrepenciesForOffer(@"C:\tmp\test.offer", cts.Token);

            Assert.IsNotNull(discrepencies);
        }

    }
}
