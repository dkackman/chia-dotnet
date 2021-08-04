using System.Dynamic;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace chia.dotnet
{
    /// <summary>
    /// Proxy that communicates with the farmer via the daemon
    /// </summary>
    public sealed class FarmerProxy : ServiceProxy
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="rpcClient">The <see cref="IRpcClient"/> to handle RPC</param>
        public FarmerProxy(IRpcClient rpcClient)
            : base(rpcClient)
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

            var response = await SendMessage("get_reward_targets", data, cancellationToken);

            return (response.Data.farmer_target, response.Data.pool_target);
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
            dynamic data = new ExpandoObject();
            data.farmer_target = farmerTarget;
            data.pool_target = poolTarget;

            _ = await SendMessage("set_reward_targets", data, cancellationToken);
        }

        /// <summary>
        /// Get signage points
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>List of signage points</returns>
        public async Task<IEnumerable<dynamic>> GetSignagePoints(CancellationToken cancellationToken = default)
        {
            var response = await SendMessage("get_signage_points", cancellationToken);

            return response.Data.signage_points;
        }

        /// <summary>
        /// Get's a signage point by hash
        /// </summary>
        /// <param name="spHash">signage point hash</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>a signage point and proofs of space</returns>
        public async Task<(dynamic SignagePoint, IEnumerable<dynamic> Proofs)> GetSignagePoint(string spHash, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.sp_hash = spHash;

            var response = await SendMessage("get_signage_point", data, cancellationToken);

            return (response.Data.signage_point, response.Data.proofs);
        }

        /// <summary>
        /// Get the list of harvesters
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>A list of harvesters</returns>
        public async Task<IEnumerable<dynamic>> GetHarvesters(CancellationToken cancellationToken = default)
        {
            var response = await SendMessage("get_harvesters", cancellationToken);

            return response.Data.harvesters;
        }

        /// <summary>
        /// Get's the pool login link, if any
        /// </summary>
        /// <param name="launcherID">The id of the pool launcher</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>The link</returns>
        public async Task<string> GetPoolLoginLink(string launcherID, CancellationToken cancellationToken = default)
        {
            dynamic data = new ExpandoObject();
            data.launcher_id = launcherID;

            var response = await SendMessage("get_pool_login_link", data, cancellationToken);

            return response.Data.login_link?.ToString();
        }

        /// <summary>
        /// Get's the state of the pool
        /// </summary>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>A list of pool states</returns>
        public async Task<IEnumerable<dynamic>> GetPoolState(CancellationToken cancellationToken = default)
        {
            var response = await SendMessage("get_pool_state", cancellationToken);

            return response.Data.pool_state;
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
            dynamic data = new ExpandoObject();
            data.launcher_id = launcherID;
            data.payout_instructions = payoutInstructions;

            _ = await SendMessage("set_payout_instructions", data, cancellationToken);
        }
    }
}
