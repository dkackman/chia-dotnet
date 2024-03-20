using System.Threading;
using System.Threading.Tasks;

namespace chia.dotnet
{
    /// <summary>
    /// Core interface to chia RPC endpoints that passes and returns dynamic objects
    /// </summary>
    public interface IServiceProxy
    {
        /// <summary>
        /// These methods are the important ones that package up the request for the rpc client and then
        /// parse and convert the response for the requester
        /// </summary>
        /// <param name="command"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<dynamic> SendMessage(string command, CancellationToken cancellationToken = default);

        /// <summary>
        /// These methods are the important ones that package up the request for the rpc client and then
        /// parse and convert the response for the requester
        /// </summary>
        /// <param name="command"></param>
        /// <param name="data"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<dynamic> SendMessage(string command, dynamic? data, CancellationToken cancellationToken = default);
    }
}
