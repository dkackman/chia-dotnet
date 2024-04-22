using System.Collections.Generic;

namespace chia.dotnet.protocol;

public record RequestPuzzleSolution
{
    public byte[] CoinName { get; init; } = new byte[32];
    public uint Height { get; init; }
}

public record PuzzleSolutionResponse
{
    public byte[] CoinName { get; init; } = new byte[32];
    public uint Height { get; init; }
    public byte[] Puzzle { get; init; } = [];
    public byte[] Solution { get; init; } = [];
}

public record RespondPuzzleSolution
{
    public PuzzleSolutionResponse Response { get; init; } = new();
}

public record RejectPuzzleSolution
{
    public byte[] CoinName { get; init; } = new byte[32];
    public uint Height { get; init; }
}

public record SendTransaction
{
    public SpendBundle Transaction { get; init; } = new();
}

public record TransactionAck
{
    public byte[] Txid { get; init; } = new byte[32];
    public byte Status { get; init; } // MempoolInclusionStatus
    public string? Error { get; init; }
}

public record NewPeakWallet
{
    public byte[] HeaderHash { get; init; } = new byte[32];
    public uint Height { get; init; }
    public ulong Weight { get; init; }
    public uint ForkPointWithPreviousPeak { get; init; }
}

public record RequestBlockHeader
{
    public uint Height { get; init; }
}

public record RespondBlockHeader
{
    public HeaderBlock HeaderBlock { get; init; } = new();
}

public record RejectHeaderRequest
{
    public uint Height { get; init; }
}

public record RequestRemovals
{
    public uint Height { get; init; }
    public byte[] HeaderHash { get; init; } = new byte[32];
    public List<byte[]>? CoinNames { get; init; }
}

public record RespondRemovals
{
    public uint Height { get; init; }
    public byte[] HeaderHash { get; init; } = new byte[32];
    public List<(byte[], Coin?)> Coins { get; init; } = [];
    public List<(byte[], byte[])>? Proofs { get; init; }
}

public record RejectRemovalsRequest
{
    public uint Height { get; init; }
    public byte[] HeaderHash { get; init; } = new byte[32];
}

public record RequestAdditions
{
    public uint Height { get; init; }
    public byte[]? HeaderHash { get; init; }
    public List<byte[]>? PuzzleHashes { get; init; }
}

public record RespondAdditions
{
    public uint Height { get; init; }
    public byte[] HeaderHash { get; init; } = new byte[32];
    public List<(byte[], List<Coin>)> Coins { get; init; } = [];
    public List<(byte[], byte[], byte[])>? Proofs { get; init; }
}

public record RejectAdditionsRequest
{
    public uint Height { get; init; }
    public byte[] HeaderHash { get; init; } = new byte[32];
}

public record RespondBlockHeaders
{
    public uint StartHeight { get; init; }
    public uint EndHeight { get; init; }
    public List<HeaderBlock> HeaderBlocks { get; init; } = [];
}

public record RejectBlockHeaders
{
    public uint StartHeight { get; init; }
    public uint EndHeight { get; init; }
}

public record RequestBlockHeaders
{
    public uint StartHeight { get; init; }
    public uint EndHeight { get; init; }
    public bool ReturnFilter { get; init; }
}

public record RequestHeaderBlocks
{
    public uint StartHeight { get; init; }
    public uint EndHeight { get; init; }
}

public record RejectHeaderBlocks
{
    public uint StartHeight { get; init; }
    public uint EndHeight { get; init; }
}

public record RespondHeaderBlocks
{
    public uint StartHeight { get; init; }
    public uint EndHeight { get; init; }
    public List<HeaderBlock> HeaderBlocks { get; init; } = [];
}

public record RegisterForPhUpdates
{
    public List<byte[]> PuzzleHashes { get; init; } = [];
    public uint MinHeight { get; init; }
}

public record RespondToPhUpdates
{
    public List<byte[]> PuzzleHashes { get; init; } = [];
    public uint MinHeight { get; init; }
    public List<CoinState> CoinStates { get; init; } = [];
}

public record RegisterForCoinUpdates
{
    public List<byte[]> CoinIds { get; init; } = [];
    public uint MinHeight { get; init; }
}

public record RespondToCoinUpdates
{
    public List<byte[]> CoinIds { get; init; } = [];
    public uint MinHeight { get; init; }
    public List<CoinState> CoinStates { get; init; } = [];
}

public record CoinStateUpdate
{
    public uint Height { get; init; }
    public uint ForkHeight { get; init; }
    public byte[] PeakHash { get; init; } = new byte[32];
    public List<CoinState> Items { get; init; } = [];
}

public record RequestChildren
{
    public byte[] CoinName { get; init; } = new byte[32];
}

public record RespondChildren
{
    public List<CoinState> CoinStates { get; init; } = [];
}

public record RequestSesInfo
{
    public uint StartHeight { get; init; }
    public uint EndHeight { get; init; }
}

public record RespondSesInfo
{
    public List<byte[]> RewardChainHash { get; init; } = [];
    public List<List<uint>> Heights { get; init; } = [];
}

public record RequestFeeEstimates
{
    public List<ulong> TimeTargets { get; init; } = [];
}

public record RespondFeeEstimates
{
    public FeeEstimateGroup Estimates { get; init; } = new();
}
