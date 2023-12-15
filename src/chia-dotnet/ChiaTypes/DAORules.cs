namespace chia.dotnet
{
    public record DAORules
    {
        public ulong ProposalTimelock { get; init; }
        public ulong SoftCloseLength { get; init; }
        public ulong AttendanceRequired { get; init; }
        public ulong PassPercentage { get; init; }
        public ulong SelfDestructLength { get; init; }
        public ulong OracleSpendDelay { get; init; }
        public ulong ProposalMinimumAmount { get; init; }
    }
}
