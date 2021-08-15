using System;

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
        public string NodeId { get; init; }
        public string PeakHash { get; init; }
        public ulong? PeakHeight { get; init; }
        public ulong? PeakWeight { get; init; }
        public string PeerHost { get; init; }
        public int PeerPort { get; init; }
        public int PeerServerPort { get; init; }
        public byte Type { get; init; }

        public DateTime CreationDateTime => CreationTime.ToDateTime();
        public DateTime LastMessageDateTime => LastMessageTime.ToDateTime();
    }
}
