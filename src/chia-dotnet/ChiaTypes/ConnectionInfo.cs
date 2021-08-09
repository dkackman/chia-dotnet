using System;

namespace chia.dotnet
{
    /// <summary>
    /// Chia's representation of a connection from node to node
    /// </summary>
    public record ConnectionInfo
    {
        public uint? BytesRead { get; init; } // 16112675,
        public uint? BytesWritten { get; init; } // 1613222,
        public double CreationTime { get; init; } // 1628272396.9376426,
        public double LastMessageTime { get; init; } // 1628347701.478834,
        public int LocalPort { get; init; } // 58444,
        public string NodeId { get; init; } // 0x1de32a64d2e0924bdb9d93aa8045a6f0a6968f7b241ccaabe354a9da5913469f,
        public string PeakHash { get; init; } // 0x2142d087065ee21a3a2405673da90c83c26fd25a20b7ad3d3a1e63b6f34b87eb,
        public ulong? PeakHeight { get; init; } // 418571,
        public ulong? PeakWeight { get; init; } // 41291924871,
        public string PeerHost { get; init; } // 161.97.175.241,
        public int PeerPort { get; init; } // 58444,
        public int PeerServerPort { get; init; } // 58444,
        public byte Type { get; init; } // 1

        public DateTime CreationDateTime => CreationTime.ToDateTime();
        public DateTime LastMessageDateTime => LastMessageTime.ToDateTime();
    }
}
