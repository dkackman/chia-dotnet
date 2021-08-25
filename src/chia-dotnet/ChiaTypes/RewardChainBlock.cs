using System.Numerics;

namespace chia.dotnet
{
    public record RewardChainBlock
    {
        public BigInteger Weight { get; init; }
        public uint Height { get; init; }
        public BigInteger TotalIters { get; init; }
        public byte SignagePointIndex { get; init; }
        public string PosSsCcChallengeHash { get; init; } = string.Empty;
        public ProofOfSpace ProofOfSpace { get; init; } = new();
        /// <summary>
        /// Not present for first sp in slot
        /// </summary>
        public VDFInfo? ChallengeChainSpVdf { get; init; }
        public string ChallengeChainSpSignature { get; init; } = string.Empty;
        public VDFInfo ChallengeChainIpVdf { get; init; } = new();
        /// <summary>
        /// Not present for first sp in slot
        /// </summary>
        public VDFInfo? RewardChainSpVdf { get; init; }
        public string RewardChainSpSignature { get; init; } = string.Empty;
        public VDFInfo RewardChainIpVdf { get; init; } = new();
        /// <summary>
        /// Iff deficit &lt; 16
        /// </summary>
        public VDFInfo? InfusedChallengeChainIpVdf { get; init; }
        public bool IsTransactionBlock { get; init; }
    }
}
