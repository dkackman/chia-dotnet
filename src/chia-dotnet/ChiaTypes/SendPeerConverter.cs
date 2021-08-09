using System;
using System.Globalization;

using Newtonsoft.Json;

namespace chia.dotnet
{
    internal sealed class SendPeerConverter : JsonConverter<SendPeer>
    {
        public override SendPeer ReadJson(JsonReader reader, Type objectType, SendPeer existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            // these things are stored as an array of two numbers [string, byte, string] - pivot those into an object
            var peer = reader.ReadAsString();
            var includedInMempool = Convert.ToByte(reader.ReadAsString(), CultureInfo.InvariantCulture);
            var error = reader.ReadAsString();
            _ = reader.Read();

            return new SendPeer()
            {
                Peer = peer,
                IncludedInMempool = includedInMempool,
                ErrorMessage = error
            };
        }

        public override void WriteJson(JsonWriter writer, SendPeer value, JsonSerializer serializer)
        {
            writer.WriteStartArray();
            writer.WriteValue(value.Peer);
            writer.WriteValue(value.IncludedInMempool);
            writer.WriteValue(value.ErrorMessage);
            writer.WriteEndArray();
        }
    }
}
