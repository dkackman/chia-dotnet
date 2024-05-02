using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading;
using System.Threading.Tasks;

namespace chia.dotnet
{
    /// <summary>
    /// Proxy that communicates with the crawler
    /// </summary>
    /// <remarks>
    /// ctor
    /// </remarks>
    /// <param name="rpcClient"><see cref="IRpcClient"/> instance to use for rpc communication</param>
    /// <param name="originService"><see cref="Message.Origin"/></param>
    public sealed class CrawlerProxy(IRpcClient rpcClient, string originService) : ServiceProxy(rpcClient, ServiceNames.Crawler, originService)
    {
        /// <summary>
        /// Retrieves aggregate information about peers
        /// </summary>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>Information about peers</returns>
        public async Task<PeerCounts> GetPeerCounts(CancellationToken cancellationToken = default)
        {
            return await SendMessage<PeerCounts>("get_peer_counts", "peer_counts", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Retrieves ip addresses of peers that have connected after a given time
        /// </summary>
        /// <param name="after"></param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>IP addresses</returns>
        public async Task<(IEnumerable<string> ips, int total)> GetIPs(DateTime after, int offset = 0, int limit = 10000, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.after = after.ToTimestamp();
            data.offset = offset;
            data.limit = limit;

            var response = await SendMessage("get_ips_after_timestamp", data, cancellationToken).ConfigureAwait(false);

            return (
                Converters.ToObject<IEnumerable<string>>(response.ips),
                response.total
                );
        }
    }
}
