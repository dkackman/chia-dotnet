using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace chia.dotnet
{
    public enum WalletType
    {
        STANDARD_WALLET = 0,
        RATE_LIMITED = 1,
        ATOMIC_SWAP = 2,
        AUTHORIZED_PAYEE = 3,
        MULTI_SIG = 4,
        CUSTODY = 5,
        COLOURED_COIN = 6,
        RECOVERABLE = 7
    }

    public record Coin
    {
        [JsonProperty("confirmed_block_index")]
        public uint ConfirmedBlockIndex { get; init; }

        [JsonProperty("spent_block_index")]
        public uint SpentBlockIndex { get; init; }

        [JsonProperty("spent")]
        public bool Spent { get; init; }

        [JsonProperty("coinbase")]
        public bool Coinbase { get; init; }

        [JsonProperty("wallet_type")]
        public WalletType WalletType { get; init; }

        [JsonProperty("wallet_id")]
        public uint WalletId { get; init; }
    }

    public record CoinSolution
    {

        [JsonProperty("coin")]
        public Coin Coin { get; init; }


        [JsonProperty("solution")]
        public dynamic Solution { get; init; }
    }

    public record SpendBundle
    {

        [JsonProperty("coin_solutions")]
        public CoinSolution[] CoinSolutions { get; init; }

        [JsonProperty("aggregated_signature")]
        public string AggregatedSignature { get; init; }
    }
}
