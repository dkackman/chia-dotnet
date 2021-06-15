using System.Dynamic;
using System.Threading;
using System.Threading.Tasks;
using System.Numerics;

namespace chia.dotnet
{
    /// <summary>
    /// Proxy that communicates with the full node via the daemon
    /// </summary>
    public class FullNodeProxy : ServiceProxy
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="daemon">The <see cref="Daemon"/> to handle RPC</param>
        public FullNodeProxy(Daemon daemon)
            : base(daemon, ServiceNames.FullNode)
        {
        }

        /// <summary>
        /// Get the current state of the blockchain as known by the node
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>blockchain_state</returns>
        public async Task<dynamic> GetBlockchainState(CancellationToken cancellationToken)
        {
            var message = CreateMessage("get_blockchain_state");
            var response = await Daemon.SendMessage(message, cancellationToken);

            return response.Data.blockchain_state;
        }

        /// <summary>
        /// Get a block by a header hash
        /// </summary>
        /// <param name="headerHash">The header hash</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>block</returns>
        public async Task<dynamic> GetBlock(string headerHash, CancellationToken cancellationToken)
        {
            dynamic data = new ExpandoObject();
            data.header_hash = headerHash;
            var message = CreateMessage("get_block", data);
            var response = await Daemon.SendMessage(message, cancellationToken);

            return response.Data.block;
        }

        /// <summary>
        /// Get a block record by a header hash
        /// </summary>
        /// <param name="headerHash">The header hash</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>block_record</returns>
        public async Task<dynamic> GetBlockRecord(string headerHash, CancellationToken cancellationToken)
        {
            dynamic data = new ExpandoObject();
            data.header_hash = headerHash;
            var message = CreateMessage("get_block_record", data);
            var response = await Daemon.SendMessage(message, cancellationToken);

            return response.Data.block_record;
        }

        /// <summary>
        /// Get the blocks ebtween a start and end height
        /// </summary>
        /// <param name="start">Start height</param>
        /// <param name="end">End Height</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>Colleciton of blocks</returns>
        public async Task<dynamic> GetBlocks(int start, int end, CancellationToken cancellationToken)
        {
            dynamic data = new ExpandoObject();
            data.start = start;
            data.end = end;
            var message = CreateMessage("get_blocks", data);
            var response = await Daemon.SendMessage(message, cancellationToken);

            return response.Data.blocks;
        }

        /// <summary>
        /// Get unfinished block headers
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>list of headers</returns>
        public async Task<dynamic> GetUnfinishedBlockHeaders(CancellationToken cancellationToken)
        {
            var message = CreateMessage("get_unfinished_block_headers");
            var response = await Daemon.SendMessage(message, cancellationToken);

            return response.Data.headers;
        }

        /// <summary>
        /// Get network space
        /// </summary>
        /// <param name="newerBlockHeaderHash"></param>
        /// <param name="olderBlockHeaderHash"></param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns><see cref="BigInteger"/> of network space in bytes</returns>
        public async Task<BigInteger> GetNetworkSpace(string newerBlockHeaderHash, string olderBlockHeaderHash, CancellationToken cancellationToken)
        {
            dynamic data = new ExpandoObject();
            data.newer_block_header_hash = newerBlockHeaderHash;
            data.older_block_header_hash = olderBlockHeaderHash;

            var message = CreateMessage("get_network_space", data);
            var response = await Daemon.SendMessage(message, cancellationToken);

            return response.Data.space;
        }

        /// <summary>
        /// Get connections that the full node has
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>Collection of connections</returns>
        public async Task<dynamic> GetConnections(CancellationToken cancellationToken)
        {
            var message = CreateMessage("get_connections");
            var response = await Daemon.SendMessage(message, cancellationToken);

            return response.Data.connections;
        }
    }
}
