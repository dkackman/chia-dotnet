using System;
using System.IO;
using System.Security.Cryptography;

namespace chia.dotnet.protocol; 

public interface IStreamable
{
    void UpdateDigest(SHA256 digest);
    void Stream(Stream output);
    Bytes Parse(Stream input, bool trusted);
}

public interface IClvmEncoder<N>
{
    N EncodeAtom(byte[] data);
}

public interface IClvmDecoder<N>
{
    N DecodeAtom(byte[] data);
}

public class Bytes : IStreamable
{
    private byte[] data;

    public Bytes(byte[] data) => this.data = data;

    public int Length => data.Length;

    public bool IsEmpty => data.Length == 0;

    public byte[] Data => data;

    public override string ToString() => BitConverter.ToString(data).Replace("-", "").ToLower();

    public void UpdateDigest(SHA256 digest)
    {
        digest.TransformBlock(data, 0, data.Length, null, 0);
    }

    public void Stream(Stream output)
    {
        if (data.Length > ushort.MaxValue)
            throw new InvalidOperationException("Data too large for streaming.");

        byte[] lengthBytes = BitConverter.GetBytes((uint)data.Length);
        if (BitConverter.IsLittleEndian)
            Array.Reverse(lengthBytes);
        output.Write(lengthBytes, 0, lengthBytes.Length);
        output.Write(data, 0, data.Length);
    }

    public Bytes Parse(Stream input, bool trusted)
    {
        byte[] lengthBytes = new byte[4];
        if (input.Read(lengthBytes, 0, 4) != 4)
            throw new EndOfStreamException();

        if (BitConverter.IsLittleEndian)
            Array.Reverse(lengthBytes);
        int length = BitConverter.ToInt32(lengthBytes, 0);

        byte[] buffer = new byte[length];
        if (input.Read(buffer, 0, length) != length)
            throw new EndOfStreamException();

        return new Bytes(buffer);
    }
}
