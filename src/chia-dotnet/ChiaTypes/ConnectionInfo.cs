using System;

using Newtonsoft.Json;

namespace chia.dotnet
{
    /// <summary>
    /// Chia's representation of a connection from node to node
    /// </summary>
    public record ConnectionInfo
    {
        public uint? BytesRead { get; init; }
        public uint? BytesWritten { get; init; }
        public double CreationTime { get; init; }
        public double LastMessageTime { get; init; }
        public int LocalPort { get; init; }
        public string NodeId { get; init; } = string.Empty;
        public string PeakHash { get; init; } = string.Empty;
        public ulong? PeakHeight { get; init; }
        public ulong? PeakWeight { get; init; }
        public string PeerHost { get; init; } = string.Empty;
        public int PeerPort { get; init; }
        public int PeerServerPort { get; init; }
        public NodeType Type { get; init; }
        [JsonIgnore]
        public DateTime CreationDateTime => CreationTime.ToDateTime();
        [JsonIgnore]
        public DateTime LastMessageDateTime => LastMessageTime.ToDateTime();
    }
}
