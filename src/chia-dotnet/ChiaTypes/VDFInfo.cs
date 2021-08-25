namespace chia.dotnet
{
    public record VDFInfo
    {
        /// <summary>
        /// Used to generate the discriminant (VDF group)
        /// </summary>
        public string Challenge { get; init; } = string.Empty;
        public ulong NumberOfIterations { get; init; }
        public ClassgroupElement Output { get; init; } = new();
    }
}
