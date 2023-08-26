namespace chia.dotnet
{
    public record WalletAddress
    {
        public string Address { get; init; } = string.Empty;
        public string HdPath { get; init; } = string.Empty;
    }
}
