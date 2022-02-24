namespace chia.dotnet
{
    public record PrivateKey
    {
        public string PK { get; init; } = string.Empty;

        public string Entropy { get; init; } = string.Empty;
    }
}
