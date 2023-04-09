using System.Collections.Generic;

namespace chia.dotnet
{
    /// <summary>
    /// NFT Info for displaying NFT on the UI
    /// </summary>
    public record NFTInfo
    {
        public string ChainInfo { get; init; } = string.Empty;
        public string DataHash { get; init; } = string.Empty;
        public IEnumerable<string> DataUris { get; init; } = new List<string>();
        public ulong EditionTotal { get; init; }
        public ulong EditionNumber { get; init; }
        public string LauncherId { get; init; } = string.Empty;
        public string LauncherPuzhash { get; init; } = string.Empty;
        public string LicenseHash { get; init; } = string.Empty;
        public IEnumerable<string> LicenseUris { get; init; } = new List<string>();
        public string MetadataHash { get; init; } = string.Empty;
        public IEnumerable<string> MetadataUris { get; init; } = new List<string>();
        public uint MintHeight { get; init; }
        public string? MinterDid { get; init; }
        public string NftCoinId { get; init; } = string.Empty;
        public object? OffChainMetadata { get; init; }
        public string? OwnerDID { get; init; }
        public string? P2Address { get; init; }
        public bool PendingTransaction { get; init; }
        public ushort? RoyaltyPercentage { get; init; }
        public string? RoyaltyPuzzleHash { get; init; } = string.Empty;
        public bool SupportsDID { get; init; }
        public string UpdaterPuzhash { get; init; } = string.Empty;
    }
}
