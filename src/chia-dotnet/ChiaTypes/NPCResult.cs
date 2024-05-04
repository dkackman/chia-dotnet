namespace chia.dotnet
{
    public record NPCResult
    {
        public ushort? Error { get; init; }
        public SpendBundleConditions Conds { get; init; } = new();
    }
}
