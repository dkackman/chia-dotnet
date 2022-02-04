namespace chia.dotnet
{
    public record WalletBalance
    {
        public ulong ConfirmedWalletBalance { get; init; }
        public ulong UnconfirmedWalletBalance { get; init; }
        public ulong SpendableBalance { get; init; }
        public ulong PendingChange { get; init; }
        public ulong MaxSendAmount { get; init; }
        public int UnspentCoinCount { get; init; }
        public int PendingCoinRemovalCount { get; init; }
    }
}
