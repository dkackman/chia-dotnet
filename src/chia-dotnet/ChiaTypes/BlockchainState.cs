using System;
using System.Numerics;

namespace chia.dotnet
{
    /// <summary>
    /// The node's view of the blockchain.
    /// </summary>
    public record BlockchainState
    {
        public string NodeId { get; init; } = string.Empty;
        public ulong Difficulty { get; init; }
        public bool GenesisChallengeInitiated { get; init; }
        public long MempoolSize { get; init; }
        public long MempoolCost { get; init; }
        public ulong MempoolFees { get; init; }
        public MempoolMinFees MempoolMinFees { get; init; } = new();
        public long MempoolMaxTotalCost { get; init; }
        public long BlockMaxCost { get; init; }
        public BlockRecord? Peak { get; init; }
        public BigInteger Space { get; init; }
        public ulong SubSlotIters { get; init; }
        public SyncState Sync { get; init; } = new();
        public uint AverageBlockTime { get; init; }
    }

    public record MempoolMinFees
    {
        public double Cost5000000 { get; init; }
    }
}
