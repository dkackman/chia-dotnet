using System.Numerics;

namespace chia.dotnet
{
    public record RewardChainBlockUnfinished
    {
        public BigInteger TotalIters { get; init; }
        public byte SignagePointIndex { get; init; }
        public string PosSsCcChallengeHash { get; init; } = string.Empty;
        public ProofOfSpace ProofOfSpace { get; init; } = new();
        /// <summary>
        /// Not present for first sp in slot
        /// </summary>
        public VDFInfo? ChallengeChainSpVdf { get; init; }
        public string ChallengeChainSpSignature { get; init; } = string.Empty;
        /// <summary>
        /// Not present for first sp in slot
        /// </summary>
        public VDFInfo? RewardChainSpVdf { get; init; }
        public string RewardChainSpSignature { get; init; } = string.Empty;
    }
}
