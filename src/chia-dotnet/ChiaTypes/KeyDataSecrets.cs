using System.Collections.Generic;

namespace chia.dotnet
{
    public record KeyDataSecrets
    {
        public IEnumerable<string> Mnemonic { get; init; } = new List<string>();
        public string Bytes { get; init; } = string.Empty;
        public PrivateKey PrivateKey { get; init; } = new();
    }
}
