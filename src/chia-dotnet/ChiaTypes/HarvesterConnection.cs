namespace chia.dotnet
{
    public record HarvesterConnection
    {
        public string Host { get; init; } = string.Empty;
        public string NodeId { get; init; } = string.Empty;
        public int Port { get; init; }
    }
}
