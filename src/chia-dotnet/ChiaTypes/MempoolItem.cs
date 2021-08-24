using System.Collections.Generic;

using Newtonsoft.Json;

namespace chia.dotnet
{
    /// <summary>
    /// This structure is used to store parsed CLVM conditions
    /// Conditions in CLVM have either format of(opcode, var1) or(opcode, var1, var2)
    /// </summary>
    public record ConditionWithArgs
    {
        public string Opcode { get; init; } = string.Empty;
        public ICollection<string> Vars { get; init; } = new List<string>();
    }

    /// <summary>
    /// This type doesn't exist in the chia code. This property is serialzied into the Json
    /// as a tuple, which shows up as a mixed type json array. There is a speciailzed converter to read
    /// the Json and get the ConditionOpCode into <see cref="ConditionOpcode"/>
    /// conditions: List[Tuple[ConditionOpcode, List[ConditionWithArgs]]]
    /// </summary>
    [JsonConverter(typeof(ConditionConverter))]
    public record Condition
    {
        public string ConditionOpcode { get; init; } = string.Empty;
        public ICollection<ConditionWithArgs> Args { get; init; } = new List<ConditionWithArgs>();
    }

    public record NPC
    {
        public string CoinName { get; init; } = string.Empty;
        public string PuzzleHash { get; init; } = string.Empty;
        public ICollection<Condition> Conditions { get; init; } = new List<Condition>();
    }

    public record NPCResult
    {
        public ushort? Error { get; init; }
        public ICollection<NPC> NpcList { get; init; } = new List<NPC>();
        /// <summary>
        /// CLVM cost only, cost of conditions and tx size is not included
        /// </summary>
        public ulong ClvmCost { get; init; }
    }

    public record MempoolItem
    {
        public SpendBundle SpendBundle { get; init; } = new();
        public ulong Fee { get; init; }
        public NPCResult NPCResult { get; init; } = new();
        public ulong Cost { get; init; }
        public string SpendBudndleName { get; init; } = string.Empty;
        public ICollection<Coin> Additions { get; init; } = new List<Coin>();
        public ICollection<Coin> Removals { get; init; } = new List<Coin>();
        public string Program { get; init; } = string.Empty;
    }
}
