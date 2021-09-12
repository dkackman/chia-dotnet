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
    public sealed class FarmerProxy : ServiceProxy
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="rpcClient"><see cref="IRpcClient"/> instance to use for rpc communication</param>
        /// <param name="originService"><see cref="Message.Origin"/></param>
        public FarmerProxy(IRpcClient rpcClient, string originService)
            : base(rpcClient, ServiceNames.Farmer, originService)
        {
        }

        /// <summary>
        /// Get the farm and pool reward targets 
        /// </summary>
        /// <param name="searchForPrivateKey">Include private key in search</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>the farm and pool reward targets</returns>
        public async Task<(string FarmerTarget, string PoolTarget)> GetRewardTargets(bool searchForPrivateKey, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.search_for_private_key = searchForPrivateKey;

            var response = await SendMessage("get_reward_targets", data, cancellationToken).ConfigureAwait(false);

            return (response.farmer_target, response.pool_target);
        }

        /// <summary>
        /// Sets the farm and pool targets for the farmer
        /// </summary>
        /// <param name="farmerTarget">Farmer target</param>
        /// <param name="poolTarget">Pool target</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
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

            _ = await SendMessage("set_reward_targets", data, cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Get signage points
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
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
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
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
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>A list of harvesters</returns>
        public async Task<IEnumerable<HarvesterInfo>> GetHarvesters(CancellationToken cancellationToken = default)
        {
            return await SendMessage<IEnumerable<HarvesterInfo>>("get_harvesters", "harvesters", cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Get's the pool login link, if any
        /// </summary>
        /// <param name="launcherID">The id of the pool launcher</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
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
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
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
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
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

            _ = await SendMessage("set_payout_instructions", data, cancellationToken).ConfigureAwait(false);
        }
    }
}
