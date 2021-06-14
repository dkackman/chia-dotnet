using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace chia.dotnet
{
    public class FullNodeProxy : ServiceProxy
    {
        public FullNodeProxy(Daemon daemon)
            : base(daemon, ServiceNames.FullNode)
        {
        }

        public async Task<Message> GetBlockchainState(CancellationToken cancellation)
        {
            var message = CreateMessage("get_blockchain_state", null);
            return await Daemon.SendMessage(message, cancellation);
        }
    }
}
