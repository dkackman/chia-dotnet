using System.Collections.Generic;

namespace chia.dotnet
{
    public record NPCResult
    {
        public ushort? Error { get; init; }
        public ICollection<NPC> NpcList { get; init; } = new List<NPC>();
        /// <summary>
        /// CLVM cost only, cost of conditions and tx size is not included
        /// </summary>
        public ulong ClvmCost { get; init; }
    }
}
