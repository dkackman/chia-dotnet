using System;
using System.Numerics;

using Newtonsoft.Json;

namespace chia.dotnet
{
    public record BlockchainState
    {
        public ulong Difficulty { get; init; }
        public bool GenesisChallengeInitiated { get; init; }
        public int MempoolSize { get; init; }
        public BlockRecord Peak { get; init; }
        public BigInteger Space { get; init; }
        public ulong SubSlotIters { get; init; }
        public SyncState Sync { get; init; }
    }

    public record SyncState
    {
        public bool SyncMode { get; init; }
        public ulong SyncProgressHeight { get; init; }
        public ulong SyncTipHeight { get; init; }
        public bool Synced { get; init; }
    }
}
