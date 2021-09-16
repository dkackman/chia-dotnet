using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace chia.dotnet
{
    /// <summary>
    /// From the user's point of view, a pool group can be in these states:
    /// `SELF_POOLING`: The singleton exists on the blockchain, and we are farming
    ///     block rewards to a wallet address controlled by the user
    /// 
    /// `LEAVING_POOL`: The singleton exists, and we have entered the "escaping" state, which
    ///     means we are waiting for a number of blocks = `relative_lock_height` to pass, so we can leave.
    /// 
    /// `FARMING_TO_POOL`: The singleton exists, and it is assigned to a pool.
    /// 
    /// `CLAIMING_SELF_POOLED_REWARDS`: We have submitted a transaction to sweep our
    ///     self-pooled funds.
    /// </summary>    
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PoolSingletonState
    {
        /// <summary>
        /// The singleton exists on the blockchain, and we are farming
        ///    block rewards to a wallet address controlled by the user
        /// </summary>
        SELF_POOLING = 1,
        /// <summary>
        /// The singleton exists, and we have entered the "escaping" state, which
        /// means we are waiting for a number of blocks = `relative_lock_height` to pass, so we can leave.
        /// </summary>
        LEAVING_POOL = 2,
        /// <summary>
        /// The singleton exists, and it is assigned to a pool.
        /// </summary>
        FARMING_TO_POOL = 3
    }
}
