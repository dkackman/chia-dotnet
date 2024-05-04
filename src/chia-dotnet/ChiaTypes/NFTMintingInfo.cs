using System.Collections.Generic;

namespace chia.dotnet
{
    public record NFTMintEntry
    {
        public ushort RoyaltyPercentage { get; init; }
        public IEnumerable<string> Uris { get; init; } = [];
        public string Hash { get; init; } = string.Empty;
        public IEnumerable<string> MetaUris { get; init; } = [];
        public string? MetaHash { get; init; }
        public IEnumerable<string> LicenseUris { get; init; } = [];
        public string? LicenseHash { get; init; }
        public ulong EditionTotal { get; init; } = 1;
        public ulong EditionNumber { get; init; } = 1;
    }

    /// <summary>
    /// Info for minting an NFT
    /// </summary>
    public record NFTMintingInfo : NFTMintEntry
    {
        public string? RoyaltyAddress { get; init; }
        public string? TargetAddress { get; init; }
        public string? DidId { get; init; }
    }
}
