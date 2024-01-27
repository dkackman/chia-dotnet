using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Threading;
using System.Threading.Tasks;

namespace chia.dotnet
{
    /// <summary>
    /// Proxy that communicates with the farmer
    /// </summary>
    /// <remarks>
    /// ctor
    /// </remarks>
    /// <param name="rpcClient"><see cref="IRpcClient"/> instance to use for rpc communication</param>
    /// <param name="originService"><see cref="Message.Origin"/></param>
    public sealed class FarmerProxy(IRpcClient rpcClient, string originService) : ServiceProxy(rpcClient, ServiceNames.Farmer, originService)
    {
        /// <summary>
        /// Event raised when a new signage point is received
        /// </summary>
        public event EventHandler<dynamic>? NewSignagePoint;

        /// <summary>
        /// Event raised when a new signage point is received
        /// </summary>
        public event EventHandler<dynamic>? NewFarmingInfo;

        /// <summary>
        /// Event raised when a proof message arrives
        /// </summary>
        public event EventHandler<dynamic>? Proof;

        /// <summary>
        /// Event raised when a partial fails
        /// </summary>
        public event EventHandler<dynamic>? PartialFailed;

        /// <summary>
        /// Event raised when a partial is submitted
        /// </summary>
        public event EventHandler<dynamic>? PartialSubmitted;

        /// <summary>
        /// Event raised when a harvester is updated
        /// </summary>
        public event EventHandler<dynamic>? HarvesterUpdated;

        /// <summary>
        /// Event raised when a harvester is removed
        /// </summary>
        public event EventHandler<dynamic>? HarvesterRemoved;

        /// <summary>
        /// <see cref="ServiceProxy.OnEventMessage(Message)"/>
        /// </summary>
        /// <param name="msg"></param>
        protected override void OnEventMessage(Message msg)
        {
            if (msg.Command == "new_signage_point")
            {
                NewSignagePoint?.Invoke(this, msg.Data);
            }
            else if (msg.Command == "new_farming_info")
            {
                NewFarmingInfo?.Invoke(this, msg.Data);
            }
            else if (msg.Command == "proof")
            {
                Proof?.Invoke(this, msg.Data);
            }
            else if (msg.Command == "failed_partial")
            {
                PartialFailed?.Invoke(this, msg.Data);
            }
            else if (msg.Command == "submitted_partial")
            {
                PartialSubmitted?.Invoke(this, msg.Data);
            }
            else if (msg.Command == "harvester_update")
            {
                HarvesterUpdated?.Invoke(this, msg.Data);
            }
            else if (msg.Command == "harvester_removed")
            {
                HarvesterRemoved?.Invoke(this, msg.Data);
            }
            else
            {
                base.OnEventMessage(msg);
            }
        }

        /// <summary>
        /// Get the farm and pool reward targets 
        /// </summary>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>the farm and pool reward targets</returns>
        public async Task<(string FarmerTarget, string PoolTarget)> GetRewardTargets(CancellationToken cancellationToken = default)
        {
            return await GetRewardTargets(500, cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Get the farm and pool reward targets 
        /// </summary>
        /// <param name="maxPhToSearch">The max number of puzzle hashes to search</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>the farm and pool reward targets</returns>
        public async Task<(string FarmerTarget, string PoolTarget)> GetRewardTargets(int maxPhToSearch, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.search_for_private_key = false;
            data.max_ph_to_search = maxPhToSearch;

            var response = await SendMessage("get_reward_targets", data, cancellationToken).ConfigureAwait(false);

            return (response.farmer_target, response.pool_target);
        }

        /// <summary>
        /// Get the farm and pool reward targets, including private keys in the search
        /// </summary>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>The farm and pool reward targets</returns>
        public async Task<(string FarmerTarget, string PoolTarget, bool HaveFarmerSk, bool HavePoolSk)> GetRewardTargetsIncludingPrivateKey(CancellationToken cancellationToken = default)
        {
            return await GetRewardTargetsIncludingPrivateKey(500, cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Get the farm and pool reward targets, including private keys in the search
        /// </summary>
        /// <param name="maxPhToSearch">The max number of puzzle hashes to search</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>The farm and pool reward targets plus indicator if privakte keys are present</returns>
        public async Task<(string FarmerTarget, string PoolTarget, bool HaveFarmerSk, bool HavePoolSk)> GetRewardTargetsIncludingPrivateKey(int maxPhToSearch = 500, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.search_for_private_key = true;
            data.max_ph_to_search = maxPhToSearch;

            var response = await SendMessage("get_reward_targets", data, cancellationToken).ConfigureAwait(false);

            return (response.farmer_target, response.pool_target, response.have_farmer_sk, response.have_pool_sk);
        }

        /// <summary>
        /// Sets the farm and pool targets for the farmer
        /// </summary>
        /// <param name="farmerTarget">Farmer target</param>
        /// <param name="poolTarget">Pool target</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>An awaitable <see cref="Task"/></returns>
        public async Task SetRewardTargets(string farmerTarget, string poolTarget, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(farmerTarget))
            {
                throw new ArgumentNullException(nameof(farmerTarget));
            }

            if (string.IsNullOrEmpty(poolTarget))
            {
                throw new ArgumentNullException(nameof(poolTarget));
            }

            dynamic data = new ExpandoObject();
            data.farmer_target = farmerTarget;
            data.pool_target = poolTarget;

            await SendMessage("set_reward_targets", data, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Get signage points
        /// </summary>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>List of signage points</returns>
        public async Task<IEnumerable<(IEnumerable<(string SpHash, ProofOfSpace ProofOfSpace)> Proofs, FarmerSignagePoint SignagePoint)>> GetSignagePoints(CancellationToken cancellationToken = default)
        {
            var response = await SendMessage("get_signage_points", cancellationToken).ConfigureAwait(false);

            // proofs are List[Tuple[str, ProofOfSpace]]
            var list = new List<(IEnumerable<(string SpHash, ProofOfSpace ProofOfSpace)> Proofs, FarmerSignagePoint SignagePoint)>();
            foreach (var sp in response.signage_points)
            {
                if (sp is not null)
                {
                    list.Add(ConvertSignagePoint(sp));
                }
            }
            return list;
        }

        private static (IEnumerable<(string SpHash, ProofOfSpace ProofOfSpace)> Proofs, FarmerSignagePoint SignagePoint) ConvertSignagePoint(dynamic sp)
        {
            Debug.Assert(sp is not null);

            var proofs = new List<(string SpHash, ProofOfSpace ProofOfSpace)>();
            foreach (var proof in sp.proofs)
            {
                var hash = proof[0];
                var proofOfSpace = Converters.ToObject<ProofOfSpace>(hash[1]);
                proofs.Add((hash, proofOfSpace));
            }
            var signagePoint = Converters.ToObject<FarmerSignagePoint>(sp.signage_point);
            return (proofs, signagePoint);
        }

        /// <summary>
        /// Get's a signage point by hash
        /// </summary>
        /// <param name="spHash">signage point hash</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>a signage point and proofs of space</returns>
        public async Task<(IEnumerable<(string SpHash, ProofOfSpace ProofOfSpace)> Proofs, FarmerSignagePoint SignagePoint)> GetSignagePoint(string spHash, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(spHash))
            {
                throw new ArgumentNullException(nameof(spHash));
            }

            dynamic data = new ExpandoObject();
            data.sp_hash = spHash;

            var response = await SendMessage("get_signage_point", data, cancellationToken).ConfigureAwait(false);

            return ConvertSignagePoint(response);
        }

        /// <summary>
        /// Get the list of harvesters
        /// </summary>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>A list of harvesters</returns>
        public async Task<IEnumerable<HarvesterInfo>> GetHarvesters(CancellationToken cancellationToken = default)
        {
            return await SendMessage<IEnumerable<HarvesterInfo>>("get_harvesters", "harvesters", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Get a summary of harvesters
        /// </summary>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>A list of harvesters</returns>
        public async Task<IEnumerable<HarvesterSummary>> GetHarvestersSummary(CancellationToken cancellationToken = default)
        {
            return await SendMessage<IEnumerable<HarvesterSummary>>("get_harvesters_summary", "harvesters", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Get a paginated list of valid plots
        /// </summary>
        /// <param name="requestData">Info about the request</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>A page of valid plots</returns>
        public async Task<PaginatedPlotRequest> GetHarvesterPlotsValid(PlotInfoRequestData requestData, CancellationToken cancellationToken = default)
        {
            return await SendMessage<PaginatedPlotRequest>("get_harvester_plots_valid", requestData, null, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Get a paginated list of invalid plots
        /// <param name="requestData">Info about the request</param>
        /// </summary>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>A page of invalid plots</returns>
        public async Task<PaginatedPlotRequest> GetHarvesterPlotsInvalid(PlotPathRequestData requestData, CancellationToken cancellationToken = default)
        {
            return await SendMessage<PaginatedPlotRequest>("get_harvester_plots_invalid", requestData, null, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Get a paginated list of plots with missing keys
        /// </summary>
        /// <param name="requestData">Info about the request</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>A page of plots with missing keys</returns>
        public async Task<PaginatedPlotRequest> GetHarvesterPlotsKeysMissing(PlotPathRequestData requestData, CancellationToken cancellationToken = default)
        {
            return await SendMessage<PaginatedPlotRequest>("get_harvester_plots_keys_missing", requestData, null, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Get a paginated list of duplicate plots
        /// </summary>
        /// <param name="requestData">Info about the request</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>A page of duplicate plots</returns>
        public async Task<PaginatedPlotRequest> GetHarvesterPlotsDuplicates(PlotPathRequestData requestData, CancellationToken cancellationToken = default)
        {
            return await SendMessage<PaginatedPlotRequest>("get_harvester_plots_duplicates", requestData, null, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Get's the pool login link, if any
        /// </summary>
        /// <param name="launcherID">The id of the pool launcher</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>The link</returns>
        public async Task<string> GetPoolLoginLink(string launcherID, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(launcherID))
            {
                throw new ArgumentNullException(nameof(launcherID));
            }

            dynamic data = new ExpandoObject();
            data.launcher_id = launcherID;

            var response = await SendMessage("get_pool_login_link", data, cancellationToken).ConfigureAwait(false);

            return response.login_link;
        }

        /// <summary>
        /// Get's the state of the pool
        /// </summary>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>A list of pool states</returns>
        public async Task<IEnumerable<PoolStateInfo>> GetPoolState(CancellationToken cancellationToken = default)
        {
            return await SendMessage<IEnumerable<PoolStateInfo>>("get_pool_state", "pool_state", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Set's a pool's payout instructions
        /// </summary>
        /// <param name="launcherID">The id of the pool launcher</param>
        /// <param name="payoutInstructions">The instructions</param>
        /// <param name="cancellationToken">A token to allow the call to be cancelled</param>
        /// <returns>An awaitable <see cref="Task"/></returns>
        public async Task SetPayoutInstructions(string launcherID, string payoutInstructions, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(launcherID))
            {
                throw new ArgumentNullException(nameof(launcherID));
            }

            if (string.IsNullOrEmpty(payoutInstructions))
            {
                throw new ArgumentNullException(nameof(payoutInstructions));
            }

            dynamic data = new ExpandoObject();
            data.launcher_id = launcherID;
            data.payout_instructions = payoutInstructions;

            await SendMessage("set_payout_instructions", data, cancellationToken).ConfigureAwait(false);
        }
    }
}
