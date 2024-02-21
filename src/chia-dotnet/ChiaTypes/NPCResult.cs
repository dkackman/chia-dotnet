using System.Collections.Generic;

namespace chia.dotnet
{
    public record NPCResult
    {
        public ushort? Error { get; init; }
        public IEnumerable<NPC> NpcList { get; init; } = [];
        /// <summary>
        /// CLVM cost only, cost of conditions and tx size is not included
        /// </summary>
        public ulong ClvmCost { get; init; }
    }
}
