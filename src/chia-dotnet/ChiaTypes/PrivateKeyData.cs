namespace chia.dotnet
{
    public record PrivateKeyData
    {
        public string PK { get; init; } = string.Empty;

        public string Entropy { get; init; } = string.Empty;
    }
}
