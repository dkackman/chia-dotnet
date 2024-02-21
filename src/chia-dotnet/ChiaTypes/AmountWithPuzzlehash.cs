using System.Collections.Generic;

namespace chia.dotnet
{
    public record AmountWithPuzzlehash
    {
        public ulong Amount { get; init; }
        public string PuzzleHash { get; init; } = string.Empty;
        public IEnumerable<string> Memos { get; init; } = [];
        public string? AssetId { get; init; }
    }
}
