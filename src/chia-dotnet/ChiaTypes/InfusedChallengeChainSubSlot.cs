namespace chia.dotnet
{
    public record InfusedChallengeChainSubSlot
    {
        public VDFInfo InfusedChallengeChainEndOfSlotVdf { get; init; } = new();
    }
}
