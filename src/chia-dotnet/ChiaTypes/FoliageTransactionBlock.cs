using System;

using Newtonsoft.Json;

namespace chia.dotnet
{
    /// <summary>
    /// Information that goes along with each transaction block that is relevant for light clients
    /// </summary>
    public record FoliageTransactionBlock
    {
        public string PrevTransactionBlockHash { get; init; } = string.Empty;
        public ulong Timestamp { get; init; }
        public string FilterHash { get; init; } = string.Empty;
        public string AdditionsRoot { get; init; } = string.Empty;
        public string RemovalsRoot { get; init; } = string.Empty;
        public string TransactionsInfoHash { get; init; } = string.Empty;
        [JsonIgnore]
        public DateTime DateTimestamp => Timestamp.ToDateTime();
    }
}
