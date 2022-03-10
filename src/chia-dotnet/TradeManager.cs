using System;
using System.Linq;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading;
using System.Threading.Tasks;

namespace chia.dotnet
{
    /// <summary>
    /// API wrapper for those wallet RPC methods dealing with trades and offers
    /// </summary>
    public class TradeManager
    {
        public WalletProxy WalletProxy { get; init; }

        public TradeManager(WalletProxy walletProxy)
        {
            WalletProxy = walletProxy ?? throw new ArgumentNullException(nameof(walletProxy));
        }

        /// <summary>
        /// Retreives the number of offers.
        /// </summary>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>The number of offers</returns>
        public async Task<(int Total, int MyOffersCount, int TakenOffersCount)> GetOffersCount(CancellationToken cancellationToken = default)
        {
            var response = await WalletProxy.SendMessage("get_offers_count", null, cancellationToken).ConfigureAwait(false);

            return (
                response.total,
                response.my_offers_count,
                response.taken_offers_count
             );
        }

        /// <summary>
        /// Get the list of offers
        /// </summary>
        /// <param name="excludeMyOffers">Do not include my offers in the result set</param>
        /// <param name="excludeTakenOffers">Do not include taken offers in the result set</param>
        /// <param name="includeCompleted">Do not include completed offers in the result set</param>
        /// <param name="sortKey">Field to sort results by</param>
        /// <param name="fileContents">Include the offer value in the result</param>
        /// <param name="reverse">Reverse the sort order of the results</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>A list of <see cref="OfferRecord"/>s</returns>
        public async Task<IEnumerable<OfferRecord>> GetOffers(bool excludeMyOffers = false, bool excludeTakenOffers = false, bool includeCompleted = false, string? sortKey = null, bool reverse = false, bool fileContents = false, CancellationToken cancellationToken = default)
        {
            var (Total, _, _) = await GetOffersCount(cancellationToken).ConfigureAwait(false);
            return await GetOffers(0, Total, excludeMyOffers, excludeTakenOffers, includeCompleted, sortKey, reverse, fileContents, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Get the list of offers
        /// </summary>
        /// <param name="start">the start index of offers (zero based)</param>
        /// <param name="end">The end index of offers</param>
        /// <param name="excludeMyOffers">Do not include my offers in the result set</param>
        /// <param name="excludeTakenOffers">Do not include taken offers in the result set</param>
        /// <param name="includeCompleted">Do not include completed offers in the result set</param>
        /// <param name="sortKey">Field to sort results by</param>
        /// <param name="fileContents">Include the offer value in the result</param>
        /// <param name="reverse">Reverse the sort order of the results</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>A list of <see cref="OfferRecord"/>s</returns>
        public async Task<IEnumerable<OfferRecord>> GetOffers(int start, int end, bool excludeMyOffers = false, bool excludeTakenOffers = false, bool includeCompleted = false, string? sortKey = null, bool reverse = false, bool fileContents = false, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.start = start;
            data.end = end;
            data.exclude_my_offers = excludeMyOffers;
            data.exclude_taken_offers = excludeTakenOffers;
            data.include_completed = includeCompleted;
            data.sort_key = sortKey;
            data.reverse = reverse;
            data.file_contents = fileContents;

            var response = await WalletProxy.SendMessage("get_all_offers", data, cancellationToken).ConfigureAwait(false);
            var offers = (IEnumerable<string>)Converters.ToEnumerable<string>(response.offers);
            var tradeRecords = (IEnumerable<TradeRecord>)Converters.ToEnumerable<TradeRecord>(response.trade_records);

            var zipped = offers.Zip(tradeRecords);

            return from zip in zipped
                   select new OfferRecord()
                   {
                       Offer = zip.First,
                       TradeRecord = zip.Second
                   };
        }

        /// <summary>
        /// Get the default list of CATs
        /// </summary>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>A list of CATs</returns>
        public async Task<IEnumerable<CATInfo>> GetCATList(CancellationToken cancellationToken = default)
        {
            return await WalletProxy.SendMessage<IEnumerable<CATInfo>>("get_cat_list", null, "cat_list", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Get the CAT name from an asset id
        /// </summary>
        /// <param name="assetId">The asset id</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>The wallet id and name of the CAT</returns>
        public async Task<(uint? WalletId, string Name)> AssetIdToName(string assetId, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.asset_id = assetId;

            var response = await WalletProxy.SendMessage("cat_asset_id_to_name", data, cancellationToken).ConfigureAwait(false);

            return (
                (uint?)response.wallet_id,
                response.name
                );
        }

        /// <summary>
        /// Checks the validity of an offer
        /// </summary>
        /// <param name="offer">The bech32 encoded offer hex</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>Indicator of the offer's validity</returns>
        public async Task<bool> CheckOfferValidity(string offer, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.offer = offer;

            return await WalletProxy.SendMessage<bool>("check_offer_validity", data, "valid", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Retreives an offer
        /// </summary>
        /// <param name="tradeId">The trade id of the offer</param>
        /// <param name="fileContents">Indicator as to whether to return the offer contents. 
        /// <see cref="OfferRecord.Offer"/> will be empty if this is false.</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>The OfferRecord</returns>
        public async Task<OfferRecord> GetOffer(string tradeId, bool fileContents = false, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.trade_id = tradeId;
            data.file_contents = fileContents;

            return await WalletProxy.SendMessage<OfferRecord>("get_offer", data, null, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Cancels an offer
        /// </summary>
        /// <param name="tradeId">The trade id of the offer</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>An awaitable Task</returns>
        public async Task CancelOffer(string tradeId, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.trade_id = tradeId;
            data.secure = false;

            await WalletProxy.SendMessage("cancel_offer", data, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Cancels an offer using a transaction
        /// </summary>
        /// <param name="tradeId">The trade id of the offer</param>
        /// <param name="fee">Transaction fee</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>An awaitable Task</returns>
        public async Task CancelOffer(string tradeId, ulong fee, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.trade_id = tradeId;
            data.fee = fee;
            data.secure = true;

            await WalletProxy.SendMessage("cancel_offer", data, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Takes an offer
        /// </summary>
        /// <param name="offer">The bech32 encoded offer</param>
        /// <param name="fee">Transaction fee</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>The associated trade record</returns>
        public async Task<TradeRecord> TakeOffer(string offer, ulong fee, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.offer = offer;
            data.fee = fee;

            return await WalletProxy.SendMessage<TradeRecord>("take_offer", data, "trade_record", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Retreives the summary of an offer
        /// </summary>
        /// <param name="offer">The bech32 encoded offer hex</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>The summary of the offer</returns>
        public async Task<(IDictionary<string, int> Offered, IDictionary<string, int> Requested)> GetOfferSummary(string offer, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.offer = offer;

            var response = await WalletProxy.SendMessage("get_offer_summary", data, cancellationToken).ConfigureAwait(false);

            return (
                Converters.ToObject<IDictionary<string, int>>(response.summary.offered),
                Converters.ToObject<IDictionary<string, int>>(response.summary.requested)
                );
        }

        /// <summary>
        /// Create an offer file from a set of id's
        /// </summary>
        /// <param name="ids">The set of ids</param>
        /// <param name="fee">Transaction fee for offer creation</param>   
        /// <param name="validateOnly">Only validate the offer contents. Do not create.</param>   
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>An awaitable <see cref="Task"/></returns>
        public async Task<OfferRecord> CreateOfferForIds(IDictionary<string, int> ids, ulong fee, bool validateOnly = false, CancellationToken cancellationToken = default)
        {
            if (ids is null)
            {
                throw new ArgumentNullException(nameof(ids));
            }

            dynamic data = new ExpandoObject();
            data.ids = ids;
            data.fee = fee;
            data.validate_only = validateOnly;

            return await WalletProxy.SendMessage<OfferRecord>("create_offer_for_ids", data, null, cancellationToken).ConfigureAwait(false);
        }
    }
}
