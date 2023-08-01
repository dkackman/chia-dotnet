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
        public int MempoolSize { get; init; }
        public int MempoolCost { get; init; }
        public ulong MempoolFees { get; init; }
        public MempoolMinFees MempoolMinFees { get; init; } = new();
        public int MempoolMaxTotalCost { get; init; }
        public int BlockMaxCost { get; init; }
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
