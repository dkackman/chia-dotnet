namespace chia.dotnet
{
    /// <summary>
    /// This object represents the wallet data as it is stored in DB.
    /// ID: Main wallet (Standard) is stored at index 1, every wallet created after done has auto incremented id.
    /// Name: can be a user provided or default generated name. (can be modified)
    /// Type: is specified during wallet creation and should never be changed.
    /// Data: this filed is intended to be used for storing any wallet specific information required for it.
    /// (RL wallet stores origin_id, admin/user pubkey, rate limit, etc.)
    /// This data should be json encoded string.
    /// </summary>
    public record WalletInfo
    {
        public uint Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public WalletType Type { get; init; }
        public string Data { get; init; } = string.Empty;
    }
}
