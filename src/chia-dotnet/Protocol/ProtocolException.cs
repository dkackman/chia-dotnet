using System;

namespace chia.dotnet.protocol;

/// <summary>
/// Exception thrown when the protocol endpoint returns a response <see cref="ProtocolMessage"/> that is an error
/// or there is a communication error on the WebSocket of http channel
/// </summary>
/// <remarks>
/// ctor
/// </remarks>
/// <param name="request">The request sent to the service</param>
/// <param name="message"><see cref="Exception.Message"/></param>
/// <param name="innerException"><see cref="Exception.InnerException"/></param>
public sealed class ProtocolException(ProtocolMessage request, string message, Exception? innerException) : Exception(message, innerException)
{
    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="request">The request sent to the service</param>
    public ProtocolException(ProtocolMessage request)
        : this(request, "The RPC endpoint returned success == false", null)
    {
    }

    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="request">The request sent to the service</param>
    /// <param name="message"><see cref="Exception.Message"/></param>
    public ProtocolException(ProtocolMessage request, string message)
        : this(request, message, null)
    {
    }

    /// <summary>
    /// The request sent to the service
    /// </summary>
    public ProtocolMessage Request { get; init; } = request;
}
