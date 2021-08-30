using System.Collections.Generic;

using Newtonsoft.Json;

namespace chia.dotnet
{
    /// <summary>
    /// All the information required to validate a block
    /// </summary>
    public record FullBlock
    {
        /// <summary>
        /// If first sb
        /// </summary>
        public ICollection<EndOfSubSlotBundle> FinishedSubSlots { get; init; } = new List<EndOfSubSlotBundle>();
        /// <summary>
        /// Reward chain trunk data
        /// </summary>
        public RewardChainBlock RewardChainBlock { get; init; } = new();
        /// <summary>
        /// If not first sp in sub-slot
        /// </summary>
        public VDFProof? ChallengeChainSpProof { get; init; }
        public VDFProof ChallengeChainIpProof { get; init; } = new();
        /// <summary>
        /// If not first sp in sub-slot
        /// </summary>
        public VDFProof? RewardChainSpProof { get; init; }
        public VDFProof RewardChainIpProof { get; init; } = new();
        /// <summary>
        /// # Iff deficit &lt; 4
        /// </summary>
        public VDFProof? InfusedChallengeChainIpProof { get; init; }
        /// <summary>
        /// Reward chain foliage data
        /// </summary>
        public Foliage Foliage { get; init; } = new();
        /// <summary>
        /// Reward chain foliage data (tx block)
        /// </summary>
        public FoliageTransactionBlock? FoliageTransactionBlock { get; init; }
        /// <summary>
        /// Reward chain foliage data (tx block additional)
        /// </summary>
        public TransactionsInfo? TransactionsInfo { get; init; }
        /// <summary>
        /// Program that generates transactions
        /// </summary>
        public string? TransactionsGenerator { get; init; }
        /// <summary>
        /// List of block heights of previous generators referenced in this block
        /// </summary>
        public ICollection<uint> TransactionsGeneratorRefList { get; init; } = new List<uint>();
        /// <summary>
        /// Is this block from a transaction
        /// </summary>
        [JsonIgnore]
        public bool IsTransactionBlock => RewardChainBlock.IsTransactionBlock;
    }
}
