using System;
using System.Dynamic;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;

namespace chia.dotnet
{
    public record Message
    {
        public string Command { get; init; }
        public dynamic Data { get; init; }
        public string Origin { get; init; }
        public string Destination { get; init; }
        public bool Ack { get; init; }
        public string Request_Id { get; init; }

        public static Message Create(string command, object data, string destination, string origin)
        {
            return new Message
            {
                Command = command,
                Data = data,
                Origin = origin,
                Destination = destination,
                Request_Id = GetNewReuqestId()
            };
        }

        public string ToJson()
        {
            var serializerSettings = new JsonSerializerSettings();
            serializerSettings.ContractResolver = new LowercaseContractResolver();
            return JsonConvert.SerializeObject(this, serializerSettings);
        }

        private class LowercaseContractResolver : DefaultContractResolver
        {
            protected override string ResolvePropertyName(string propertyName) => propertyName.ToLower();
        }

        public static Message FromJson(string json)
        {
            var message = JsonConvert.DeserializeObject<Message>(json);
            return message;
        }

        private static Random random = new();

        private static string GetNewReuqestId()
        {
            byte[] buffer = new byte[32];
            random.NextBytes(buffer);
            return BitConverter.ToString(buffer).Replace("-", "");
        }
    }
}
