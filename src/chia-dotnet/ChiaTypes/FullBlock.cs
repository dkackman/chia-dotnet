using System;
using System.Collections.Generic;
using System.Numerics;

namespace chia.dotnet
{
    /// <summary>
    /// Represents a classgroup element (a,b,c) where a, b, and c are 512 bit signed integers. However this is using
    /// a compressed representation. VDF outputs are a single classgroup element. VDF proofs can also be one classgroup
    /// element(or multiple).
    /// </summary>
    public record ClassGroupElement
    {
        public string Data { get; init; }
    }

    public record VDFInfo
    {
        /// <summary>
        /// Used to generate the discriminant (VDF group)
        /// </summary>
        public string Challenge { get; init; }
        public ulong NumberOfIterations { get; init; }
        public ClassGroupElement VdfOutput { get; init; }
    }

    public record ChallengeChainSubSlot
    {
        public VDFInfo ChallengeChainEndOfSlotVdf { get; init; }
        /// <summary>
        /// Only at the end of a slot
        /// </summary>
        public string InfusedChallengeChainSubSlotHash { get; init; }
        /// <summary>
        /// Only once per sub-epoch, and one sub-epoch delayed
        /// </summary>
        public string SubepochSummaryHash { get; init; }
        /// <summary>
        /// Only at the end of epoch, sub-epoch, and slot
        /// </summary>
        public ulong? NewSubSlotIters { get; init; }
        /// <summary>
        /// Only at the end of epoch, sub-epoch, and slot
        /// </summary>
        public ulong? NewDifficulty { get; init; }
    }

    public record InfusedChallengeChainSubSlot
    {
        public VDFInfo InfusedChallengeChainEndOfSlotVdf { get; init; }
    }

    public record RewardChainSubSlot
    {
        public VDFInfo EndOfSlotVdf { get; init; }
        public string ChallengeChainSubSlotHash { get; init; }
        public string InfusedChallengeChainSubSlotHash { get; init; }
        /// <summary>
        /// 16 or less. usually zero
        /// </summary>
        public byte Deficit { get; init; }
    }

    public record VDFProof
    {
        public byte WitnessType { get; init; }
        public string Witness { get; init; }
        public bool NormalizedToIdentity { get; init; }
    }

    public record SubSlotProofs
    {
        public VDFProof ChallengeChainSlotProof { get; init; }
        public VDFProof InfusedChallengeChainSlotProof { get; init; }
        public VDFProof RewardChainSlotProof { get; init; }
    }

    public record ProofOfSpace
    {
        public string Challenge { get; init; }
        /// <summary>
        /// Only one of these two should be present
        /// </summary>
        public string PublicPoolKey { get; init; }
        public string PoolContractPuzzleHash { get; init; }
        public string PlotPublicKey { get; init; }
        public byte Size { get; init; }
        public string Proof { get; init; }
    }

    public record RewardChainBlock
    {
        public BigInteger Weight { get; init; }
        public uint Height { get; init; }
        public BigInteger TotalIters { get; init; }
        public byte SignagePointIndex { get; init; }
        public string PosSsCcChallengeHash { get; init; }
        public ProofOfSpace ProofOfSpace { get; init; }
        /// <summary>
        /// Not present for first sp in slot
        /// </summary>
        public VDFInfo ChallengeChainSpVdf { get; init; }
        public string ChallengeChainSpSignature { get; init; }
        public VDFInfo ChallengeChainIpVdf { get; init; }
        /// <summary>
        /// Not present for first sp in slot
        /// </summary>
        public VDFInfo RewardChainSpVdf { get; init; }
        public string RewardChainSpSignature { get; init; }
        public VDFInfo RewardChainIpVdf { get; init; }
        /// <summary>
        /// Iff deficit &lt; 16
        /// </summary>
        public VDFInfo InfusedChallengeChainIpVdf { get; init; }
        public bool IsTransactionBlock { get; init; }
    }

    public record PoolTarget
    {
        public string PuzzleHash { get; init; }
        /// <summary>
        /// A max height of 0 means it is valid forever
        /// </summary>
        public uint MaxHeight { get; init; }
    }

    /// <summary>
    /// Part of the block that is signed by the plot key
    /// </summary>
    public record FoliageBlockData
    {
        public string UnfinishedRewardBlockHash { get; init; }
        public PoolTarget PoolTarget { get; init; }
        /// <summary>
        /// Iff ProofOfSpace has a pool pk
        /// </summary>
        public string PoolSignature { get; init; }
        public string FarmerRewardPuzzleHash { get; init; }
        /// <summary>
        /// Used for future updates. Can be any 32 byte value initially
        /// </summary>
        public string ExtensionData { get; init; }
    }

    /// <summary>
    /// The entire foliage block, containing signature and the unsigned back pointer
    /// The hash of this is the "header hash". Note that for unfinished blocks, the prev_block_hash
    /// Is the prev from the signage point, and can be replaced with a more recent block
    /// </summary>
    public record Foliage
    {
        public string PrevBlockHash { get; init; }
        public string RewardBlockHash { get; init; }
        public FoliageBlockData FoliageBlockData { get; init; }
        public string FoliageBlockDataSignature { get; init; }
        public string FoliageTransactionBlockHash { get; init; }
        public string FoliageTransactionBlockSignature { get; init; }
    }

    /// <summary>
    /// Information that goes along with each transaction block that is relevant for light clients
    /// </summary>
    public record FoliageTransactionBlock
    {
        public string PrevTransactionBlockHash { get; init; }
        public ulong Timestamp { get; init; }
        public string FilterHash { get; init; }
        public string AdditionsRoot { get; init; }
        public string RemovalsRoot { get; init; }
        public string TransactionsInfoHash { get; init; }

        public DateTime? DateTimestamp => Timestamp.ToDateTime();
    }

    /// <summary>
    /// Information that goes along with each transaction block
    /// </summary>
    public record TransactionsInfo
    {
        /// <summary>
        /// sha256 of the block generator in this block
        /// </summary>
        public string GeneratorRoot { get; init; }
        /// <summary>
        /// sha256 of the concatenation of the generator ref list entries
        /// </summary>
        public string GeneratorRefsRoot { get; init; }
        public string AggregatedSignature { get; init; }
        /// <summary>
        /// This only includes user fees, not block rewards
        /// </summary>
        public ulong Fees { get; init; }
        /// <summary>
        /// This is the total cost of this block, including CLVM cost, cost of program size and conditions
        /// </summary>
        public ulong Cost { get; init; }
        /// <summary>
        /// These can be in any order
        /// </summary>
        public ICollection<Coin> RewardClaimsIncorporated { get; init; }
    }

    /// <summary>
    /// All the information required to validate a block
    /// </summary>
    public record FullBlock
    {
        /// <summary>
        /// If first sb
        /// </summary>
        public ICollection<EndOfSubSlotBundle> FinishedSubSlots { get; init; }
        /// <summary>
        /// Reward chain trunk data
        /// </summary>
        public RewardChainBlock RewardChainBlock { get; init; }
        /// <summary>
        /// If not first sp in sub-slot
        /// </summary>
        public VDFProof ChallengeChainSpProof { get; init; }
        public VDFProof ChallengeChainIpProof { get; init; }
        /// <summary>
        /// If not first sp in sub-slot
        /// </summary>
        public VDFProof RewardChainSpProof { get; init; }
        public VDFProof RewardChainIpProof { get; init; }
        /// <summary>
        /// # Iff deficit &lt; 4
        /// </summary>
        public VDFProof InfusedChallengeChainIpProof { get; init; }
        /// <summary>
        /// Reward chain foliage data
        /// </summary>
        public Foliage Foliage { get; init; }
        /// <summary>
        /// Reward chain foliage data (tx block)
        /// </summary>
        public FoliageTransactionBlock FoliageTransactionBlock { get; init; }
        /// <summary>
        /// Reward chain foliage data (tx block additional)
        /// </summary>
        public TransactionsInfo TransactionsInfo { get; init; }
        /// <summary>
        /// Program that generates transactions
        /// </summary>
        public string TransactionsGenerator { get; init; }
        /// <summary>
        /// List of block heights of previous generators referenced in this block
        /// </summary>
        public ICollection<uint> TransactionsGeneratorRefList { get; init; }
    }
}
