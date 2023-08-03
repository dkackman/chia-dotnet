namespace chia.dotnet
{
    public record InternalNode
    {
        public string Hash { get; init; } = string.Empty;
        public string LeftHash { get; init; } = string.Empty;
        public string RightHash { get; init; } = string.Empty;
        // TODO - need test case for serialization for this - tuple might not be the right type
        public (string, string) Pair { get; init; } = new();
        public string Atom { get; init; } = string.Empty;
    }
}
