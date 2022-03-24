namespace chia.dotnet
{
    public enum TradeStatus : uint
    {
        PENDING_ACCEPT = 0,
        PENDING_CONFIRM = 1,
        PENDING_CANCEL = 2,
        CANCELLED = 3,
        CONFIRMED = 4,
        FAILED = 5
    }
}
