using System;

namespace chia.dotnet
{
    public record PlotInfo
    {
        public int FileSize { get; init; }
        public string Filename { get; init; }
        public string PlotId { get; init; }
        public string PlotPublicKey { get; init; }
        public string PoolContractPuzzleHash { get; init; }
        public string PoolPublicKey { get; init; }
        public KValues Size { get; init; }
        public double TimeModified { get; init; }

        public DateTime DateTimeModified => TimeModified.ToDateTime();
    }
}
