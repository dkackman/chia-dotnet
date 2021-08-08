namespace chia.dotnet
{
    public record WalletInfo
    {
        public uint Id { get; init; }
        public string Name { get; init; }
        public byte Type { get; init; }
        public string Data { get; init; }
    }
}
