using System.Collections.Generic;

namespace chia.dotnet
{
    /// <summary>
    /// Info for minting NFTs in bulk
    /// </summary>
    public record NFTBulkMintingInfo
    {
        public string? RoyaltyAddress { get; init; }
        public ushort? RoyaltyPercentage { get; init; }
        public IEnumerable<NftMintEntry> MetadataList { get; init; } = new List<NftMintEntry>();
        /// <summary>
        /// a list of targets for transferring minted NFTs (aka airdrop)
        /// </summary>
        public IEnumerable<string>? TargetList { get; init; }
        /// <summary>
        /// The starting point for mint number used in intermediate launcher puzzle
        /// </summary>
        public int MintNumberStart { get; init; } = 1;
        /// <summary>
        /// The total number of NFTs being minted
        /// </summary>
        public int? MintTotal { get; init; }
        /// <summary>
        /// For use with bulk minting to provide the coin used for funding the minting spend.
        /// This coin can be one that will be created in the future
        /// </summary>
        public IEnumerable<Coin>? XchCoins { get; init; }
        /// <summary>
        /// For use with bulk minting, so we can specify the puzzle hash that the change
        /// from the funding transaction goes to.
        /// </summary>
        public string? XchChangeTarget { get; init; }
        public string? NewInnerpuzhash { get; init; }
        public string? NewP2Puzhash { get; init; }
        public Coin? DidCoin { get; init; }
        public string? DidLineageParentHex { get; init; }
        public bool MintFromDid { get; init; }
    }
}
