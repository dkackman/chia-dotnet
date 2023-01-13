using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading;
using System.Threading.Tasks;

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
        public CrawlerProxy(IRpcClient rpcClient, string originService)
            : base(rpcClient, ServiceNames.DataLayer, originService)
        {
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

            await SendMessage("get_peer_counts", "data", cancellationToken).ConfigureAwait(false);
        }


        /// <summary>
        /// Applies a batch of updates.
        /// </summary>
        /// <param name="id">Id</param>
        /// <param name="changeList">Name value pairs of changes</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>Transaction id</returns>
        public async Task<string> BatchUpdate(string id, IDictionary<KeyValuePair<string, string>> changeList, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            dynamic data = new ExpandoObject();
            data.id = id;
            data.changelist = changeList;

            var response = await SendMessage("batch_update", data, cancellationToken).ConfigureAwait(false);

            return response.tx_id;
        }
    }
}
