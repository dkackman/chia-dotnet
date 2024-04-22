using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace chia.dotnet.protocol;

public record NewPeak
{
    public byte[] HeaderHash { get; init; } = [];
    public uint Height { get; init; }
    public decimal Weight { get; init; }
    public uint ForkPointWithPreviousPeak { get; init; }
    public byte[] UnfinishedRewardBlockHash { get; init; } = [];
}

public record NewTransaction
{
    public byte[] TransactionId { get; init; } = [];
    public ulong Cost { get; init; }
    public ulong Fees { get; init; }
}

public record RequestTransaction
{
    public byte[] TransactionId { get; init; } = [];
}

public record RespondTransaction
{
    public SpendBundle Transaction { get; init; } = new();
}

public record RequestProofOfWeight
{
    public uint TotalNumberOfBlocks { get; init; }
    public byte[] Tip { get; init; } = [];
}

public record RespondProofOfWeight
{
    public WeightProof Wp { get; init; } = new();
    public byte[] Tip { get; init; } = [];
}

public record RequestBlock
{
    public uint Height { get; init; }
    public bool IncludeTransactionBlock { get; init; }
}

public record RejectBlock
{
    public uint Height { get; init; }
}

public record RequestBlocks
{
    public uint StartHeight { get; init; }
    public uint EndHeight { get; init; }
    public bool IncludeTransactionBlock { get; init; }
}

public record RespondBlocks
{
    public uint StartHeight { get; init; }
    public uint EndHeight { get; init; }
    public List<FullBlock> Blocks { get; init; } = [];
}

public record RejectBlocks
{
    public uint StartHeight { get; init; }
    public uint EndHeight { get; init; }
}

public record RespondBlock
{
    public FullBlock Block { get; init; } = new();
}

public record NewUnfinishedBlock
{
    public byte[] UnfinishedRewardHash { get; init; } = [];
}

public record RequestUnfinishedBlock
{
    public byte[] UnfinishedRewardHash { get; init; } = [];
}

public record RespondUnfinishedBlock
{
    public UnfinishedBlock UnfinishedBlock { get; init; } = new();
}

public record NewSignagePointOrEndOfSubSlot
{
    public byte[]? PrevChallengeHash { get; init; }
    public byte[] ChallengeHash { get; init; } = [];
    public byte IndexFromChallenge { get; init; }
    public byte[] LastRcInfusion { get; init; } = [];
}

public record RequestSignagePointOrEndOfSubSlot
{
    public byte[] ChallengeHash { get; init; } = [];
    public byte IndexFromChallenge { get; init; }
    public byte[] LastRcInfusion { get; init; } = [];
}

public record RespondSignagePoint
{
    public byte IndexFromChallenge { get; init; }
    public VDFInfo ChallengeChainVdf { get; init; } = new();
    public VDFProof ChallengeChainProof { get; init; } = new();
    public VDFInfo RewardChainVdf { get; init; } = new();
    public VDFProof RewardChainProof { get; init; } = new();
}

public record RespondEndOfSubSlot
{
    public EndOfSubSlotBundle EndOfSlotBundle { get; init; } = new();
}

public record RequestMempoolTransactions
{
    public byte[] Filter { get; init; } = [];
}

public record NewCompactVDF
{
    public uint Height { get; init; }
    public byte[] HeaderHash { get; init; } = [];
    public byte FieldVdf { get; init; }
    public VDFInfo VdfInfo { get; init; } = new();
}

public record RequestCompactVDF
{
    public uint Height { get; init; }
    public byte[] HeaderHash { get; init; } = [];
    public byte FieldVdf { get; init; }
    public VDFInfo VdfInfo { get; init; } = new();
}

public record RespondCompactVDF
{
    public uint Height { get; init; }
    public byte[] HeaderHash { get; init; } = [];
    public byte FieldVdf { get; init; }
    public VDFInfo VdfInfo { get; init; } = new();
    public VDFProof VdfProof { get; init; } = new();
}

public record RequestPeers
{
}

public record RespondPeers
{
    public List<TimestampedPeerInfo> PeerList { get; init; } = [];
}

public record TimestampedPeerInfo
{
    public string Host { get; init; } = string.Empty;
    public ushort Port { get; init; }
    public ulong Timestamp { get; init; }

    [JsonIgnore]
    public DateTime DateTimestamp => Timestamp.ToDateTime();
}
