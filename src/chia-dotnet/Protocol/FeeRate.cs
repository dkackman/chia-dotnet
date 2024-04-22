namespace chia.dotnet.protocol;

public record FeeRate
{
    /// <summary>
    /// Represents Fee Rate in mojos divided by CLVM Cost.
    /// Performs XCH/mojo conversion.
    /// Similar to 'Fee per cost'.
    /// </summary>
    public ulong MojosPerClvmCost { get; init; }
}
