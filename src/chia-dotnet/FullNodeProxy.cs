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
        /// <param name="rpcClient"><see cref="IRpcClient"/> instance to use for rpc communication</param>
        /// <param name="originService"><see cref="Message.Origin"/></param>
        public FullNodeProxy(IRpcClient rpcClient, string originService)
            : base(rpcClient, ServiceNames.FullNode, originService)
        {
        }

        /// <summary>
        /// Returns a summary of the node's view of the blockchain.
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>blockchain_state</returns>
        public async Task<BlockchainState> GetBlockchainState(CancellationToken cancellationToken = default)
        {
            return await SendMessage<BlockchainState>("get_blockchain_state", "blockchain_state", cancellationToken);
        }

        /// <summary>
        /// Get a block by a header hash
        /// </summary>
        /// <param name="headerhash">The header hash</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>block</returns>
        public async Task<FullBlock> GetBlock(string headerhash, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.header_hash = headerhash;

            return await SendMessage<FullBlock>("get_block", data, "block", cancellationToken);
        }

        /// <summary>
        /// Get the blocks between a start and end height
        /// </summary>
        /// <param name="start">Start height</param>
        /// <param name="end">End Height - non-inclusive</param>
        /// <param name="excludeHeaderhash">Flag indicating whether to include the header hash in the result or not</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>A list of blocks</returns>
        public async Task<IEnumerable<FullBlock>> GetBlocks(uint start, uint end, bool excludeHeaderhash, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.start = start;
            data.end = end;
            data.exclude_header_hash = excludeHeaderhash;

            return await SendMessage<IEnumerable<FullBlock>>("get_blocks", data, "blocks", cancellationToken);
        }

        /// <summary>
        /// Get a block record by a header hash
        /// </summary>
        /// <param name="headerhash">The header hash</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>block_record</returns>
        public async Task<BlockRecord> GetBlockRecord(string headerhash, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.header_hash = headerhash;

            return await SendMessage<BlockRecord>("get_block_record", data, "block_record", cancellationToken);
        }

        /// <summary>
        /// Retrieves a block record by height (assuming the height is less then or equal peak height)
        /// </summary>
        /// <param name="height">the height to get</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>block_record</returns>
        public async Task<BlockRecord> GetBlockRecordByHeight(uint height, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.height = height;

            return await SendMessage<BlockRecord>("get_block_record_by_height", data, "block_record", cancellationToken);
        }

        /// <summary>
        /// Retrieves block records in a range
        /// </summary>        
        /// <param name="start">Start height</param>
        /// <param name="end">End Height - non-inclusive</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>list of block_record</returns>
        public async Task<IEnumerable<BlockRecord>> GetBlockRecords(uint start, uint end, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.start = start;
            data.end = end;

            return await SendMessage<IEnumerable<BlockRecord>>("get_block_records", data, "block_records", cancellationToken);
        }

        /// <summary>
        /// Get unfinished block headers
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>A list of headers</returns>
        public async Task<IEnumerable<UnfinishedHeaderBlock>> GetUnfinishedBlockHeaders(CancellationToken cancellationToken = default)
        {
            return await SendMessage<IEnumerable<UnfinishedHeaderBlock>>("get_unfinished_block_headers", "headers", cancellationToken);
        }

        /// <summary>
        /// Retrieves the coins for a given puzzlehash, by default returns unspent coins.
        /// </summary>
        /// <param name="puzzlehash">The puzzle hash</param>
        /// <param name="includeSpendCoins">whether to include spent coins too, instead of just unspent</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>A list of coin records</returns>
        public async Task<IEnumerable<CoinRecord>> GetCoinRecordsByPuzzleHash(string puzzlehash, bool includeSpendCoins, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.puzzle_hash = puzzlehash;
            data.include_spend_coins = includeSpendCoins;

            return await SendMessage<IEnumerable<CoinRecord>>("get_coin_records_by_puzzle_hash", data, "coin_records", cancellationToken);
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
        public async Task<IEnumerable<CoinRecord>> GetCoinRecordsByPuzzleHash(string puzzlehash, uint startHeight, uint endHeight, bool includeSpendCoins, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.puzzle_hash = puzzlehash;
            data.start_height = startHeight;
            data.end_height = endHeight;
            data.include_spend_coins = includeSpendCoins;

            return await SendMessage<IEnumerable<CoinRecord>>("get_coin_records_by_puzzle_hash", data, "coin_records", cancellationToken);
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
        public async Task<IEnumerable<CoinRecord>> GetCoinRecordsByPuzzleHashes(IEnumerable<string> puzzlehashes, uint startHeight, uint endHeight, bool includeSpendCoins, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.puzzle_hash = puzzlehashes.ToList();
            data.start_height = startHeight;
            data.end_height = endHeight;
            data.include_spend_coins = includeSpendCoins;

            return await SendMessage<IEnumerable<CoinRecord>>("get_coin_records_by_puzzle_hashes", data, "coin_records", cancellationToken);
        }

        /// <summary>
        /// Retrieves a coin record by its name/id.
        /// </summary>
        /// <param name="name">The coin name</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>A coin record</returns>
        public async Task<CoinRecord> GetCoinRecordByName(string name, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.name = name;

            return await SendMessage<CoinRecord>("get_coin_record_by_name", data, "coin_record", cancellationToken);
        }

        /// <summary>
        /// Retrieves the additions and removals (state transitions) for a certain block. 
        /// Returns coin records for each addition and removal.
        /// </summary>
        /// <param name="headerhash">The header hash</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>A list of additions and a list of removals</returns>
        public async Task<(ICollection<CoinRecord> Additions, ICollection<CoinRecord> Removals)> GetAdditionsAndRemovals(string headerhash, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.header_hash = headerhash;

            var response = await SendMessage("get_additions_and_removals", data, cancellationToken);

            return (
                Converters.ToObject<ICollection<CoinRecord>>(response.additions),
                Converters.ToObject<ICollection<CoinRecord>>(response.removals));
        }

        /// <summary>
        /// Returns all items in the mempool.
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>a list of mempool items</returns>
        public async Task<IEnumerable<dynamic>> GetAllMempoolItems(CancellationToken cancellationToken = default)
        {
            var response = await SendMessage("get_all_mempool_items", cancellationToken);

            return response.mempool_items;
        }

        /// <summary>
        /// Returns a list of all transaction IDs (spend bundle hashes) in the mempool.
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>a list of tx_ids</returns>
        public async Task<IEnumerable<dynamic>> GetAllMemmpoolTxIds(CancellationToken cancellationToken = default)
        {
            var response = await SendMessage("get_all_mempool_tx_ids", cancellationToken);

            return response.tx_ids;
        }

        /// <summary>
        /// Gets a mempool item by tx id.
        /// </summary>
        /// <param name="txId">Trasnaction id</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>a list of tx_ids</returns>
        public async Task<dynamic> GetMemmpooItemByTxId(string txId, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.tx_id = txId;

            var response = await SendMessage("get_mempool_item_by_tx_id", cancellationToken);

            return response.mempool_item;
        }

        /// <summary>
        /// Retrieves an estimate of total space validating the chain between two block header hashes.
        /// </summary>
        /// <param name="newerBlockHeaderhash"></param>
        /// <param name="olderBlockHeaderhash"></param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns><see cref="BigInteger"/> of network space in bytes</returns>
        public async Task<BigInteger> GetNetworkSpace(string newerBlockHeaderhash, string olderBlockHeaderhash, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.newer_block_header_hash = newerBlockHeaderhash;
            data.older_block_header_hash = olderBlockHeaderhash;

            var response = await SendMessage("get_network_space", data, cancellationToken);

            return response.space;
        }

        /// <summary>
        /// Retrieves information about the current network
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>The network name and coin prefix</returns>
        public async Task<(string NetworkName, string NetworkPrefix)> GetNetworkInfo(CancellationToken cancellationToken = default)
        {
            var response = await SendMessage("get_network_info", cancellationToken);

            return (response.network_name, response.network_prefix);
        }

        /// <summary>
        /// Pushes a transaction / spend bundle to the mempool and blockchain. 
        /// Returns whether the spend bundle was successfully included into the mempool
        /// </summary>
        /// <param name="spendBundle"></param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>Indicator of whether the spend bundle was successfully included in the mempool</returns>
        public async Task<bool> PushTx(SpendBundle spendBundle, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.spend_bundle = spendBundle;

            var response = await SendMessage("push_tx", data, cancellationToken);

            return response.status?.ToString() == "SUCCESS";
        }

        /// <summary>
        /// Gets a recent signage point
        /// </summary>
        /// <param name="spHash">signage point hash</param> 
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>Indicator of whether the spend bundle was successfully included in the mempool</returns>
        public async Task<(dynamic signagePoint, double timeReceived, bool reverted)> GetRecentSignagePoint(string spHash, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.sp_hash = spHash;

            var response = await SendMessage("get_recent_signage_point_or_eos", data, cancellationToken);

            return (response.signage_point, response.time_received, response.reverted);
        }

        /// <summary>
        /// Gets a recent end of slot
        /// </summary>
        /// <param name="challengeHash">challenge hash</param> 
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>A signange _point or an eos</returns>
        public async Task<(dynamic eos, double timeReceived, bool reverted)> GetRecentEOS(string challengeHash, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.challenge_hash = challengeHash;

            var response = await SendMessage("get_recent_signage_point_or_eos", data, cancellationToken);

            return (response.eos, response.time_received, response.reverted);
        }


        /// <summary>
        /// Gets a coin solution 
        /// </summary>
        /// <param name="coinId">Id of the coin</param>
        /// <param name="height">Block height</param> 
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>A coin_solution</returns>
        public async Task<dynamic> GetPuzzleAndSolution(string coinId, uint height, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.coin_id = coinId;
            data.height = height;

            var response = await SendMessage("get_puzzle_and_solution", data, cancellationToken);

            return response.coin_solution;
        }
    }
}
