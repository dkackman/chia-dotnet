namespace chia.dotnet
{
    public record RewardChainSubSlot
    {
        public VDFInfo EndOfSlotVdf { get; init; } = new();
        public string ChallengeChainSubSlotHash { get; init; } = string.Empty;
        public string? InfusedChallengeChainSubSlotHash { get; init; }
        /// <summary>
        /// 16 or less. usually zero
        /// </summary>
        public byte Deficit { get; init; }
    }
}
