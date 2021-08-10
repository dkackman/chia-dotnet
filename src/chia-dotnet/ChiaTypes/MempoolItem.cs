using System.Collections.Generic;

using Newtonsoft.Json;

namespace chia.dotnet
{
    public record ConditionWithArgs
    {
        public string Opcode { get; init; }
        public ICollection<string> Vars { get; init; }
    }

    /// <summary>
    /// This type doesn't exist in the chia code. This property is serialzied into the Json
    /// as a tuple, which shows up as a mixed type json array. There is a speciailzed converter to read
    /// the Json and get the ConditionOpCode into <see cref="Condition.ConditionOpcode"/>
    /// conditions: List[Tuple[ConditionOpcode, List[ConditionWithArgs]]]
    /// </summary>
    [JsonConverter(typeof(ConditionConverter))]
    public record Condition
    {
        public string ConditionOpcode { get; init; }
        public ICollection<ConditionWithArgs> Args { get; init; }
    }

    public record NPC
    {
        public string CoinName { get; init; }
        public string PuzzleHash { get; init; }
        public ICollection<Condition> Conditions { get; init; }
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
