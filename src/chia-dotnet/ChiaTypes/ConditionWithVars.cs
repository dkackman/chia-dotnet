using System.Collections.Generic;

namespace chia.dotnet
{
    /// <summary>
    /// This structure is used to store parsed CLVM conditions
    /// Conditions in CLVM have either format of(opcode, var1) or(opcode, var1, var2)
    /// </summary>
    public record ConditionWithVars
    {
        public ushort Opcode { get; init; }
        public IEnumerable<string> Vars { get; init; } = new List<string>();
    }
}
