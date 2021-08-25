using System;

using Newtonsoft.Json;

namespace chia.dotnet
{
    [JsonConverter(typeof(PoolPointConverter))]
    public record PoolPoint
    {
        public double TimeFound { get; init; }
        public ulong Difficulty { get; init; }
        [JsonIgnore]
        public DateTime DateTimeFound => TimeFound.ToDateTime();
    }
}
