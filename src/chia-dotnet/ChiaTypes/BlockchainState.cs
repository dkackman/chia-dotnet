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
        public Int64 MempoolSize { get; init; }
        public Int64 MempoolCost { get; init; }
        public ulong MempoolFees { get; init; }
        public MempoolMinFees MempoolMinFees { get; init; } = new();
        public Int64 MempoolMaxTotalCost { get; init; }
        public Int64 BlockMaxCost { get; init; }
        public BlockRecord? Peak { get; init; }
        public BigInteger Space { get; init; }
        public ulong SubSlotIters { get; init; }
        public SyncState Sync { get; init; } = new();
    }

    public record MempoolMinFees
    {
        public double Cost5000000 { get; init; }
    }
}
