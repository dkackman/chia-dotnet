using System.Numerics;
using System.Collections.Generic;

namespace chia.dotnet
{
    public record ClassGroupElement
    {
        public string Data { get; init; }
    }

    public record VDFInfo
    {
        public string Challenge { get; init; }
        public ulong NumberOfIterations { get; init; }
        public ClassGroupElement VdfOutput { get; init; }
    }

    public record ChallengeChainSubSlot
    {
        public VDFInfo ChallengeChainEndOfSlotVdf { get; init; }
        public string InfusedChallengeChainSubSlotHash { get; init; }
        public string SubepochSummaryHash { get; init; }
        public ulong? NewSubSlotIters { get; init; }
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

    public record EndOfSlotBundle
    {
        public ChallengeChainSubSlot ChallengeChain { get; init; }
        public InfusedChallengeChainSubSlot InfusedChallengeChain { get; init; }
        public RewardChainSubSlot RewardChain { get; init; }
        public SubSlotProofs Proofs { get; init; }
    }

    public record ProofOfSpace
    {
        public string Challenge { get; init; }
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
        public VDFInfo ChallengeChainSpVdf { get; init; }
        public string ChallengeChainSpSignature { get; init; }
        public VDFInfo ChallengeChainIpVdf { get; init; }
        public VDFInfo RewardChainSpVdf { get; init; }
        public string RewardChainSpSignature { get; init; }
        public VDFInfo RewardChainIpVdf { get; init; }
        public VDFInfo InfusedChallengeChainIpVdf { get; init; }
        public bool IsTransactionBlock { get; init; }
    }

    public record PoolTarget
    {
        public string PuzzleHash { get; init; }
        public uint MaxHeight { get; init; }
    }

    public record FoliageBlockData
    {
        public string UnfinishedRewardBlockHash { get; init; }
        public PoolTarget PoolTarget { get; init; }
        public string PoolSignature { get; init; }
        public string FarmerRewardPuzzleHash { get; init; }
        public string ExtensionData { get; init; }
    }

    public record Foliage
    {
        public string PrevBlockHash { get; init; }
        public string RewardBlockHash { get; init; }
        public FoliageBlockData FoliageBlockData { get; init; }
        public string FoliageBlockDataSignature { get; init; }
        public string FoliageTransactionBlockHash { get; init; }
        public string FoliageTransactionBlockSignature { get; init; }
    }

    public record FoliageTransactionBlock
    {
        public string PrevTransactionBlockHash { get; init; }
        public ulong Timestamp { get; init; }
        public string FilterHash { get; init; }
        public string AdditionsRoot { get; init; }
        public string RemovalsRoot { get; init; }
        public string TransactionsInfoHash { get; init; }
    }

    public record TransactionsInfo
    {
        public string GeneratorRoot { get; init; }
        public string GeneratorRefsRoot { get; init; }
        public string AggregatedSignature { get; init; }
        public ulong Fees { get; init; }
        public ulong Cost { get; init; }
        public ICollection<Coin> RewardClaimsIncorporated { get; init; }
    }

    public record FullBlock
    {
        public ICollection<EndOfSlotBundle> FinishedSubSlots { get; init; }
        public RewardChainBlock RewardChainBlock { get; init; }
        public VDFProof ChallengeChainSpProof { get; init; }
        public VDFProof ChallengeChainIpProof { get; init; }
        public VDFProof RewardChainSpProof { get; init; }
        public VDFProof RewardChainIpProof { get; init; }
        public VDFProof InfusedChallengeChainIpProof { get; init; }
        public Foliage Foliage { get; init; }
        public FoliageTransactionBlock FoliageTransactionBlock { get; init; }
        public TransactionsInfo TransactionsInfo { get; init; }
        public string TransactionsGenerator { get; init; }
        public ICollection<uint> TransactionsGeneratorRefList { get; init; }
    }
}
