namespace chia.dotnet
{
    public record PoolTarget
    {
        public string PuzzleHash { get; init; } = string.Empty;
        /// <summary>
        /// A max height of 0 means it is valid forever
        /// </summary>
        public uint MaxHeight { get; init; }
    }
}
