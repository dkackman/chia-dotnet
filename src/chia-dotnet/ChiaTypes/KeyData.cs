namespace chia.dotnet
{
    public record KeyData
    {
        public uint Fingerprint { get; init; }
        public string PublicKey { get; init; } = string.Empty;
        public string? Label { get; init; }
        public KeyDataSecrets? Secrets { get; init; }
    }
}
