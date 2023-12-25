using System;

namespace chia.dotnet
{
    /// <summary>
    /// Exception thrown when the RPC endpoint returns a response <see cref="Message"/> but Data.success is false
    /// oro there is a communication error on the websocket of http channgel
    /// </summary>
    /// <remarks>
    /// ctor
    /// </remarks>
    /// <param name="request">The request sent to the service</param>
    /// <param name="message"><see cref="Exception.Message"/></param>
    /// <param name="innerException"><see cref="Exception.InnerException"/></param>
    public sealed class ResponseException(Message request, string message, Exception? innerException) : Exception(message, innerException)
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="request">The request sent to the service</param>
        public ResponseException(Message request)
            : this(request, "The RPC endpoint returned success == false", null)
        {
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="request">The request sent to the service</param>
        /// <param name="message"><see cref="Exception.Message"/></param>
        public ResponseException(Message request, string message)
            : this(request, message, null)
        {
        }

        /// <summary>
        /// The request sent to the service
        /// </summary>
        public Message Request { get; init; } = request;
    }
}
