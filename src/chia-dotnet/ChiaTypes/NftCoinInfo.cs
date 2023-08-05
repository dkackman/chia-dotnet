namespace chia.dotnet
{
    public record NftCoinInfo
    {

        public string NftCoinId { get; init; } = string.Empty;

        public int WalletId { get; init; }
    }
}
