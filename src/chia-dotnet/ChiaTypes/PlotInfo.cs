using System;

using Newtonsoft.Json;

namespace chia.dotnet
{
    /// <summary>
    /// Info about a plot file
    /// </summary>
    public record PlotInfo
    {
        public ulong FileSize { get; init; }
        public string Filename { get; init; } = string.Empty;
        public string PlotId { get; init; } = string.Empty;
        public string? PlotPublicKey { get; init; }
        public string PoolContractPuzzleHash { get; init; } = string.Empty;
        public string? PoolPublicKey { get; init; }
        public KValues Size { get; init; }
        public double TimeModified { get; init; }
        [JsonIgnore]
        public DateTime DateTimeModified => TimeModified.ToDateTime();
    }
}
