using System.Collections.Generic;

namespace chia.dotnet
{
    /// <summary>
    /// NFT Info for displaying NFT on the UI
    /// </summary>
    public record NFTInfo
    {
        /// <summary>
        /// Launcher coin ID
        /// </summary>
        public string LauncherId { get; init; } = string.Empty;
        /// <summary>
        /// Current NFT coin ID
        /// </summary>
        public string NFTCoinID { get; init; } = string.Empty;
        /// <summary>
        /// Owner DID
        /// </summary>
        public string? OwnerDID { get; init; }
        /// <summary>
        /// Percentage of the transaction fee paid to the author, e.g. 1000 = 1%
        /// </summary>
        public ushort? RoyaltyPercentage { get; init; }
        /// <summary>
        /// uzzle hash where royalty will be sent to
        /// </summary>
        public string? RoyaltyPuzzleHash { get; init; } = string.Empty;
        /// <summary>
        /// A list of content URIs
        /// </summary>
        public IEnumerable<string> DataUris { get; init; } = new List<string>();
        /// <summary>
        /// Hash of the content
        /// </summary>
        public string DataHash { get; init; } = string.Empty;
        /// <summary>
        /// A list of metadata URIs
        /// </summary>
        public IEnumerable<string> MetadataUris { get; init; } = new List<string>();
        /// <summary>
        /// Hash of the metadata
        /// </summary>
        public string MetaataHash { get; init; } = string.Empty;
        /// <summary>
        /// A list of license URIs
        /// </summary>
        public IEnumerable<string> LicenseUris { get; init; } = new List<string>();
        /// <summary>
        /// Hash of the license
        /// </summary>
        public string LicenseHash { get; init; } = string.Empty;
        /// <summary>
        /// How many NFTs in the current edition
        /// </summary>
        public ulong EditionTotal { get; init; }
        /// <summary>
        /// Number of the current NFT in the edition
        /// </summary>
        public ulong EditionNumber { get; init; }
        /// <summary>
        /// Puzzle hash of the metadata updater in hex
        /// </summary>
        public string UpdaterPuzhash { get; init; } = string.Empty;
        /// <summary>
        /// Information saved on the chain in hex
        /// </summary>
        public string ChainInfo { get; init; } = string.Empty;
        /// <summary>
        /// Block height of the NFT minting
        /// </summary>
        public uint MintHeight { get; init; }
        /// <summary>
        /// If the inner puzzle supports DID
        /// </summary>
        public bool SupportsDID { get; init; }
        /// <summary>
        /// Indicate if the NFT is pending for a transaction
        /// </summary>
        public bool PendingTransaction { get; init; }
        /// <summary>
        /// Puzzle hash of the singleton launcher in hex
        /// </summary>
        public string LauncherPuzhash { get; init; } = string.Empty;
        /// <summary>
        /// DID of the NFT minter
        /// </summary>
        public string? MinterDID { get; init; }
        /// <summary>
        /// Serialized off-chain metadata
        /// </summary>
        public string? OffChainMetadata { get; init; }
        /// <summary>
        /// The innermost puzzle hash of the NFT
        /// </summary>
        public string P2Address { get; init; } = string.Empty;
    }
}
