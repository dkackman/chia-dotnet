using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using System.Linq;
using chia.dotnet.tests.Core;

namespace chia.dotnet.tests
{
    public class TradeManagerTests : TestBase
    {
        public TradeManagerTests(ChiaDotNetFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public async Task GetAllOffers()
        {
            using var cts = new CancellationTokenSource(2000);
            var offers = await TradeManager.GetOffers(fileContents: true, cancellationToken: cts.Token);

            // requires at least one open offer
            Assert.NotNull(offers);
            Assert.True(offers.Any(o => !string.IsNullOrWhiteSpace(o.Offer)));
        }

        [Fact]
        public async Task GetOffersCount()
        {
            using var cts = new CancellationTokenSource(2000);
            var counts = await TradeManager.GetOffersCount(cts.Token);

            // requires at least one open offer
            Assert.NotNull(counts);
            Assert.True(counts.Total >= 0);
        }

        [Fact]
        public async Task GetCATList()
        {
            using var cts = new CancellationTokenSource(2000);
            var cats = await TradeManager.GetCATList(cts.Token);
            Assert.NotNull(cats);
        }

        [Fact]
        public async Task CreateOfferForIdsInvalidZeroBalance()
        {
            using var cts = new CancellationTokenSource(15000);

            uint zeroBalanceCoinWalletId = 2;
            uint takerCoinWalletId = 1; // wallet ID for requested coin

            var idsAndAmounts = new Dictionary<uint, long>()
             {
                 { zeroBalanceCoinWalletId, 1 },
                 { takerCoinWalletId, -1 }
             };

            var offer = await TradeManager.CreateOffer(idsAndAmounts, cancellationToken: cts.Token);
        }

        [Fact]
        public async Task CreateOfferForIdsValidateOnly()
        {
            using var cts = new CancellationTokenSource(15000);

            uint makerCoinWalletId = 1; // wallet ID for offered coin (must have balance >= 1)
            uint takerCoinWalletId = 2; // wallet ID for requested coin

            // net amounts in mojo
            var idsAndAmounts = new Dictionary<uint, long>()
             {
                 { makerCoinWalletId, 1 },
                 { takerCoinWalletId, -1 }
             };

            var offer = await TradeManager.CreateOffer(idsAndAmounts, validateOnly: true, cancellationToken: cts.Token);

            Assert.NotNull(offer);
        }

        [Fact]
        public async Task CreateOfferForIds()
        {
            uint makerCoinWalletId = 2; // wallet ID for offered coin (must have balance >= 1)
            uint takerCoinWalletId = 1; // wallet ID for requested coin

            var idsAndAmounts = new Dictionary<uint, long>()
             {
                 { makerCoinWalletId, 1 },
                 { takerCoinWalletId, -1 }
             };

            using var cts = new CancellationTokenSource(15000);
            var offer = await TradeManager.CreateOffer(idsAndAmounts, fee: 1, cancellationToken: cts.Token);

            Assert.NotNull(offer);
        }

        [Fact]
        public async Task CreateOfferFromSummary()
        {
            var makerCoinAssetId = "89dad43c67e91506cb2a05f4ed060fe7be31add0bb656f959dc0778fadac2c4c"; // asset ID for offered coin (must have balance >= 1)
            var takerCoinAssetId = "txch"; // asset ID for requested coin

            var summary = new OfferSummary
            {
                Requested = new Dictionary<string, ulong> { { makerCoinAssetId, 1 } },
                Offered = new Dictionary<string, ulong> { { takerCoinAssetId, 1 } }
            };

            using var cts = new CancellationTokenSource(10000);
            var offer = await TradeManager.CreateOffer(summary, cancellationToken: cts.Token);

            Assert.NotNull(offer);
            Assert.True(offer.TradeRecord.Status == TradeStatus.PENDING_ACCEPT);
        }

        [Fact]
        public async Task CheckOfferValidity()
        {
            // a valid offer requires the actual coins used to create the offer to be un-spent on the blockchain
            var validOffer = "offer1qqp83w76wzru6cmqvpsxygqqwc7hynr6hum6e0mnf72sn7uvvkpt68eyumkhelprk0adeg42nlelk2mpafrgx923m0l47yqsaxujnj67dj4d4xhf0dv5fx4uyw6zxasykmyde0v4a9t096w2f4sdn08qc54gum80kkw9wxuh4mwl5a7et253f0ul0k8r8hryflzkawulhqhy0urlr0pck26tq5vvpahyrslmgj67hhq76shmkc37rjja6u9408mdvnyhe7ha7a3m7wdxqkrjvnpg9r2fh73e404q4j6v8claxnm9r27l002hecyn7uyvklvdq3vjvvtr3zscmvtvqcnfks6c9xsygedj8gadj8gadjrgdvz9uykj456sythkzc2d6wus6nh88e0j4gwex9jvfr443wxw2xdezuuxnmu0fpg9ddhhfhaeqd2av70t3fxl5j63kmf44tzlm6e0z07lv7u0dshw6n72juc0jnm67hdruvpj63y3p0u7rh9tqcdmccgm6whn266lelufcunt78xk0d3mz3jrs0eted585lpl22lnrqdrg5avd39jveacfpxq9mwkj09t9320amvqjc0xqscmypke8lctpq6h8l3hxxllsn6cr8yyegrfxcxh54u6p060vtn5372m88as5kx906knga262kvmkcys50mck34wd98lekxy6wuqffklutucz0arrftavw6r39ch0u70lvl9xuxlalyevzudculm067uvqmt26dgx235l0lsha3ua7l056ydazeex0qj7kp0wwazd6h9wwmjsnnlxgt78muc8a48s2zjpz0cl7p822ttnltvf0065uf0ezfh0rrvte5xcua4f4ufn39pydzc8gakxmlv4mlskr4fnj479v9jxpvllrahapj70rwjgrt0jjkkkeh6t0l78rj6l9pvlll8lzl443z37xh797uvzuwql8uu6nkn0veykz7t8zerv6afamutn8dmwt406aflp3knmhv9szmpz654l305p2zaqgxg3ds9p4zpj9w6yt3j47t4n4rse6a3s5h4n4zkhh27lvphzdl3cxl3et7wr5hfpn89pg6qq8gnrdm4e083054n9fnd3tl6e3d8r74le9gjtdjfvh7mvd2p25dj8gml4adzxvddlv668y40sdmxg5nk2yhd0fq0xt957mxchhvm4mwa94uc2k6nzenj4wraekvt6mu0m4naapkqtx8m7ha6yfewavhjlv3hn5audclkuqqqyh2km7gxmzur4";
            using var cts = new CancellationTokenSource(2000);
            var (Valid, Id) = await TradeManager.CheckOfferValidity(validOffer, cts.Token);

            Assert.True(Valid);
        }

        [Fact]
        public async Task GetOffer()
        {
            // must use an existing tradeId from local wallet
            var existingTradeId = "0x5e0e73963bb2870a4d214eddc7aa9f4fd39cbf7cd5fb131a0647a490042dcf8d";
            using var cts = new CancellationTokenSource(10000);
            var offer = await TradeManager.GetOffer(existingTradeId, fileContents: true, cancellationToken: cts.Token);

            Assert.NotNull(offer);
            Assert.False(string.IsNullOrWhiteSpace(offer.Offer));
        }

        [Fact]
        public async Task GetOfferSummary()
        {
            // a valid offer requires the actual coins used to create the offer to be un-spent on the blockchain
            var validOffer = "offer1qqp83w76wzru6cmqvpsxygqqwc7hynr6hum6e0mnf72sn7uvvkpt68eyumkhelprk0adeg42nlelk2mpafrgx923m0l47yqsaxujnj67dj4d4xhf0dv5fx4uyw6zxasykmyde0v4a9t096w2f4sdn08qc54gum80kkw9wxuh4mwl5a7et253f0ul0k8r8hryflzkawulhqhy0urlr0pck26tq5vvpahyrslmgj67hhq76shmkc37rjja6u9408mdvnyhe7ha7a3m7wdxqkrjvnpg9r2fh73e404q4j6v8claxnm9r27l002hecyn7uyvklvdq3vjvvtr3zscmvtvqcnfks6c9xsygedj8gadj8gadjrgdvz9uykj456sythkzc2d6wus6nh88e0j4gwex9jvfr443wxw2xdezuuxnmu0fpg9ddhhfhaeqd2av70t3fxl5j63kmf44tzlm6e0z07lv7u0dshw6n72juc0jnm67hdruvpj63y3p0u7rh9tqcdmccgm6whn266lelufcunt78xk0d3mz3jrs0eted585lpl22lnrqdrg5avd39jveacfpxq9mwkj09t9320amvqjc0xqscmypke8lctpq6h8l3hxxllsn6cr8yyegrfxcxh54u6p060vtn5372m88as5kx906knga262kvmkcys50mck34wd98lekxy6wuqffklutucz0arrftavw6r39ch0u70lvl9xuxlalyevzudculm067uvqmt26dgx235l0lsha3ua7l056ydazeex0qj7kp0wwazd6h9wwmjsnnlxgt78muc8a48s2zjpz0cl7p822ttnltvf0065uf0ezfh0rrvte5xcua4f4ufn39pydzc8gakxmlv4mlskr4fnj479v9jxpvllrahapj70rwjgrt0jjkkkeh6t0l78rj6l9pvlll8lzl443z37xh797uvzuwql8uu6nkn0veykz7t8zerv6afamutn8dmwt406aflp3knmhv9szmpz654l305p2zaqgxg3ds9p4zpj9w6yt3j47t4n4rse6a3s5h4n4zkhh27lvphzdl3cxl3et7wr5hfpn89pg6qq8gnrdm4e083054n9fnd3tl6e3d8r74le9gjtdjfvh7mvd2p25dj8gml4adzxvddlv668y40sdmxg5nk2yhd0fq0xt957mxchhvm4mwa94uc2k6nzenj4wraekvt6mu0m4naapkqtx8m7ha6yfewavhjlv3hn5audclkuqqqyh2km7gxmzur4";
            using var cts = new CancellationTokenSource(10000);
            var offerSummary = await TradeManager.GetOfferSummary(validOffer, false, cts.Token);

            Assert.NotNull(offerSummary);
            Assert.True(offerSummary.Offered.Any());
            Assert.True(offerSummary.Requested.Any());
        }

        [Fact]
        public async Task CancelOffer()
        {
            // this offer will only be cancelled in wallet, not securely cancelled on blockchain
            var cancellableTradeId = "0xefaf83ce58408b48fdfdb8320bf06b4a8df57e324dab5aedc9a956a0407f9a75";

            using var cts = new CancellationTokenSource(2000);
            var offer = await TradeManager.GetOffer(cancellableTradeId, fileContents: true, cts.Token);

            Assert.False(offer.TradeRecord.Status == TradeStatus.CANCELLED);

            using var cts2 = new CancellationTokenSource(10000);
            await TradeManager.CancelOffer(cancellableTradeId, cancellationToken: cts2.Token);
        }

        [Fact]
        public async Task CancelOfferSecure()
        {
            // this should be an offer with real coins that you own
            var secureCancellableTradeId = "0xfbd875cd29d8b3322d1295ccf48bf489bda7eb79868677971a769d3e77aedaf7";

            using var cts = new CancellationTokenSource(2000);
            var offer = await TradeManager.GetOffer(secureCancellableTradeId, fileContents: true, cts.Token);

            Assert.False(offer.TradeRecord.Status == TradeStatus.CANCELLED);

            using var cts2 = new CancellationTokenSource(2000);
            var (Valid, Id) = await TradeManager.CheckOfferValidity(offer.Offer, cts2.Token);

            Assert.True(Valid);

            // WARNING this creates is a secure cancellation transaction on the blockchain
            using var cts3 = new CancellationTokenSource(10000);
            await TradeManager.CancelOffer(secureCancellableTradeId, secure: true, cancellationToken: cts3.Token);
        }

        [Fact]
        public async Task TakeOffer()
        {
            // a real offer that your wallet can complete
            var acceptableOffer = "offer1qqp83w76wzru6cmqvpsxygqqvc0atkd5dht62v3nn8vklgl00efe6lpt0jskh74lkvkr47cheelm5ncr5c8pdt9r7hwxs0fhtt8cm4nn506acsxhw0sxym6p6x7sxj0d8etjat76x95urp9536z7k894ryuk06yffa043v8kgmme6w6svkwm0tkc5jl3gefm4hzl4efdr0llxlvxdvh7cnkdnua0wsrelldr2w376pp8fd8ws3m7jxppxg9sg9k5fn6g4lce73jemfxg469hn3097m5lwavmt40xs9gxrk5hha0cpdgvjyn986w3t8zhat6wghnv2430m3d9yemy9lwej6t3jtmjlalfau47ttkhlya6qqgu5sqkpfh0uh70tcs4g6wl0rcwum5d97e6c4q0dz2kak03clc58j49ax93u6j006f8khn80ut70rm8zu6jcvhr870qjleng7ayytwyatyl42xehd2pfqp47k9jkjlel739f47rh8ns6dncgwr6006h2du9w7xung3fz67ftceu7jfet52envrc6wkh9f240wlsg8ctdtmjekwvcycf0a0t67f35av6xfs26rvsxn7hlsdwg78ynmlhsm6h0m4fxfselv2vkrmc839unk8h7ezknad4fatn0elj2287rtqz50u7n6l46aadpl03x7v86qalxz3xl9mkut8627mnrxmaj8uvmuka7m6llkuqcxl7f32m5p4fhlctd2mxmxthu2nargcmx4nmamut6gte6wrncxrrx84xmwht5z7uu5ujuwgn4r90aluze6khlas4pdw3xrwd8dsuelkx4wp0mhx4j4vale4lucum24hrc0n5chns0qmdn835l9vza2dnx4p9jazr4e4gv0x0387r2v08k7jhfk9twtdmcchx84keu2ta3p06zaltmlmmu76jd6c0e73lykv67vfw6pkw07j4vuj7df8mkm4dsmr2d42mucf6g9sa5j0h2n0x2ngem0e37n3knff994lghydqqmskm6esrdqwh8";
            using var cts = new CancellationTokenSource(10000);

            var tradeRecord = await TradeManager.TakeOffer(acceptableOffer, cancellationToken: cts.Token);

            Assert.NotNull(tradeRecord);
            Assert.True(tradeRecord.Status == TradeStatus.PENDING_CONFIRM);
        }

        [Fact(Skip = "Requires review")]
        public async Task GetStrayCats()
        {
            // Arrange
            using var cts = new CancellationTokenSource(15000);

            // Act
            var returnValue = await TradeManager.GetStrayCats(cts.Token);

            // Assert
            Assert.NotNull(returnValue);
        }
    }
}
