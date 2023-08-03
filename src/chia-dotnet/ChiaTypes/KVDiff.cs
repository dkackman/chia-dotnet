namespace chia.dotnet
{
    public record KVDiff
    {
        public string Type { get; init; } = string.Empty;
        public string Key { get; init; } = string.Empty;
        public string Value { get; init; } = string.Empty;
    }
}
