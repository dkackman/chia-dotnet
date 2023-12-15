namespace chia.dotnet
{
    public record ProposalState
    {
        public int TotalVotesNeeded { get; init; }
        public int YesVotesNeeded { get; init; }
        public int BlocksNeeded { get; init; }
        public bool Passed { get; init; }
        public bool Closeable { get; init; }
        public bool? Closed { get; init; }
    }
}
