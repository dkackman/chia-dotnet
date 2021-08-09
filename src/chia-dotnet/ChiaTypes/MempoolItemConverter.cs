using System;
using System.Globalization;

using Newtonsoft.Json;

namespace chia.dotnet
{
    internal sealed class MempoolItemConverter : JsonConverter<MempoolItem>
    {
        public override MempoolItem ReadJson(JsonReader reader, Type objectType, MempoolItem existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            // these things are stored as an array of two numbers [double, int] - pivot those into an object
            //var id = reader.ReadAsString();
            //var difficulty = Convert.ToUInt64(reader.ReadAsString(), CultureInfo.InvariantCulture);
            _ = reader.Read();

            return new MempoolItem()
            {
                // Id = new Opaque()
            };
        }

        public override void WriteJson(JsonWriter writer, MempoolItem value, JsonSerializer serializer)
        {
            writer.WriteStartArray();
            // writer.WriteValue(value.Id);
            //writer.WriteValue(value.Difficulty);
            writer.WriteEndArray();
        }
    }
}
