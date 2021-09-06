namespace chia.dotnet
{
    public enum MempoolInclusionStatus : byte
    {
        /// <summary>
        /// Transaction added to mempool
        /// </summary>
        SUCCESS = 1,
        /// <summary>
        /// Transaction not yet added to mempool
        /// </summary>
        PENDING = 2,
        /// <summary>
        /// Transaction was invalid and dropped
        /// </summary>
        FAILED = 3
    }
}
