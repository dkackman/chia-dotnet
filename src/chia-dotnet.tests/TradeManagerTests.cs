using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System;
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
            try
            {
                using var cts = new CancellationTokenSource(20000);

                var rpcClient = Factory.CreateRpcClientFromHardcodedLocation();
                await rpcClient.Connect(cts.Token);

                var daemon = new DaemonProxy(rpcClient, "unit_tests");
                await daemon.RegisterService(cts.Token);

                var wallet = new WalletProxy(rpcClient, "unit_tests");
                _ = await wallet.LogIn(cts.Token);

                _theTradeManager = new TradeManager(wallet);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.Print(e.Message);
            }
        }

        [ClassCleanup()]
        public static void ClassCleanup()
        {
            _theTradeManager.WalletProxy.RpcClient.Dispose();
        }

        [TestMethod()]
        public async Task GetAllOffers()
        {
            using var cts = new CancellationTokenSource(2000);
            var offers = await _theTradeManager.GetOffers(cancellationToken: cts.Token);
            Assert.IsNotNull(offers);
        }

        [TestMethod()]
        public async Task GetOffersCount()
        {
            using var cts = new CancellationTokenSource(2000);
            var counts = await _theTradeManager.GetOffersCount(cts.Token);
            Assert.IsNotNull(counts);
        }

        [TestMethod()]
        public async Task GetCATList()
        {
            using var cts = new CancellationTokenSource(2000);
            var cats = await _theTradeManager.GetOffersCount(cts.Token);
            Assert.IsNotNull(cats);
        }

        [TestMethod()]
        //[Ignore("CAUTION")]
        public async Task CreateOfferForIds()
        {
            using var cts = new CancellationTokenSource(15000);

            var ids = new Dictionary<uint, ulong>()
            {
                { 1, 1 },
                { 2, 1 }
            };
            var offer = await _theTradeManager.CreateOffer(ids, 1, true, cts.Token);

            Assert.IsNotNull(offer);
        }
    }
}
