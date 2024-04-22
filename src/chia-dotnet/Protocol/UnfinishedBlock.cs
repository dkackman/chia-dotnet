using System.Collections.Generic;

namespace chia.dotnet.protocol;

public record UnfinishedBlock
{
    public List<EndOfSubSlotBundle> FinishedSubSlots { get; init; } = [];
    public RewardChainBlockUnfinished RewardChainBlock { get; init; } = new();
    public VDFProof? ChallengeChainSpProof { get; init; }
    public VDFProof? RewardChainSpProof { get; init; }
    public Foliage Foliage { get; init; } = new();
    public FoliageTransactionBlock? FoliageTransactionBlock { get; init; }
    public TransactionsInfo? TransactionsInfo { get; init; }
    public byte[] TransactionsGenerator { get; init; } = [];
    public List<uint> TransactionsGeneratorRefList { get; init; } = [];
}
