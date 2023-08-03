using System.Collections.Generic;

namespace chia.dotnet
{
    /// <summary>
    /// Info for minting an NFT
    /// </summary>
    public record NFTMintingInfo
    {
        public string RoyaltyAddress { get; init; } = string.Empty;
        public string TargetAddress { get; init; } = string.Empty;
        public string? DIDID { get; init; }
        public ushort RoyaltyPercentage { get; init; } = 0;
        public IEnumerable<string> Uris { get; init; } = new List<string>();
        public string Hash { get; init; } = string.Empty;
        public IEnumerable<string> MetaUris { get; init; } = new List<string>();
        public string? MetaHash { get; init; }
        public IEnumerable<string> LicenseUris { get; init; } = new List<string>();
        public string? LicenseHash { get; init; }
        public ulong EditionTotal { get; init; } = 1;
        public ulong EditionNumber { get; init; } = 1;
    }
}
