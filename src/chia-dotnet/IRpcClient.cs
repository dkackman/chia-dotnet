using System;
using System.Threading;
using System.Threading.Tasks;

namespace chia.dotnet
{
    /// <summary>
    /// Interface representing rpc communcation
    /// </summary>
    public interface IRpcClient : IDisposable
    {
        /// <summary>
        /// Details of the RPC service endpoint
        /// </summary>
        EndpointInfo Endpoint { get; init; }

        /// <summary>
        /// Posts a <see cref="Message"/> to the websocket but does not wait for a response
        /// </summary>
        /// <param name="message">The message to post</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <remarks>Awaiting this method waits for the message to be sent only. It doesn't await a response.</remarks>
        /// <returns>Awaitable <see cref="Task"/></returns>
        Task PostMessage(Message message, CancellationToken cancellationToken = default);

        /// <summary>
        /// Sends a <see cref="Message"/> to the endpoint and waits for a response
        /// </summary>
        /// <param name="message">The message to send</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <remarks>Awaiting this method will block until a response is received from the <see cref="Endpoint"/> or the <see cref="CancellationToken"/> is cancelled</remarks>
        /// <returns>The response message</returns>
        /// <exception cref="ResponseException">Throws when <see cref="Message.IsSuccessfulResponse"/> is False</exception>
        Task<dynamic> SendMessage(Message message, CancellationToken cancellationToken = default);
    }
}
