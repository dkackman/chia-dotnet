using System.Dynamic;
using System.Threading;
using System.Threading.Tasks;
using System.Numerics;

namespace chia.dotnet
{
    public class FullNodeProxy : ServiceProxy
    {
        public FullNodeProxy(Daemon daemon)
            : base(daemon, ServiceNames.FullNode)
        {
        }

        public async Task<dynamic> GetBlockchainState(CancellationToken cancellationToken)
        {
            var message = CreateMessage("get_blockchain_state");
            var response = await Daemon.SendMessage(message, cancellationToken);

            return response.Data.blockchain_state;
        }

        public async Task<dynamic> GetBlock(string headerHash, CancellationToken cancellationToken)
        {
            dynamic data = new ExpandoObject();
            data.header_hash = headerHash;
            var message = CreateMessage("get_block", data);
            var response = await Daemon.SendMessage(message, cancellationToken);

            return response.Data.block;
        }

        public async Task<dynamic> GetBlocks(int start, int end, CancellationToken cancellationToken)
        {
            dynamic data = new ExpandoObject();
            data.start = start;
            data.end = end;
            var message = CreateMessage("get_blocks", data);
            var response = await Daemon.SendMessage(message, cancellationToken);

            return response.Data.blocks;
        }

        public async Task<dynamic> GetUnfinishedBlockHeaders(CancellationToken cancellationToken)
        {
            var message = CreateMessage("get_unfinished_block_headers");
            var response = await Daemon.SendMessage(message, cancellationToken);

            return response.Data.headers;
        }

        public async Task<BigInteger> GetNetworkSpace(string newerBlockHeaderHash, string olderBlockHeaderHash, CancellationToken cancellationToken)
        {
            dynamic data = new ExpandoObject();
            data.newer_block_header_hash = newerBlockHeaderHash;
            data.older_block_header_hash = olderBlockHeaderHash;

            var message = CreateMessage("get_network_space", data);
            var response = await Daemon.SendMessage(message, cancellationToken);

            return response.Data.space;
        }
    }
}
