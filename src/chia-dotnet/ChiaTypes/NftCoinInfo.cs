namespace chia.dotnet
{
    public record NFTCoinInfo
    {

        public string NftCoinId { get; init; } = string.Empty;

        public int WalletId { get; init; }
    }
}
