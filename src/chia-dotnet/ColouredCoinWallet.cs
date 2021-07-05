using System;
using System.Collections.Generic;
using System.Numerics;
using System.Dynamic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace chia.dotnet
{
    /// <summary>
    /// Rerpresents the methods used for interacting with a coloured coin wallet 
    /// </summary>
    /// <remarks>Also encapsulates the logged in state of the wallet with an id of <see cref="WalletId"/></remarks>
    public sealed class ColouredCoinWallet
    {
        private readonly WalletProxy _walletProxy;

        public ColouredCoinWallet(uint walletId, WalletProxy walletProxy)
        {
            WalletId = walletId;
            _walletProxy = walletProxy ?? throw new ArgumentNullException(nameof(walletProxy));
        }

        /// <summary>
        /// The id of the wallet
        /// </summary>
        public uint WalletId { get; init; }

        /// <summary>
        /// Login with <see cref="WalletId"/>
        /// </summary>
        /// <remarks>Always login before interacting with the wallet. Logged in state is kept on the serve so might have changed</remarks>
        /// <returns>an awaitabel <see cref="Task"/></returns>
        public async Task<uint> Login(CancellationToken cancellationToken)
        {
            var fingerprints = await _walletProxy.GetPublicKeys(cancellationToken);

            // not 100% sure this applies in all cases but wallets seem to come back in id order
            // haven't figured out a different or better way to get a fingerprint from an id
            var fingerprint = fingerprints.First();

            return await _walletProxy.LogIn(fingerprint, true, cancellationToken);
        }

        /// <summary>
        /// Get the name of a wallet's coloured coin
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>The coin name</returns>
        public async Task<string> GetName(CancellationToken cancellationToken)
        {
            dynamic data = new ExpandoObject();
            data.wallet_id = WalletId;

            var response = await _walletProxy.SendMessage("cc_get_name", data, cancellationToken);

            return response.Data.name.ToString();
        }

        /// <summary>
        /// Set the name of a wallet's coloured coin
        /// </summary>
        /// <param name="name">The new name</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>An awaitable <see cref="Task"/></returns>
        public async Task SetName(string name, CancellationToken cancellationToken)
        {
            dynamic data = new ExpandoObject();
            data.wallet_id = WalletId;
            data.name = name;

            _ = await _walletProxy.SendMessage("cc_set_name", data, cancellationToken);
        }

        /// <summary>
        /// Get the colour of a wallet's coloured coin
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>The colout as a string</returns>
        public async Task<string> GetColour(CancellationToken cancellationToken)
        {
            dynamic data = new ExpandoObject();
            data.wallet_id = WalletId;

            var response = await _walletProxy.SendMessage("cc_get_colour", data, cancellationToken);

            return response.Data.colour;
        }

        /// <summary>
        /// Spend a coloured coin
        /// </summary>
        /// <param name="limit">The limit amount</param>
        /// <param name="fee">fee to create the wallet</param>
        /// <param name="amount">the amount to put in the wallet</param>         
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>A transaction</returns>
        public async Task<dynamic> Spend(string innerAddress, BigInteger amount, BigInteger fee, CancellationToken cancellationToken)
        {
            dynamic data = new ExpandoObject();
            data.wallet_id = WalletId;
            data.inner_address = innerAddress;
            data.amount = amount;
            data.fee = fee;

            var response = await _walletProxy.SendMessage("cc_spend", data, cancellationToken);

            return response.Data.transaction;
        }

        /// <summary>
        /// Create an offer file from a set of id's
        /// </summary>
        /// <param name="ids">The set of ids</param>
        /// <param name="filename">path to the offer file to create</param>   
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>An awaitable <see cref="Task"/></returns>
        public async Task CreateOfferForIds(IDictionary<int, int> ids, string filename, CancellationToken cancellationToken)
        {
            dynamic data = new ExpandoObject();
            data.ids = ids;
            data.filename = filename;

            _ = await _walletProxy.SendMessage("create_offer_for_ids", data, cancellationToken);
        }

        /// <summary>
        /// Get offer discrepencies
        /// </summary>
        /// <param name="filename">path to the offer file</param>         
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>The dicrepencies</returns>
        public async Task<dynamic> GetDiscrepenciesForOffer(string filename, CancellationToken cancellationToken)
        {
            dynamic data = new ExpandoObject();
            data.filename = filename;

            var response = await _walletProxy.SendMessage("get_discrepancies_for_offer", data, cancellationToken);

            return response.Data.discrepancies;
        }

        /// <summary>
        /// Respond to an offer
        /// </summary>
        /// <param name="filename">path to the offer file</param>        
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>An awaitable <see cref="Task"/></returns>
        public async Task RespondToOffer(string filename, CancellationToken cancellationToken)
        {
            dynamic data = new ExpandoObject();

            data.filename = filename;

            _ = await _walletProxy.SendMessage("respond_to_offer", data, cancellationToken);
        }

        /// <summary>
        /// Get a trade
        /// </summary>
        /// <param name="tradeId">The id of the trade to find</param>         
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>The trade</returns>
        public async Task<dynamic> GetTrade(string tradeId, CancellationToken cancellationToken)
        {
            dynamic data = new ExpandoObject();
            data.trade_id = tradeId;

            var response = await _walletProxy.SendMessage("get_trade", data, cancellationToken);

            return response.Data.trade;
        }

        /// <summary>
        /// Get all trades
        /// </summary>        
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>The trades</returns>
        public async Task<IEnumerable<dynamic>> GetAllTrades(CancellationToken cancellationToken)
        {
            var response = await _walletProxy.SendMessage("get_all_trades", cancellationToken);

            return response.Data.trades;
        }

        /// <summary>
        /// Cancel a trade
        /// </summary>
        /// <param name="tradeId">The id of the trade to find</param>         
        /// <param name="secure">Flag indicating whether to cancel pedning offer securely or not</param>         
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>An awaitable <see cref="Task"/></returns>
        public async Task CancelTrade(string tradeId, bool secure, CancellationToken cancellationToken)
        {
            dynamic data = new ExpandoObject();
            data.trade_id = tradeId;
            data.secure = secure;

            _ = await _walletProxy.SendMessage("cancel_trade", data, cancellationToken);
        }
    }
}
