namespace chia.dotnet
{
    public record PassphraseRequirements
    {
        public bool IsOptional { get; init; }
        public int MinLength { get; init; }
    }
}
