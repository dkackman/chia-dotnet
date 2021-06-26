using System.Dynamic;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

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
        /// Retrieves a block record by height (assuming the height is less then or equal peak height)
        /// </summary>
        /// <param name="height">the height to get</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>block_record</returns>
        public async Task<dynamic> GetBlockRecordByHeight(uint height, CancellationToken cancellationToken)
        {
            dynamic data = new ExpandoObject();
            data.height = height;
            var message = CreateMessage("get_block_record_by_height", data);
            var response = await Daemon.SendMessage(message, cancellationToken);

            return response.Data.block_record;
        }

        /// <summary>
        /// Retrieves block records in a range
        /// </summary>        
        /// <param name="start">Start height</param>
        /// <param name="end">End Height - non-inclusive</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>list of block_record</returns>
        public async Task<dynamic> GetBlockRecords(uint start, uint end, CancellationToken cancellationToken)
        {
            dynamic data = new ExpandoObject();
            data.start = start;
            data.end = end;
            var message = CreateMessage("get_block_records", data);
            var response = await Daemon.SendMessage(message, cancellationToken);

            return response.Data.block_records;
        }

        /// <summary>
        /// Get the blocks between a start and end height
        /// </summary>
        /// <param name="start">Start height</param>
        /// <param name="end">End Height - non-inclusive</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>A list of blocks</returns>
        public async Task<dynamic> GetBlocks(uint start, uint end, CancellationToken cancellationToken)
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
        /// <returns>A list of headers</returns>
        public async Task<dynamic> GetUnfinishedBlockHeaders(CancellationToken cancellationToken)
        {
            var message = CreateMessage("get_unfinished_block_headers");
            var response = await Daemon.SendMessage(message, cancellationToken);

            return response.Data.headers;
        }

        /// <summary>
        /// Retrieves a list of coin records with a certain puzzle hash.
        /// </summary>
        /// <param name="puzzleHash">The puzzle hash</param>
        /// <param name="includeSpendCoins">whether to include spent coins too, instead of just unspent</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>A list of coin records</returns>
        public async Task<dynamic> GetCoinRecordsByPuzzleHash(string puzzleHash, bool includeSpendCoins, CancellationToken cancellationToken)
        {
            dynamic data = new ExpandoObject();
            data.puzzle_hash = puzzleHash;
            data.include_spend_coins = includeSpendCoins;
            var message = CreateMessage("get_coin_records_by_puzzle_hash", data);
            var response = await Daemon.SendMessage(message, cancellationToken);

            return response.Data.coin_records;
        }

        /// <summary>
        /// Retrieves a list of coin records with a certain puzzle hash.
        /// </summary>
        /// <param name="puzzleHash">The puzzle hash</param>
        /// <param name="start">confirmation start height for search</param>
        /// <param name="end">confirmation end height for search</param>
        /// <param name="includeSpendCoins">whether to include spent coins too, instead of just unspent</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>A list of coin records</returns>
        public async Task<dynamic> GetCoinRecordsByPuzzleHash(string puzzleHash, uint start, uint end, bool includeSpendCoins, CancellationToken cancellationToken)
        {
            dynamic data = new ExpandoObject();
            data.puzzle_hash = puzzleHash;
            data.start = start;
            data.end = end;
            data.include_spend_coins = includeSpendCoins;
            var message = CreateMessage("get_coin_records_by_puzzle_hash", data);
            var response = await Daemon.SendMessage(message, cancellationToken);

            return response.Data.coin_records;
        }

        /// <summary>
        /// Retrieves a coin record by its name/id.
        /// </summary>
        /// <param name="name">The coin name</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>A coin record</returns>
        public async Task<dynamic> GetCoinRecordsByName(string name, CancellationToken cancellationToken)
        {
            dynamic data = new ExpandoObject();
            data.name = name;
            var message = CreateMessage("get_coin_record_by_name", data);
            var response = await Daemon.SendMessage(message, cancellationToken);

            return response.Data.coin_record;
        }

        /// <summary>
        /// Retrieves the additions and removals (state transitions) for a certain block. Returns coin records for each addition and removal.
        /// </summary>
        /// <param name="headerHash">The header hash</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>A list of headers</returns>
        public async Task<dynamic> GetAdditionsAndRemovals(string headerHash, CancellationToken cancellationToken)
        {
            dynamic data = new ExpandoObject();
            data.header_hash = headerHash;
            var message = CreateMessage("get_additions_and_removals", data);
            var response = await Daemon.SendMessage(message, cancellationToken);

            return response.Data;
        }

        /// <summary>
        /// Returns all items in the mempool.
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>a list of mempool items</returns>
        public async Task<dynamic> GetAllMempoolItems(CancellationToken cancellationToken)
        {
            var message = CreateMessage("get_all_mempool_items");
            var response = await Daemon.SendMessage(message, cancellationToken);

            return response.Data.mempool_items;
        }

        /// <summary>
        /// Returns a list of all transaction IDs (spend bundle hashes) in the mempool.
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>a list of tx_ids</returns>
        public async Task<dynamic> GetAllMemmpoolTxIds(CancellationToken cancellationToken)
        {
            var message = CreateMessage("get_all_mempool_tx_ids");
            var response = await Daemon.SendMessage(message, cancellationToken);

            return response.Data.tx_ids;
        }

        /// <summary>
        /// Gets a mempool item by tx id.
        /// </summary>
        /// <param name="spendBundleHash"></param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>a list of tx_ids</returns>
        public async Task<dynamic> GetMemmpooItemByTxId(string txId, CancellationToken cancellationToken)
        {
            dynamic data = new ExpandoObject();
            data.tx_id = txId;
            var message = CreateMessage("get_mempool_item_by_tx_id");
            var response = await Daemon.SendMessage(message, cancellationToken);

            return response.Data.mempool_item;
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
        /// Retrieves some information about the current network
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>some information about the current network</returns>
        public async Task<dynamic> GetNetworkInfo(CancellationToken cancellationToken)
        {
            var message = CreateMessage("get_network_info");
            var response = await Daemon.SendMessage(message, cancellationToken);

            return response.Data;
        }
    }
}
