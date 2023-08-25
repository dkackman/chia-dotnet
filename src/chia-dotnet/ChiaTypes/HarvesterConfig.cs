namespace chia.dotnet
{
    public record HarvesterConfig
    {
        public bool UseGpuHarvesting { get; init; }
        public int GpuIndex { get; init; }
        public bool EnforceGpuIndex { get; init; }
        public bool DisableCpuAffinity { get; init; }
        public int ParallelDecompressorCount { get; init; }
        public int DecompressorThreadCount { get; init; }
        public bool RecursivePlotScan { get; init; }
        public uint RefreshParameterIntervalSeconds { get; init; }
    }
}
