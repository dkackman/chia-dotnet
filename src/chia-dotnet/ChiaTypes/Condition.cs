using System.Collections.Generic;

using Newtonsoft.Json;

namespace chia.dotnet
{
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
}
