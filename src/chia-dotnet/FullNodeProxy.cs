using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace chia.dotnet
{
    /// <summary>
    /// Proxy that communicates with the full node
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
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>The <see cref="BlockchainState"/></returns>
        public async Task<BlockchainState> GetBlockchainState(CancellationToken cancellationToken = default)
        {
            return await SendMessage<BlockchainState>("get_blockchain_state", "blockchain_state", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Retrieves aggregate information about blocks.
        /// </summary>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>The <see cref="BlockCountMetrics"/></returns>
        public async Task<BlockCountMetrics> GetBlockCountMetrics(CancellationToken cancellationToken = default)
        {
            return await SendMessage<BlockCountMetrics>("get_block_count_metrics", "metrics", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Get a block by a header hash
        /// </summary>
        /// <param name="headerhash">The header hash</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>The <see cref="FullBlock"/></returns>
        public async Task<FullBlock> GetBlock(string headerhash, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(headerhash))
            {
                throw new ArgumentNullException(nameof(headerhash));
            }

            dynamic data = new ExpandoObject();
            data.header_hash = headerhash;

            return await SendMessage<FullBlock>("get_block", data, "block", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Get the blocks between a start and end height
        /// </summary>
        /// <param name="start">Start height</param>
        /// <param name="end">End Height - non-inclusive</param>
        /// <param name="excludeHeaderhash">Flag indicating whether to include the header hash in the result or not</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>A list of <see cref="FullBlock"/>s</returns>
        public async Task<IEnumerable<FullBlock>> GetBlocks(uint start, uint end, bool? excludeHeaderhash = null, bool? excludeReorged = null, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.start = start;
            data.end = end;
            if (excludeHeaderhash != null)
            {
                data.exclude_header_hash = excludeHeaderhash;
            }

            if (excludeReorged != null)
            {
                data.exclude_reorged = excludeReorged;
            }

            return await SendMessage<IEnumerable<FullBlock>>("get_blocks", data, "blocks", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Estimate a spend fee
        /// </summary>
        /// <param name="spendBundle">The spend bundle to esimtate</param>
        /// <param name="targetTimes">Array of target times</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>Fee estimate details</returns>
        public async Task<(IEnumerable<int> estimates,
            IEnumerable<int> targetTimes,
            ulong currentFeeRate,
            ulong mempoolSize,
            ulong mempoolMaxSize,
            bool synced,
            ulong peakHeight,
            ulong lastPeakTimestamp,
            ulong utcTimestamp
            )> GetFeeEstimate(SpendBundle spendBundle, IEnumerable<int> targetTimes, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.spend_bundle = spendBundle;
            data.target_times = targetTimes.ToList();

            var response = await SendMessage("get_fee_estimate", data, cancellationToken).ConfigureAwait(false);

            return (
                response.estimates,
                response.target_times,
                response.current_fee_rate,
                response.mempool_size,
                response.mempool_max_size,
                response.full_node_synced,
                response.peak_height,
                response.last_peak_timestamp,
                response.node_time_utc
                );
        }

        /// <summary>
        /// Estimate a spend fee
        /// </summary>
        /// <param name="spendBundle">The spend bundle to esimtate</param>
        /// <param name="targetTimes">Array of target times</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>Fee estimate details</returns>
        public async Task<(IEnumerable<int> estimates,
            IEnumerable<int> targetTimes,
            ulong currentFeeRate,
            ulong mempoolSize,
            ulong mempoolMaxSize,
            bool synced,
            ulong peakHeight,
            ulong lastPeakTimestamp,
            ulong utcTimestamp
            )> GetFeeEstimate(ulong cost, IEnumerable<int> targetTimes, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.cost = cost;
            data.target_times = targetTimes.ToList();

            var response = await SendMessage("get_fee_estimate", data, cancellationToken).ConfigureAwait(false);

            return (
                response.estimates,
                response.target_times,
                response.current_fee_rate,
                response.mempool_size,
                response.mempool_max_size,
                response.full_node_synced,
                response.peak_height,
                response.last_peak_timestamp,
                response.node_time_utc
                );
        }

        /// <summary>
        /// Get a block record by a header hash
        /// </summary>
        /// <param name="headerhash">The header hash</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>The <see cref="BlockRecord"/></returns>
        public async Task<BlockRecord> GetBlockRecord(string headerhash, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(headerhash))
            {
                throw new ArgumentNullException(nameof(headerhash));
            }

            dynamic data = new ExpandoObject();
            data.header_hash = headerhash;

            return await SendMessage<BlockRecord>("get_block_record", data, "block_record", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Retrieves a block record by height (assuming the height is less then or equal peak height)
        /// </summary>
        /// <param name="height">the height to get</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>The <see cref="BlockRecord"/></returns>
        public async Task<BlockRecord> GetBlockRecordByHeight(uint height, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.height = height;

            return await SendMessage<BlockRecord>("get_block_record_by_height", data, "block_record", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Retrieves block records in a range
        /// </summary>
        /// <param name="start">Start height</param>
        /// <param name="end">End Height - non-inclusive</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>list of <see cref="BlockRecord"/>s</returns>
        public async Task<IEnumerable<BlockRecord>> GetBlockRecords(uint start, uint end, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.start = start;
            data.end = end;

            return await SendMessage<IEnumerable<BlockRecord>>("get_block_records", data, "block_records", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Get unfinished block headers
        /// </summary>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>A list of <see cref="UnfinishedHeaderBlock"/>s</returns>
        public async Task<IEnumerable<UnfinishedHeaderBlock>> GetUnfinishedBlockHeaders(CancellationToken cancellationToken = default)
        {
            return await SendMessage<IEnumerable<UnfinishedHeaderBlock>>("get_unfinished_block_headers", "headers", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Retrieves the coins for a given puzzlehash
        /// </summary>
        /// <param name="includeSpentCoins">whether to include spent coins too, instead of just unspent</param>
        /// <param name="puzzlehash">The puzzle hash</param>
        /// <param name="startHeight">confirmation start height for search</param>
        /// <param name="endHeight">confirmation end height for search</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>A list of <see cref="CoinRecord"/>s</returns>
        public async Task<IEnumerable<CoinRecord>> GetCoinRecordsByPuzzleHash(string puzzlehash, bool includeSpentCoins, int? startHeight, int? endHeight, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(puzzlehash))
            {
                throw new ArgumentNullException(nameof(puzzlehash));
            }

            dynamic data = new ExpandoObject();
            data.puzzle_hash = puzzlehash;
            data.include_spent_coins = includeSpentCoins;

            if (startHeight.HasValue)
            {
                data.start_height = startHeight.Value;
            }

            if (endHeight.HasValue)
            {
                data.end_height = endHeight.Value;
            }

            return await SendMessage<IEnumerable<CoinRecord>>("get_coin_records_by_puzzle_hash", data, "coin_records", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Retrieves the coins for given coin IDs
        /// </summary>
        /// <param name="names">The coin names</param>
        /// <param name="includeSpentCoins">Flag indicating whether to include spent coins or not</param>
        /// <param name="startHeight">confirmation start height for search</param>
        /// <param name="endHeight">confirmation end height for search</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>A list of <see cref="CoinRecord"/>s</returns>
        public async Task<IEnumerable<CoinRecord>> GetCoinRecordsByNames(IEnumerable<string> names, bool includeSpentCoins, int? startHeight = null, int? endHeight = null, CancellationToken cancellationToken = default)
        {
            if (names is null)
            {
                throw new ArgumentNullException(nameof(names));
            }

            dynamic data = new ExpandoObject();
            data.names = names.ToList();
            data.include_spent_coins = includeSpentCoins;

            if (startHeight.HasValue)
            {
                data.start_height = startHeight.Value;
            }

            if (endHeight.HasValue)
            {
                data.end_height = endHeight.Value;
            }

            return await SendMessage<IEnumerable<CoinRecord>>("get_coin_records_by_names", data, "coin_records", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Retrieves the coins for a given list of puzzlehashes, by default returns unspent coins.
        /// </summary>
        /// <param name="puzzlehashes">The list of puzzle hashes</param>
        /// <param name="includeSpentCoins">whether to include spent coins too, instead of just unspent</param>
        /// <param name="startHeight">confirmation start height for search</param>
        /// <param name="endHeight">confirmation end height for search</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>A list of <see cref="CoinRecord"/>s</returns>
        public async Task<IEnumerable<CoinRecord>> GetCoinRecordsByPuzzleHashes(IEnumerable<string> puzzlehashes, bool includeSpentCoins, int? startHeight = null, int? endHeight = null, CancellationToken cancellationToken = default)
        {
            if (puzzlehashes is null)
            {
                throw new ArgumentNullException(nameof(puzzlehashes));
            }

            dynamic data = new ExpandoObject();
            data.puzzle_hashes = puzzlehashes.ToList();
            data.include_spent_coins = includeSpentCoins;

            if (startHeight.HasValue)
            {
                data.start_height = startHeight.Value;
            }

            if (endHeight.HasValue)
            {
                data.end_height = endHeight.Value;
            }

            return await SendMessage<IEnumerable<CoinRecord>>("get_coin_records_by_puzzle_hashes", data, "coin_records", cancellationToken).ConfigureAwait(false);
        }


        /// <summary>
        /// Retrieves the coins for a given list of parent ids
        /// </summary>
        /// <param name="parentIds">The list of parent ids hashes</param>
        /// <param name="includeSpentCoins">whether to include spent coins too, instead of just unspent</param>
        /// <param name="startHeight">confirmation start height for search</param>
        /// <param name="endHeight">confirmation end height for search</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>A list of <see cref="CoinRecord"/>s</returns>
        public async Task<IEnumerable<CoinRecord>> GetCoinRecordsByParentIds(IEnumerable<string> parentIds, bool includeSpentCoins, int? startHeight = null, int? endHeight = null, CancellationToken cancellationToken = default)
        {
            if (parentIds is null)
            {
                throw new ArgumentNullException(nameof(parentIds));
            }

            dynamic data = new ExpandoObject();
            data.parent_ids = parentIds.ToList();
            data.include_spent_coins = includeSpentCoins;

            if (startHeight.HasValue)
            {
                data.start_height = startHeight.Value;
            }

            if (endHeight.HasValue)
            {
                data.end_height = endHeight.Value;
            }

            return await SendMessage<IEnumerable<CoinRecord>>("get_coin_records_by_parent_ids", data, "coin_records", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Retrieves a coin record by its name/id.
        /// </summary>
        /// <param name="name">The coin name</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>A <see cref="CoinRecord"/></returns>
        public async Task<CoinRecord> GetCoinRecordByName(string name, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            dynamic data = new ExpandoObject();
            data.name = name;

            return await SendMessage<CoinRecord>("get_coin_record_by_name", data, "coin_record", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Retrieves a coin record by hint
        /// </summary>
        /// <param name="hint">The hint</param>
        /// <param name="includeSpentCoins">whether to include spent coins too, instead of just unspent</param>
        /// <param name="startHeight">confirmation start height for search</param>
        /// <param name="endHeight">confirmation end height for search</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>A list of <see cref="CoinRecord"/>s</returns>
        public async Task<IEnumerable<CoinRecord>> GetCoinRecordsByHint(string hint, bool includeSpentCoins, uint? startHeight = null, uint? endHeight = null, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(hint))
            {
                throw new ArgumentNullException(nameof(hint));
            }

            dynamic data = new ExpandoObject();
            data.hint = hint;
            data.include_spent_coins = includeSpentCoins;

            if (startHeight.HasValue)
            {
                data.start_height = startHeight.Value;
            }

            if (endHeight.HasValue)
            {
                data.end_height = endHeight.Value;
            }

            return await SendMessage<IEnumerable<CoinRecord>>("get_coin_records_by_hint", data, "coin_records", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Retrieves the additions and removals (state transitions) for a certain block.
        /// Returns coin records for each addition and removal.
        /// </summary>
        /// <param name="headerhash">The header hash</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>A list of additions and a list of removals</returns>
        public async Task<(IEnumerable<CoinRecord> Additions, IEnumerable<CoinRecord> Removals)> GetAdditionsAndRemovals(string headerhash, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(headerhash))
            {
                throw new ArgumentNullException(nameof(headerhash));
            }

            dynamic data = new ExpandoObject();
            data.header_hash = headerhash;

            var response = await SendMessage("get_additions_and_removals", data, cancellationToken).ConfigureAwait(false);

            return (
                Converters.ToEnumerable<CoinRecord>(response.additions),
                Converters.ToEnumerable<CoinRecord>(response.removals)
                );
        }

        /// <summary>
        /// Retrieves every coin that was spent in a block
        /// </summary>
        /// <param name="headerhash">The block's header_hash</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public async Task<IEnumerable<CoinSpend>> GetBlockSpends(string headerhash, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(headerhash))
            {
                throw new ArgumentNullException(nameof(headerhash));
            }
            dynamic data = new ExpandoObject();
            data.header_hash = headerhash;

            return await SendMessage<IEnumerable<CoinSpend>>("get_block_spends", data, "block_spends", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Returns all items in the mempool.
        /// </summary>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>A dictionary of mempool items</returns>
        public async Task<IDictionary<string, MempoolItem>> GetAllMempoolItems(CancellationToken cancellationToken = default)
        {
            return await SendMessage<IDictionary<string, MempoolItem>>("get_all_mempool_items", "mempool_items", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Returns a list of all transaction IDs (spend bundle hashes) in the mempool.
        /// </summary>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>a list of tx_ids</returns>
        public async Task<IEnumerable<string>> GetAllMemmpoolTxIds(CancellationToken cancellationToken = default)
        {
            var response = await SendMessage("get_all_mempool_tx_ids", cancellationToken).ConfigureAwait(false);

            return Converters.ToEnumerable<string>(response.tx_ids);
        }

        /// <summary>
        /// Gets a mempool item by tx id.
        /// </summary>
        /// <param name="txId">Transaction id</param>
        /// <param name="includePending">Including pending transactions</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>The <see cref="MempoolItem"/></returns>
        public async Task<MempoolItem> GetMemmpooItemByTxId(string txId, bool includePending = false, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(txId))
            {
                throw new ArgumentNullException(nameof(txId));
            }

            dynamic data = new ExpandoObject();
            data.tx_id = txId;
            data.include_pending = includePending;

            return await SendMessage<MempoolItem>("get_mempool_item_by_tx_id", data, "mempool_item", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Retrieves an estimate of total space validating the chain between two block header hashes.
        /// </summary>
        /// <param name="newerBlockHeaderhash"></param>
        /// <param name="olderBlockHeaderhash"></param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns><see cref="BigInteger"/> of network space in bytes</returns>
        public async Task<BigInteger> GetNetworkSpace(string newerBlockHeaderhash, string olderBlockHeaderhash, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(newerBlockHeaderhash))
            {
                throw new ArgumentNullException(nameof(newerBlockHeaderhash));
            }

            if (string.IsNullOrEmpty(olderBlockHeaderhash))
            {
                throw new ArgumentNullException(nameof(olderBlockHeaderhash));
            }

            dynamic data = new ExpandoObject();
            data.newer_block_header_hash = newerBlockHeaderhash;
            data.older_block_header_hash = olderBlockHeaderhash;

            var response = await SendMessage("get_network_space", data, cancellationToken).ConfigureAwait(false);

            return response.space;
        }

        /// <summary>
        /// Retrieves information about the current network
        /// </summary>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>The network name and coin prefix</returns>
        public async Task<(string NetworkName, string NetworkPrefix)> GetNetworkInfo(CancellationToken cancellationToken = default)
        {
            var response = await SendMessage("get_network_info", cancellationToken).ConfigureAwait(false);

            return (response.network_name, response.network_prefix);
        }

        /// <summary>
        /// Pushes a transaction / spend bundle to the mempool and blockchain.
        /// Returns whether the spend bundle was successfully included into the mempool
        /// </summary>
        /// <param name="spendBundle"></param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>Indicator of whether the spend bundle was successfully included in the mempool</returns>
        public async Task<bool> PushTx(SpendBundle spendBundle, CancellationToken cancellationToken = default)
        {
            if (spendBundle is null)
            {
                throw new ArgumentNullException(nameof(spendBundle));
            }

            dynamic data = new ExpandoObject();
            data.spend_bundle = spendBundle;

            var response = await SendMessage("push_tx", data, cancellationToken).ConfigureAwait(false);

            if (response.status?.ToString() == "SUCCESS")
            {
                return true;
            }

            var message = Message.Create("push_tx", data, DestinationService, OriginService);
            throw new ResponseException(message, JsonConvert.SerializeObject(response));
        }

        /// <summary>
        /// Gets a recent signage point
        /// </summary>
        /// <param name="spHash">signage point hash</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>The <see cref="SignagePoint"/></returns>
        public async Task<(SignagePoint SignagePoint, double TimeReceived, bool Reverted, DateTime DateTimeReceived)> GetRecentSignagePoint(string spHash, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(spHash))
            {
                throw new ArgumentNullException(nameof(spHash));
            }

            dynamic data = new ExpandoObject();
            data.sp_hash = spHash;

            var response = await SendMessage("get_recent_signage_point_or_eos", data, cancellationToken).ConfigureAwait(false);

            return (
                Converters.ToObject<SignagePoint>(response.signage_point),
                response.time_received,
                response.reverted,
                Converters.ToDateTime((double)response.time_received)
                );
        }

        /// <summary>
        /// Gets a recent end of slot
        /// </summary>
        /// <param name="challengeHash">challenge hash</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>The <see cref="EndOfSubSlotBundle"/></returns>
        public async Task<(EndOfSubSlotBundle eos, double timeReceived, bool reverted, DateTime DateTimeReceived)> GetRecentEOS(string challengeHash, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(challengeHash))
            {
                throw new ArgumentNullException(nameof(challengeHash));
            }

            dynamic data = new ExpandoObject();
            data.challenge_hash = challengeHash;

            var response = await SendMessage("get_recent_signage_point_or_eos", data, cancellationToken).ConfigureAwait(false);

            return (
                Converters.ToObject<EndOfSubSlotBundle>(response.eos),
                response.time_received,
                response.reverted,
                Converters.ToDateTime((double)response.time_received)
                );
        }

        /// <summary>
        /// Gets a coin solution
        /// </summary>
        /// <param name="coinId">Id/name  of the coin</param>
        /// <param name="height">Block height at which the coin was spent 'spent_block_index'</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>A <see cref="CoinSpend"/></returns>
        /// <remarks>coinId is the coin name</remarks>
        public async Task<CoinSpend> GetPuzzleAndSolution(string coinId, uint height, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(coinId))
            {
                throw new ArgumentNullException(nameof(coinId));
            }

            dynamic data = new ExpandoObject();
            data.coin_id = coinId;
            data.height = height;

            return await SendMessage<CoinSpend>("get_puzzle_and_solution", data, "coin_solution", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Estimates the average time it is taking to process the last 500 blocks
        /// </summary>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>The <see cref="TimeSpan"/> estimation</returns>
        public async Task<TimeSpan> GetAverageBlockTime(CancellationToken cancellationToken = default)
        {
            var SECONDS_PER_BLOCK = TimeSpan.FromSeconds(24 * 3600 / 4608);
            const int blocks_to_compare = 500;
            try
            {
                var blockchain_state = await GetBlockchainState(cancellationToken).ConfigureAwait(false);
                var curr = blockchain_state.Peak;

                if (curr is null || curr.Height < (blocks_to_compare + 100))
                {
                    return SECONDS_PER_BLOCK;
                }

                while (curr is not null && curr.Height > 0 && !curr.IsTransactionBlock)
                {
                    curr = await GetBlockRecord(curr.PrevHash, cancellationToken).ConfigureAwait(false);
                }

                if (curr is null)
                {
                    return SECONDS_PER_BLOCK;
                }

                var past_curr = await GetBlockRecordByHeight(curr.Height - blocks_to_compare, cancellationToken).ConfigureAwait(false);

                while (past_curr is not null && past_curr.Height > 0 && !past_curr.IsTransactionBlock)
                {
                    past_curr = await GetBlockRecord(past_curr.PrevHash, cancellationToken).ConfigureAwait(false);
                }

                return curr.Timestamp is null || past_curr is null || past_curr.Timestamp is null
                    ? SECONDS_PER_BLOCK
                    : TimeSpan.FromSeconds(((double)curr.Timestamp - (double)past_curr.Timestamp) / (curr.Height - past_curr.Height));
            }
            catch
            {
                return SECONDS_PER_BLOCK;
            }
        }
    }
}
