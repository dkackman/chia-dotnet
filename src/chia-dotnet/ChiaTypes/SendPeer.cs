
using Newtonsoft.Json;

namespace chia.dotnet
{
    /// <summary>
    /// Represents the list of peers that we sent the transaction to, whether each one
    /// included it in the mempool, and what the error message (if any) was
    /// </summary>
    /// <remarks>Represented as `List[Tuple[str, uint8, Optional[str]]]` in python</remarks>
    [JsonConverter(typeof(SendPeerConverter))]
    public record SendPeer
    {
        public string Peer { get; init; } = string.Empty;
        public byte IncludedInMempool { get; init; }
        public string? ErrorMessage { get; init; }
    }
}
