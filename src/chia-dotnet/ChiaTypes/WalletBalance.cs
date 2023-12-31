using System.Numerics;

namespace chia.dotnet
{
    public record WalletBalance
    {
        public BigInteger ConfirmedWalletBalance { get; init; }
        public BigInteger UnconfirmedWalletBalance { get; init; }
        public BigInteger SpendableBalance { get; init; }
        public BigInteger PendingChange { get; init; }
        public BigInteger MaxSendAmount { get; init; }
        public int UnspentCoinCount { get; init; }
        public int PendingCoinRemovalCount { get; init; }
        public WalletType WalletType { get; init; }
        public string? AssetId { get; init; }
        public uint WalletId { get; init; }
        public uint? Fingerprint { get; init; }
    }
}
