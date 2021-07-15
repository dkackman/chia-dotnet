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
        /// Unique correlation id of the message. This will round trip to the RPC server and back in its response
        /// </summary>
        public string RequestId { get; init; }

        /// <summary>
        /// Inidcates whether this is a response (<see cref="Ack"/> is true) and the success flag is also true
        /// </summary>
        [JsonIgnore]
        public bool IsSuccessfulResponse => Ack && Data?.success == true;

        /// <summary>
        /// Construct a new instance of a <see cref="Message"/>
        /// </summary>
        /// <param name="command"><see cref="Command"/></param>
        /// <param name="data"><see cref="Data"/></param>
        /// <param name="destination"><see cref="Destination"/></param>
        /// <param name="origin"><see cref="Origin"/></param>
        /// <returns>A populated <see cref="Message"/></returns>
        /// <remarks>Ensure that <see cref="Data"/> and <see cref="RequestId"/> are set appropriately</remarks>
        public static Message Create(string command, object data, string destination, string origin)
        {
            return string.IsNullOrEmpty(command)
                ? throw new ArgumentNullException(nameof(command))
                : string.IsNullOrEmpty(destination)
                ? throw new ArgumentNullException(nameof(destination))
                : string.IsNullOrEmpty(origin)
                ? throw new ArgumentNullException(nameof(origin))
                : new Message
                {
                    Command = command,
                    Data = data ?? new ExpandoObject(),
                    Origin = origin,
                    Destination = destination,
                    RequestId = GetNewReuqestId()
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
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                }
            };
            return JsonConvert.SerializeObject(this, serializerSettings);
        }

        /// <summary>
        /// Deserialize a <see cref="Message"/> from a json string
        /// </summary>
        /// <param name="json">Json representation of the Message</param>
        /// <returns><see cref="Message"/></returns>
        public static Message FromJson(string json)
        {
            var serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                }
            };
            return JsonConvert.DeserializeObject<Message>(json, serializerSettings);
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
