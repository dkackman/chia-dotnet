using System;
using System.Dynamic;

namespace chia.dotnet
{
    /// <summary>
    /// Wrapper class that uses a <see cref="Daemon"/> to send and receive messages to other services
    /// </summary>
    /// <remarks>The lifetime of the daemon is not controlled by the proxy. It should be disposed outside of this class. <see cref="RpcClient.Connect(System.Threading.CancellationToken)"/></remarks>
    /// and <see cref="Daemon.Register(System.Threading.CancellationToken)"/> should be invoked 
    public class ServiceProxy
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="daemon"><see cref="Daemon"/> instance to use for rpc communication</param>
        /// <param name="destinationService"><see cref="Message.Destination"/></param>
        public ServiceProxy(Daemon daemon, string destinationService)
        {
            Daemon = daemon ?? throw new ArgumentNullException(nameof(daemon));

            if (string.IsNullOrEmpty(destinationService))
            {
                throw new ArgumentNullException(nameof(destinationService));
            }

            DestinationService = destinationService;
        }

        /// <summary>
        /// The <see cref="Daemon"/> used for underlying RPC
        /// </summary>
        public Daemon Daemon { get; init; }

        /// <summary>
        /// <see cref="Message.Destination"/>
        /// </summary>
        public string DestinationService { get; init; }

        /// <summary>
        /// Constructs a <see cref="Message"/> instance with <see cref="Message.Destination"/> and <see cref="Message.Origin"/> set
        /// from <see cref="DestinationService"/> and <see cref="Daemon.OriginService"/>
        /// </summary>
        /// <param name="command">The command to send</param>
        /// <returns><see cref="Message"/></returns>
        protected Message CreateMessage(string command)
        {
            return Message.Create(command, null, DestinationService, Daemon.OriginService);
        }

        /// <summary>
        /// Constructs a <see cref="Message"/> instance with <see cref="Message.Destination"/> and <see cref="Message.Origin"/> set
        /// from <see cref="DestinationService"/> and <see cref="Daemon.OriginService"/>
        /// </summary>
        /// <param name="command">The command to send</param>
        /// <param name="data">Data to send with the command</param>
        /// <returns><see cref="Message"/></returns>
        protected Message CreateMessage(string command, dynamic data)
        {
            return Message.Create(command, data, DestinationService, Daemon.OriginService);
        }
    }
}
