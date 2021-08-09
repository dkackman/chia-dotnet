using System.Collections.Generic;
using Newtonsoft.Json;

namespace chia.dotnet
{
    public record ConditionWithArgs
    {
        public string Opcode { get; init; }
        public ICollection<string> Vars { get; init; }
    }

    public record Condition
    {

    }

    public record NPC
    {
        public string CoinName { get; init; }
        public string PuzzleHash { get; init; }
        public ICollection<dynamic> Conditions { get; init; } //conditions: List[Tuple[ConditionOpcode, List[ConditionWithArgs]]]
    }

    public record NPCResult
    {
        public ushort? Error { get; init; }
        public ICollection<NPC> NpcList { get; init; }
        public ulong ClvmCost { get; init; }
    }

    public record MempoolItem
    {
        public SpendBundle SpendBundle { get; init; }
        public ulong Fee { get; init; }
        public NPCResult NPCResult { get; init; }
        public ulong Cost { get; init; }
        public string SpendBudndleName { get; init; }
        public ICollection<Coin> Additions { get; init; }
        public ICollection<Coin> Removals { get; init; }
        public string Program { get; init; }
    }
}
