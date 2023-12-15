namespace chia.dotnet
{
    public record ProposalInfo
    {
        /// <summary>
        /// this is launcher_id
        /// </summary>
        public string ProposalId { get; init; } = string.Empty;
        public string InnerPuzzle { get; init; } = string.Empty;
        public ulong AmountVoted { get; init; }
        public ulong YesVotes { get; init; }
        public Coin CurrentCoin { get; init; } = new();
        public string? CurrentInnerpuz { get; init; }
        /// <summary>
        /// if this is null then the proposal has finished
        /// </summary>
        public Coin? TimeCoin { get; init; }
        /// <summary>
        /// Block height that current proposal singleton coin was created in
        /// </summary>
        public uint SingletonBlockHeight { get; init; }
        public bool? Passed { get; init; }
        public bool? Closed { get; init; }
    }
}
