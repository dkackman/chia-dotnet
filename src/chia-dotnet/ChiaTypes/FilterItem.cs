namespace chia.dotnet
{
    public record FilterItem
    {
        public string Key { get; init; } = string.Empty;
        public string? Value { get; init; }
    }
}
