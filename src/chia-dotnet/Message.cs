using System;
using System.Dynamic;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace chia.dotnet
{
    /// <summary>
    /// The messaging data structure for request and response exchange with the RPC endpoint
    /// </summary>
    public record Message
    {
        /// <summary>
        /// The command to be processed by the endpoint service
        /// </summary>
        public string Command { get; init; }

        /// <summary>
        /// Data to go along with the command
        /// </summary>
        public dynamic Data { get; init; }

        /// <summary>
        /// The name of the origin service
        /// </summary>
        public string Origin { get; init; }

        /// <summary>
        /// The name of the destination service
        /// </summary>
        public string Destination { get; init; }

        /// <summary>
        /// Indication whether message is an acknowledgement (i.e response)
        /// </summary>
        public bool Ack { get; init; }

        /// <summary>
        /// Unique id to correlate requests to responses
        /// </summary>
        public string Request_Id { get; init; }

        /// <summary>
        /// Construct a new instance of a <see cref="Message"/>
        /// </summary>
        /// <param name="command"><see cref="Command"/></param>
        /// <param name="data"><see cref="Data"/></param>
        /// <param name="destination"><see cref="Destination"/></param>
        /// <param name="origin"><see cref="Origin"/></param>
        /// <returns></returns>
        public static Message Create(string command, object data, string destination, string origin)
        {
            return new Message
            {
                Command = command,
                Data = data ?? new ExpandoObject(),
                Origin = origin,
                Destination = destination,
                Request_Id = GetNewReuqestId()
            };
        }

        /// <summary>
        /// Serialize the <see cref="Message"/> to a json string
        /// </summary>
        /// <returns>Json representation of the message</returns>
        public string ToJson()
        {
            var serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new LowercaseContractResolver()
            };
            return JsonConvert.SerializeObject(this, serializerSettings);
        }

        private class LowercaseContractResolver : DefaultContractResolver
        {
            protected override string ResolvePropertyName(string propertyName)
            {
                return propertyName.ToLower();
            }
        }

        /// <summary>
        /// Deserialize a <see cref="Message"/> from a json string
        /// </summary>
        /// <param name="json">Json representation of the Message</param>
        /// <returns><see cref="Message"/></returns>
        public static Message FromJson(string json)
        {
            var message = JsonConvert.DeserializeObject<Message>(json);
            return message;
        }

        private static readonly Random random = new();

        private static string GetNewReuqestId()
        {
            var buffer = new byte[32];
            random.NextBytes(buffer);
            return BitConverter.ToString(buffer).Replace("-", "");
        }
    }
}
