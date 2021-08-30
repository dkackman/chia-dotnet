using Newtonsoft.Json;

namespace chia.dotnet
{
    public record HarvesterConnection
    {
        public string Host { get; init; } = string.Empty;
        public string NodeId { get; init; } = string.Empty;
        public int Port { get; init; }
        /// <summary>
        /// Flag indicating whether the harvester is local to the node
        /// </summary>
        [JsonIgnore]
        public bool IsLocal => Host is "127.0.0.1" or "localhost" or "::1" or "0:0:0:0:0:0:0:1";
    }
}
