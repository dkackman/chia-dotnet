using System;
using System.Collections.Generic;

using Newtonsoft.Json;

namespace chia.dotnet
{
    internal sealed class ConditionConverter : JsonConverter<Condition>
    {
        public override Condition ReadJson(JsonReader reader, Type objectType, Condition existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
            {
                return null;
            }

            var opcode = reader.ReadAsString(); // the opcode is stored without a name (as part of an unnamed tuple (aka array in json))
            _ = reader.Read(); // move ahead to the start of the collection
            var args = serializer.Deserialize<ICollection<ConditionWithArgs>>(reader);
            _ = reader.Read();

            return new Condition()
            {
                ConditionOpcode = opcode,
                Args = args
            };
        }

        public override void WriteJson(JsonWriter writer, Condition value, JsonSerializer serializer)
        {
            writer.WriteStartArray();
            writer.WriteValue(value.ConditionOpcode);
            serializer.Serialize(writer, value.Args);
            writer.WriteEndArray();
        }
    }
}
