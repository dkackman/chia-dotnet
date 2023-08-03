namespace chia.dotnet
{
    public record TerminalNode
    {
        public string Hash { get; init; } = string.Empty;
        public string Key { get; init; } = string.Empty;
        public string Value { get; init; } = string.Empty;
    }
}
