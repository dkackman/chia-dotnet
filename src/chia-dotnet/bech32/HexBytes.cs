using System;
using System.Linq;
using System.Security.Cryptography;

namespace chia.dotnet.bech32
{
    /// <summary>
    /// Utility to perform operations on an array of bytes and represent them as a Hex string
    /// </summary>
    /// <remarks>adapted from https://github.com/Playwo/ChiaRPC.Net/blob/master/ChiaRPC.Net/Utils/Bech32M.cs</remarks>
    public readonly struct HexBytes
    {
        /// <summary>
        /// <see cref="Bytes"/> hex string representation
        /// </summary>
        public string Hex { get; init; }

        /// <summary>
        /// The array of bytes
        /// </summary>
        public byte[] Bytes { get; init; }

        /// <summary>
        /// FLag indicating that the array is empty
        /// </summary>
        public bool IsEmpty => string.IsNullOrWhiteSpace(Hex);

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="hex"></param>
        /// <param name="bytes"></param>
        public HexBytes(string hex, byte[] bytes)
        {
            Hex = hex ?? string.Empty;
            Bytes = bytes ?? Array.Empty<byte>();
        }

        /// <summary>
        /// SHA256 encoded copy 
        /// </summary>
        /// <returns>Encoded copy</returns>
        public HexBytes Sha256()
        {
            var hash = SHA256.HashData(Bytes);
            var hexHash = HexMate.Convert.ToHexString(hash);

            return new HexBytes(hexHash, hash);
        }

        public override bool Equals(object? obj)
        {
            return obj is HexBytes other && Hex.ToUpperInvariant() == other.Hex.ToUpperInvariant();
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Hex);
        }

        public override string ToString()
        {
            return Hex.ToLowerInvariant();
        }

        public static HexBytes operator +(HexBytes a, HexBytes b)
        {
            var concatHex = string.Concat(a.Hex, b.Hex);
            var concatBytes = a.Bytes.Concat(b.Bytes).ToArray();

            return new HexBytes(concatHex, concatBytes);
        }

        public static HexBytes operator +(HexBytes a, byte[] b)
        {
            var bs = HexMate.Convert.ToHexString(b);
            var concatHex = string.Concat(a.Hex, bs);
            var concatBytes = a.Bytes.Concat(b).ToArray();

            return new HexBytes(concatHex, concatBytes);
        }

        public static HexBytes operator +(HexBytes a, string b)
        {
            var bb = HexMate.Convert.FromHexString(b);
            var concatHex = string.Concat(a.Hex, b);
            var concatBytes = a.Bytes.Concat(bb).ToArray();

            return new HexBytes(concatHex, concatBytes);
        }

        public static bool operator ==(HexBytes a, HexBytes b)
        {
            return a.Hex.ToUpperInvariant() == b.Hex.ToUpperInvariant();
        }

        public static bool operator !=(HexBytes a, HexBytes b)
        {
            return a.Hex.ToUpperInvariant() != b.Hex.ToUpperInvariant();
        }

        public static HexBytes FromHex(string hex)
        {
            return string.IsNullOrWhiteSpace(hex)
                            ? Empty
                            : !hex.StartsWith("0x")
                            ? new HexBytes(hex, HexMate.Convert.FromHexString(hex.AsSpan()))
                            : new HexBytes(hex[2..], HexMate.Convert.FromHexString(hex.AsSpan()[2..]));
        }

        public static bool TryFromHex(string hex, out HexBytes result)
        {
            if (string.IsNullOrWhiteSpace(hex))
            {
                result = Empty;
                return true;
            }

            var bytes = new byte[hex.Length / 2].AsSpan();
            var s = !hex.StartsWith("0x") ? hex.AsSpan() : hex.AsSpan()[2..];
            if (!HexMate.Convert.TryFromHexChars(s, bytes, out var written))
            {
                result = Empty;
                return false;
            }

            result = new HexBytes(s.ToString(), bytes[..written].ToArray());
            return true;
        }

        public static HexBytes FromBytes(byte[] bytes)
        {
            return bytes is null ? Empty : new HexBytes(HexMate.Convert.ToHexString(bytes), bytes);
        }

        /// <summary>
        /// 
        /// </summary>
        public static HexBytes Empty => new(string.Empty, Array.Empty<byte>());
    }
}
