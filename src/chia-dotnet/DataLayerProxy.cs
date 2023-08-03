using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace chia.dotnet
{
    /// <summary>
    /// Proxy that communicates with the Data Layer
    /// </summary>
    public sealed class DataLayerProxy : ServiceProxy
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="rpcClient"><see cref="IRpcClient"/> instance to use for rpc communication</param>
        /// <param name="originService"><see cref="Message.Origin"/></param>
        public DataLayerProxy(IRpcClient rpcClient, string originService)
            : base(rpcClient, ServiceNames.DataLayer, originService)
        {
        }

        /// <summary>
        /// Adds a mirror
        /// </summary>
        /// <param name="id">Mirror id</param>
        /// <param name="amount">The Amount</param>
        /// <param name="urls">List of mirror urls</param>
        /// <param name="fee">Fee amount (in units of mojos)</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>An awaitable Task</returns>
        public async Task AddMirror(string id, ulong amount, IEnumerable<string> urls, ulong fee = 0, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.id = id;
            data.amount = amount;
            data.urls = urls.ToList();

            data.fee = fee;

            await SendMessage("add_mirror", "data", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Adds missing files
        /// </summary>
        /// <param name="ids">List of file id's</param>
        /// <param name="foldername">The folder name</param>
        /// <param name="overwrite">Indicator whetehr to overwrite files</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>An awaitable Task</returns>
        public async Task AddMissingFiles(string[] ids, string foldername, bool overwrite = false, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(foldername))
            {
                throw new ArgumentNullException(nameof(foldername));
            }
            dynamic data = new ExpandoObject();
            data.ids = ids;
            data.foldername = foldername;
            data.overwrite = overwrite;

            await SendMessage("add_missing_files", "data", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Applies a batch of updates.
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="changeList">Name value pairs of changes</param>
        /// <param name="fee">Fee amount (in units of mojos)</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>Transaction id</returns>
        public async Task<string> BatchUpdate(string id, IDictionary<string, string> changeList, ulong fee = 0, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            dynamic data = new ExpandoObject();
            data.id = id;
            data.changelist = changeList;
            data.fee = fee;

            var response = await SendMessage("batch_update", data, cancellationToken).ConfigureAwait(false);

            return response.tx_id;
        }

        /// <summary>
        /// Cancels an offer using a transaction
        /// </summary>
        /// <param name="tradeId">The trade id of the offer</param>
        /// <param name="fee">Transaction fee</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <param name="secure">This will create a transaction that includes coins that were offered</param>
        /// <returns>An awaitable Task</returns>
        public async Task CancelOffer(string tradeId, bool secure = false, ulong fee = 0, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.trade_id = tradeId;
            data.fee = fee;
            data.secure = secure;

            await SendMessage("cancel_offer", data, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Creates a data store.
        /// </summary>
        /// <param name="fee">Fee amount (in units of mojos)</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>The tree id and list of transactions</returns>
        public async Task<(string id, IEnumerable<TransactionRecord> txs)> CreateDataStore(ulong fee, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.fee = fee;

            var response = await SendMessage("create_data_store", data, cancellationToken).ConfigureAwait(false);

            return (
                response.id,
                response.txs
            );
        }
    }
}
