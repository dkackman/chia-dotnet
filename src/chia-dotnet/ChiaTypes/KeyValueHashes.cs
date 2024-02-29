namespace chia.dotnet
{
    public record KeyValueHashes
    {
        public string KeyClvmHash { get; init; } = string.Empty;
        public string ValueClvmHash { get; init; } = string.Empty;
    }
}
