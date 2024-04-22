using System.Collections.Generic;

namespace chia.dotnet.protocol;

public record SubEpochData
{
    public byte[] RewardChainHash { get; init; } = [];
    public byte NumBlocksOverflow { get; init; }
    public ulong? NewSubSlotIters { get; init; }
    public ulong? NewDifficulty { get; init; }
}

public record SubSlotData
{
    public ProofOfSpace? ProofOfSpace { get; init; }
    public VDFProof? CcSignagePoint { get; init; }
    public VDFProof? CcInfusionPoint { get; init; }
    public VDFProof? IccInfusionPoint { get; init; }
    public VDFInfo? CcSpVdfInfo { get; init; }
    public byte? SignagePointIndex { get; init; }
    public VDFProof? CcSlotEnd { get; init; }
    public VDFProof? IccSlotEnd { get; init; }
    public VDFInfo? CcSlotEndInfo { get; init; }
    public VDFInfo? IccSlotEndInfo { get; init; }
    public VDFInfo? CcIpVdfInfo { get; init; }
    public VDFInfo? IccIpVdfInfo { get; init; }
    public decimal? TotalIters { get; init; }

    public bool IsEndOfSlot() => CcSlotEndInfo != null;
    public bool IsChallenge() => ProofOfSpace != null;
}

public record SubEpochChallengeSegment
{
    public uint SubEpochN { get; init; }
    public List<SubSlotData> SubSlots { get; init; } = [];
    public VDFInfo? RcSlotEndInfo { get; init; }
}

public record SubEpochSegments
{
    public List<SubEpochChallengeSegment> ChallengeSegments { get; init; } = [];
}

public record RecentChainDataRecord
{
    public List<HeaderBlock> RecentChainData { get; init; } = [];
}

public record ProofBlockHeader
{
    public List<EndOfSubSlotBundle> FinishedSubSlots { get; init; } = [];
    public RewardChainBlock RewardChainBlock { get; init; } = new();
}

public record WeightProof
{
    public List<SubEpochData> SubEpochs { get; init; } = [];
    public List<SubEpochChallengeSegment> SubEpochSegments { get; init; } = [];
    public List<HeaderBlock> RecentChainData { get; init; } = [];
}
