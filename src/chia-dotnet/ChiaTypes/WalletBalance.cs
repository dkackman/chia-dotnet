namespace chia.dotnet
{
    public record WalletBalance
    {
        public System.UInt128 ConfirmedWalletBalance { get; init; }
        public System.UInt128 UnconfirmedWalletBalance { get; init; }
        public System.UInt128 SpendableBalance { get; init; }
        public System.UInt128 PendingChange { get; init; }
        public System.UInt128 MaxSendAmount { get; init; }
        public int UnspentCoinCount { get; init; }
        public int PendingCoinRemovalCount { get; init; }
        public WalletType WalletType { get; init; }
        public string? AssetId { get; init; }
        public uint WalletId { get; init; }
        public uint? Fingerprint { get; init; }
    }
}
