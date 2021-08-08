using System;
using System.Collections.Generic;
using System.Globalization;

using Newtonsoft.Json;

namespace chia.dotnet
{
    public record PoolConfig
    {
        public string AuthenticationPublicKey { get; init; }
        public string LauncherId { get; init; }
        public string OwnerPublicKey { get; init; }
        public string P2SingletonPuzzleHash { get; init; }
        public string PayoutInstructions { get; init; }
        public string PoolUrl { get; init; }
        public string TargetPuzzleHash { get; init; }
    }

    internal sealed class PoolPointConverter : JsonConverter<PoolPoint>
    {
        public override PoolPoint ReadJson(JsonReader reader, Type objectType, PoolPoint existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            // these things are stored as an array of two numbers [double, int]
            // pivot those into an object
            var found = reader.ReadAsDouble();
            var difficulty = Convert.ToUInt64(reader.ReadAsString(), CultureInfo.InvariantCulture);
            _ = reader.Read();
            return new PoolPoint()
            {
                TimeFound = found ?? 0,
                Difficulty = difficulty
            };
        }

        public override void WriteJson(JsonWriter writer, PoolPoint value, JsonSerializer serializer)
        {
            writer.WriteStartArray();
            writer.WriteValue(value.TimeFound);
            writer.WriteValue(value.Difficulty);
            writer.WriteEndArray();
        }
    }

    [JsonConverter(typeof(PoolPointConverter))]
    public record PoolPoint
    {
        public double TimeFound { get; init; }
        public ulong Difficulty { get; init; }

        public DateTime DateTimeFound => TimeFound.ToDateTime();
    }

    public record PoolState
    {
        public int AuthenticationTokenTimeout { get; init; }
        public ulong CurrentDifficulty { get; init; }
        public ulong CurrentPoints { get; init; }
        public double NextFarmerUpdate { get; init; }
        public double NextPoolInfoUpdate { get; init; }
        public string P2SingletonPuzzleHash { get; init; }

        [JsonProperty("points_acknowledged_24h")]
        public IEnumerable<PoolPoint> PointsAcknowledged24h { get; init; }
        public ulong PointsAcknowledgedSinceStart { get; init; }

        [JsonProperty("points_found_24h")]
        public IEnumerable<PoolPoint> PointsFound24h { get; init; }
        public ulong PointsFoundSinceStart { get; init; }
        public PoolConfig PoolConfig { get; init; }
        public IEnumerable<dynamic> PoolErrors24h { get; init; }

        public DateTime NextFarmerUpdateDateTime => NextFarmerUpdate.ToDateTime();
        public DateTime NextPoolInfoUpdateDateTime => NextFarmerUpdate.ToDateTime();
    }
}
