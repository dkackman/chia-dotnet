namespace chia.dotnet
{
    /// <summary>
    /// Represents a classgroup element (a,b,c) where a, b, and c are 512 bit signed integers. However this is using
    /// a compressed representation. VDF outputs are a single classgroup element. VDF proofs can also be one classgroup
    /// element(or multiple).
    /// </summary>
    public record ClassgroupElement
    {
        public string Data { get; init; } = string.Empty;
    }
}
