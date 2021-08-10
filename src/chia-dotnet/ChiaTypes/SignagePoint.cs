namespace chia.dotnet
{
    public record SignagePoint
    {
        public VDFInfo CcVdf { get; init; }
        public VDFProof CcProof { get; init; }
        public VDFInfo RcVdf { get; init; }
        public VDFProof RcProof { get; init; }
    }
}
