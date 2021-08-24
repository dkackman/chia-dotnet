namespace chia.dotnet
{
    /// <summary>
    /// Response in error case for all endpoints of the pool protocol
    /// </summary>
    public record ErrorResponse
    {
        public ushort ErrorCode { get; init; }
        public string? ErrorMessage { get; init; }
    }
}
