using System;
using System.Globalization;

using Newtonsoft.Json;

namespace chia.dotnet
{
    internal sealed class PoolPointConverter : JsonConverter<PoolPoint>
    {
        public override PoolPoint ReadJson(JsonReader reader, Type objectType, PoolPoint existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            // these things are stored as an array of two numbers [double, int] - pivot those into an object
            var found = reader.ReadAsDouble() ?? 0;
            var difficulty = Convert.ToUInt64(reader.ReadAsString(), CultureInfo.InvariantCulture);
            _ = reader.Read();

            return new PoolPoint()
            {
                TimeFound = found,
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
}
