using System;

namespace chia.dotnet
{
    /// <summary>
    /// Exception thrown when the RPC endpoint returns a response <see cref="Message"/> but Data.success is false
    /// </summary>
    public class ResponseException : ApplicationException
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="request">The request sent to the service</param>
        /// <param name="response">The response from the service</param>
        public ResponseException(Message request, Message response)
            : this(request, response, "The RPC endpoint returned success == false", null)
        {
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="request">The request sent to the service</param>
        /// <param name="response">The response from the service</param>
        /// <param name="message"><see cref="Exception.Message"/></param>
        /// <param name="innerException"><see cref="Exception.InnerException"/></param>
        public ResponseException(Message request, Message response, string message, Exception innerException)
            : base(message, innerException)
        {
            Request = request;
            Response = response;
        }

        /// <summary>
        /// The request sent to the service
        /// </summary>
        public Message Request { get; init; }


        /// <summary>
        /// The response from the service
        /// </summary>
        public Message Response { get; init; }
    }
}
