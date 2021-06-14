using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using System.Threading.Tasks;

namespace chia.dotnet
{
    /// <summary>
    /// Wrapper class to use the <see cref="Daemon"/> to send and receive messages to other services
    /// </summary>
    public class ServiceProxy
    {
        private readonly Daemon _daemon;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="daemon"><see cref="Daemon"/> instance to use for rpc communication</param>
        /// <param name="destinationService"><see cref="Message.Destination"/></param>
        public ServiceProxy(Daemon daemon, string destinationService)
        {
            _daemon = daemon ?? throw new ArgumentNullException(nameof(daemon));

            if(string.IsNullOrEmpty(destinationService))
            {
                throw new ArgumentNullException(nameof(destinationService));
            }

            DestinationService = destinationService;
        }

        /// <summary>
        /// <see cref="Message.Destination"/>
        /// </summary>
        public string DestinationService { get; init; }

        /// <summary>
        /// Constructs a <see cref="Message"/> instance with <see cref="Message.Destination"/> and <see cref="Message.Origin"/> set correctly
        /// </summary>
        /// <param name="command">The command to send</param>
        /// <param name="data">Data to send with the command</param>
        /// <returns><see cref="Message"/></returns>
        protected Message CreateMessage(string command, dynamic data)
        {
            return Message.Create(command, data ?? new ExpandoObject(), DestinationService, _daemon.OriginServiceName);
        }
    }
}
