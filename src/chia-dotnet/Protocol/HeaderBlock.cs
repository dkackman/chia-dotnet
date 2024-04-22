using System.Collections.Generic;

namespace chia.dotnet.protocol;

public record HeaderBlock
{
    public List<EndOfSubSlotBundle> FinishedSubSlots { get; init; } = [];
    public RewardChainBlock RewardChainBlock { get; init; } = new();
    public VDFProof? ChallengeChainSpProof { get; init; }
    public VDFProof ChallengeChainIpProof { get; init; } = new();
    public VDFProof? RewardChainSpProof { get; init; }
    public VDFProof RewardChainIpProof { get; init; } = new();
    public VDFProof? InfusedChallengeChainIpProof { get; init; }
    public Foliage Foliage { get; init; } = new();
    public FoliageTransactionBlock? FoliageTransactionBlock { get; init; }
    public byte[] TransactionsFilter { get; init; } = [];
    public TransactionsInfo? TransactionsInfo { get; init; }
}
