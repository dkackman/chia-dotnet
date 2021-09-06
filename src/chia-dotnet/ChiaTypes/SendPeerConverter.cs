using System;
using System.Globalization;

using Newtonsoft.Json;

namespace chia.dotnet
{
    internal sealed class SendPeerConverter : JsonConverter<SendPeer>
    {
        public override SendPeer? ReadJson(JsonReader reader, Type objectType, SendPeer? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            // these things are stored as an array of two numbers [string, byte, string] - pivot those into an object
            var peer = reader.ReadAsString();
            var includedInMempool = Convert.ToByte(reader.ReadAsString(), CultureInfo.InvariantCulture);
            var error = reader.ReadAsString();
            _ = reader.Read();

            return new SendPeer()
            {
                Peer = peer ?? string.Empty,
                MempoolInclusionStatus = (MempoolInclusionStatus)includedInMempool,
                ErrorMessage = error ?? string.Empty
            };
        }

        public override void WriteJson(JsonWriter writer, SendPeer? value, JsonSerializer serializer)
        {
            writer.WriteStartArray();
            if (value is not null)
            {
                writer.WriteValue(value.Peer);
                writer.WriteValue((byte)value.MempoolInclusionStatus);
                writer.WriteValue(value.ErrorMessage);
            }
            writer.WriteEndArray();
        }
    }
}
