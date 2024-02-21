using System.Collections.Generic;

namespace chia.dotnet
{
    public record NPC
    {
        public string CoinName { get; init; } = string.Empty;
        public string PuzzleHash { get; init; } = string.Empty;
        public IEnumerable<Condition> Conditions { get; init; } = [];
    }
}
