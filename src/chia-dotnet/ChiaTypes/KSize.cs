namespace chia.dotnet
{
    /// <summary>
    /// Valid plot sizes
    /// https://github.com/Chia-Network/chia-blockchain/wiki/k-sizes
    /// </summary>
    public enum KSize
    {
        /// <summary>
        /// Valid for testing only - <see cref="PlotterConfig.OverrideK"/> must be true in order to use
        /// </summary>
        K25 = 25,
        K32 = 32,
        K33 = 33,
        K34 = 34,
        K35 = 35,
    }
}
