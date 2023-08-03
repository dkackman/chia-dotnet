namespace chia.dotnet
{
    public record Layer
    {
        public Side OtherHashSide { get; init; }
        public string OtherHash { get; init; } = string.Empty;
        public string CombinedHash { get; init; } = string.Empty;
    }
}
