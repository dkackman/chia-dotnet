using System.Collections.Generic;

namespace chia.dotnet
{
    public record DAOInfo
    {
        public string TreasuryId { get; init; } = string.Empty;
        public uint CatWalletId { get; init; }
        public uint DaoCatWalletId { get; init; }
        public IEnumerable<ProposalInfo> ProposalsList { get; init; } = new List<ProposalInfo>();
        public IEnumerable<IDictionary<string, LineageProof?>> ParentInfo { get; init; } = new List<IDictionary<string, LineageProof?>>();
        public Coin? CurrentTreasuryCoin { get; init; }
        public string? CurrentTreasuryInnerpuz { get; init; }
        /// <summary>
        /// the block height that the current treasury singleton was created in
        /// </summary>
        public uint SingletonBlockHeight { get; init; }
        /// <summary>
        /// we ignore proposals with fewer votes than this - defaults to 1
        /// </summary>
        public ulong FilterBelowVoteAmount { get; init; } = 1;
        public IEnumerable<string> Assets { get; init; } = new List<string>();
        public uint CurrentHeight { get; init; }
    }
}
