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

                var rpcClient = Factory.CreateDaemon();
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
            var cats = await _theTradeManager.GetCATList(cts.Token);
            Assert.IsNotNull(cats);
        }

        [TestMethod()]
        //[Ignore("CAUTION")]
        public async Task CreateOfferForIds()
        {
            using var cts = new CancellationTokenSource(15000);

            // it is hard to tell from the chia code how this works. The dictionary key is a wallet id
            // I think the postive values are the requested amounts
            // and things with negative values are the offered amounts
            // 
            // see TradeManager.py _create_offer_for_ids around line 275
            // im not quite sure whar unit of measure the value is in
            // MOJO for xch i assume. not sure about CATs
            var ids = new Dictionary<uint, long>()
            {
                { 1, 1 },
                { 2, -1 }
            };
            var offer = await _theTradeManager.CreateOffer(ids, 1, true, cts.Token);

            Assert.IsNotNull(offer);
        }
    }
}
