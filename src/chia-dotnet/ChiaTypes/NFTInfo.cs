using System.Collections.Generic;

namespace chia.dotnet
{
    /// <summary>
    /// NFT Info for displaying NFT on the UI
    /// </summary>
    public record NFTInfo
    {
        public string LauncherId { get; init; } = string.Empty;
        public string NFTCoinID { get; init; } = string.Empty;
        public string? OwnerDID { get; init; }
        public ushort? RoyaltyPercentage { get; init; }
        public string? RoyaltyPuzzleHash { get; init; } = string.Empty;
        public IEnumerable<string> DataUris { get; init; } = new List<string>();
        public string DataHash { get; init; } = string.Empty;
        public IEnumerable<string> MetadataUris { get; init; } = new List<string>();
        public string MetaataHash { get; init; } = string.Empty;
        public IEnumerable<string> LicenseUris { get; init; } = new List<string>();
        public ulong EditionTotal { get; init; }
        public ulong EditionNumber { get; init; }
        public string UpdaterPuzhash { get; init; } = string.Empty;
        public string ChainInfo { get; init; } = string.Empty;
        public uint MintHeight { get; init; }
        public bool SupportsDID { get; init; }
        public bool PendingTransaction { get; init; }
        public string LauncherPuzhash { get; init; } = string.Empty;
        public string MinterDID { get; init; } = string.Empty;
        public string OffChainMetadata { get; init; } = string.Empty;
        public string P2Address { get; init; } = string.Empty;
    }
}
