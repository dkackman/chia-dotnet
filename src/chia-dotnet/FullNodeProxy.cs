using System.Dynamic;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace chia.dotnet
{
    /// <summary>
    /// Proxy that communicates with the full node via the daemon
    /// </summary>
    public sealed class FullNodeProxy : ServiceProxy
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
        /// Returns a summary of the node's view of the blockchain.
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>blockchain_state</returns>
        public async Task<dynamic> GetBlockchainState(CancellationToken cancellationToken)
        {
            var response = await SendMessage("get_blockchain_state", cancellationToken);

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

            var response = await SendMessage("get_block", data, cancellationToken);

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

            var response = await SendMessage("get_block_record", data, cancellationToken);

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

            var response = await SendMessage("get_block_record_by_height", data, cancellationToken);

            return response.Data.block_record;
        }

        /// <summary>
        /// Retrieves block records in a range
        /// </summary>        
        /// <param name="start">Start height</param>
        /// <param name="end">End Height - non-inclusive</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>list of block_record</returns>
        public async Task<IEnumerable<dynamic>> GetBlockRecords(uint start, uint end, CancellationToken cancellationToken)
        {
            dynamic data = new ExpandoObject();
            data.start = start;
            data.end = end;

            var response = await SendMessage("get_block_records", data, cancellationToken);

            return response.Data.block_records;
        }

        /// <summary>
        /// Get the blocks between a start and end height
        /// </summary>
        /// <param name="start">Start height</param>
        /// <param name="end">End Height - non-inclusive</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>A list of blocks</returns>
        public async Task<IEnumerable<dynamic>> GetBlocks(uint start, uint end, CancellationToken cancellationToken)
        {
            dynamic data = new ExpandoObject();
            data.start = start;
            data.end = end;

            var response = await SendMessage("get_blocks", data, cancellationToken);

            return response.Data.blocks;
        }

        /// <summary>
        /// Get unfinished block headers
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>A list of headers</returns>
        public async Task<IEnumerable<dynamic>> GetUnfinishedBlockHeaders(CancellationToken cancellationToken)
        {
            var response = await SendMessage("get_unfinished_block_headers", cancellationToken);

            return response.Data.headers;
        }

        /// <summary>
        /// Retrieves the coins for a given puzzlehash, by default returns unspent coins.
        /// </summary>
        /// <param name="puzzlehash">The puzzle hash</param>
        /// <param name="includeSpendCoins">whether to include spent coins too, instead of just unspent</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>A list of coin records</returns>
        public async Task<IEnumerable<dynamic>> GetCoinRecordsByPuzzleHash(string puzzlehash, bool includeSpendCoins, CancellationToken cancellationToken)
        {
            dynamic data = new ExpandoObject();
            data.puzzle_hash = puzzlehash;
            data.include_spend_coins = includeSpendCoins;

            var response = await SendMessage("get_coin_records_by_puzzle_hash", data, cancellationToken);

            return response.Data.coin_records;
        }

        /// <summary>
        /// Retrieves the coins for a given puzzlehash, by default returns unspent coins.
        /// </summary>
        /// <param name="puzzlehash">The puzzle hash</param>
        /// <param name="startHeight">confirmation start height for search</param>
        /// <param name="endHeight">confirmation end height for search</param>
        /// <param name="includeSpendCoins">whether to include spent coins too, instead of just unspent</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>A list of coin records</returns>
        public async Task<IEnumerable<dynamic>> GetCoinRecordsByPuzzleHash(string puzzlehash, uint startHeight, uint endHeight, bool includeSpendCoins, CancellationToken cancellationToken)
        {
            dynamic data = new ExpandoObject();
            data.puzzle_hash = puzzlehash;
            data.start_height = startHeight;
            data.end_height = endHeight;
            data.include_spend_coins = includeSpendCoins;

            var response = await SendMessage("get_coin_records_by_puzzle_hash", data, cancellationToken);

            return response.Data.coin_records;
        }

        /// <summary>
        /// Retrieves the coins for a given list of puzzlehashes, by default returns unspent coins.
        /// </summary>
        /// <param name="puzzlehashes">The list of puzzle hashes</param>
        /// <param name="startHeight">confirmation start height for search</param>
        /// <param name="endHeight">confirmation end height for search</param>
        /// <param name="includeSpendCoins">whether to include spent coins too, instead of just unspent</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>A list of coin records</returns>
        public async Task<IEnumerable<dynamic>> GetCoinRecordsByPuzzleHashes(IEnumerable<string> puzzlehashes, uint startHeight, uint endHeight, bool includeSpendCoins, CancellationToken cancellationToken)
        {
            dynamic data = new ExpandoObject();
            data.puzzle_hash = puzzlehashes.ToList();
            data.start_height = startHeight;
            data.end_height = endHeight;
            data.include_spend_coins = includeSpendCoins;

            var response = await SendMessage("get_coin_records_by_puzzle_hashes", data, cancellationToken);

            return response.Data.coin_records;
        }

        /// <summary>
        /// Retrieves a coin record by its name/id.
        /// </summary>
        /// <param name="name">The coin name</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>A coin record</returns>
        public async Task<dynamic> GetCoinRecordByName(string name, CancellationToken cancellationToken)
        {
            dynamic data = new ExpandoObject();
            data.name = name;

            var response = await SendMessage("get_coin_record_by_name", data, cancellationToken);

            return response.Data.coin_record;
        }

        /// <summary>
        /// Retrieves the additions and removals (state transitions) for a certain block. Returns coin records for each addition and removal.
        /// </summary>
        /// <param name="headerHash">The header hash</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>A list of additions and a list of removals</returns>
        public async Task<(IEnumerable<dynamic> Additions, IEnumerable<dynamic> Removals)> GetAdditionsAndRemovals(string headerHash, CancellationToken cancellationToken)
        {
            dynamic data = new ExpandoObject();
            data.header_hash = headerHash;

            var response = await SendMessage("get_additions_and_removals", data, cancellationToken);

            return (response.Data.additions, response.Data.removals);
        }

        /// <summary>
        /// Returns all items in the mempool.
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>a list of mempool items</returns>
        public async Task<IEnumerable<dynamic>> GetAllMempoolItems(CancellationToken cancellationToken)
        {
            var response = await SendMessage("get_all_mempool_items", cancellationToken);

            return response.Data.mempool_items;
        }

        /// <summary>
        /// Returns a list of all transaction IDs (spend bundle hashes) in the mempool.
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>a list of tx_ids</returns>
        public async Task<IEnumerable<dynamic>> GetAllMemmpoolTxIds(CancellationToken cancellationToken)
        {
            var response = await SendMessage("get_all_mempool_tx_ids", cancellationToken);

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

            var response = await SendMessage("get_mempool_item_by_tx_id", cancellationToken);

            return response.Data.mempool_item;
        }

        /// <summary>
        /// Retrieves an estimate of total space validating the chain between two block header hashes.
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

            var response = await SendMessage("get_network_space", data, cancellationToken);

            return response.Data.space;
        }

        /// <summary>
        /// Retrieves some information about the current network
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>some information about the current network</returns>
        public async Task<(string NetworkName, string NetworkPrefix)> GetNetworkInfo(CancellationToken cancellationToken)
        {
            var response = await SendMessage("get_network_info", cancellationToken);

            return (response.Data.network_name, response.Data.network_prefix);
        }

        /// <summary>
        /// Pushes a transaction / spend bundle to the mempool and blockchain. 
        /// Returns whether the spend bundle was successfully included into the mempool
        /// </summary>
        /// <param name="spendBundle"></param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>Indicator of whether the spend bundle was successfully included in the mempool</returns>
        public async Task<bool> PushTx(dynamic spendBundle, CancellationToken cancellationToken)
        {
            dynamic data = new ExpandoObject();
            data.spend_bundle = spendBundle;

            var response = await SendMessage("push_tx", data, cancellationToken);

            return response.Data.status?.ToString() == "SUCCESS";
        }

        /// <summary>
        /// Gets a recent signage point
        /// </summary>
        /// <param name="spHash">signage point hash</param> 
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>Indicator of whether the spend bundle was successfully included in the mempool</returns>
        public async Task<(dynamic signagePoint, double timeReceived, bool reverted)> GetRecentSignagePoint(string spHash, CancellationToken cancellationToken)
        {
            dynamic data = new ExpandoObject();
            data.sp_hash = spHash;

            var response = await SendMessage("get_recent_signage_point_or_eos", data, cancellationToken);

            return (response.Data.signage_point, response.Data.time_received, response.Data.reverted);
        }

        /// <summary>
        /// Gets a recent end of slot
        /// </summary>
        /// <param name="challengeHash">challenge hash</param> 
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>A signange _point or an eos</returns>
        public async Task<(dynamic eos, double timeReceived, bool reverted)> GetRecentEOS(string challengeHash, CancellationToken cancellationToken)
        {
            dynamic data = new ExpandoObject();
            data.challenge_hash = challengeHash;

            var response = await SendMessage("get_recent_signage_point_or_eos", data, cancellationToken);

            return (response.Data.eos, response.Data.time_received, response.Data.reverted);
        }


        /// <summary>
        /// Gets a coin solution 
        /// </summary>
        /// <param name="coinId">Id of the coin</param>
        /// <param name="height">Block height</param> 
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>A coin_solution</returns>
        public async Task<dynamic> GetPuzzleAndSolution(string coinId, uint height, CancellationToken cancellationToken)
        {
            dynamic data = new ExpandoObject();
            data.coin_id = coinId;
            data.height = height;

            var response = await SendMessage("get_puzzle_and_solution", data, cancellationToken);

            return response.Data.coin_solution;
        }
    }
}
