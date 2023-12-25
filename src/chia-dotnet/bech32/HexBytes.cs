using System;
using System.Linq;
using System.Security.Cryptography;

namespace chia.dotnet.bech32
{
    /// <summary>
    /// Utility to perform operations on an array of bytes and represent them as a Hex string
    /// </summary>
    /// <remarks>adapted from https://github.com/Playwo/ChiaRPC.Net/blob/master/ChiaRPC.Net/Utils/Bech32M.cs</remarks>
    /// <remarks>
    /// ctor
    /// </remarks>
    /// <param name="hex"></param>
    /// <param name="bytes"></param>
    public readonly struct HexBytes(string hex, byte[] bytes)
    {
        /// <summary>
        /// <see cref="Bytes"/> hex string representation
        /// </summary>
        public string Hex { get; init; } = hex ?? string.Empty;

        /// <summary>
        /// The array of bytes
        /// </summary>
        public byte[] Bytes { get; init; } = bytes ?? [];

        /// <summary>
        /// FLag indicating that the array is empty
        /// </summary>
        public bool IsEmpty => string.IsNullOrWhiteSpace(Hex);

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
            return obj is HexBytes other && Hex.Equals(other.Hex, StringComparison.OrdinalIgnoreCase);
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
            return a.Hex.Equals(b.Hex, StringComparison.OrdinalIgnoreCase);
        }

        public static bool operator !=(HexBytes a, HexBytes b)
        {
            return !a.Hex.Equals(b.Hex, StringComparison.OrdinalIgnoreCase);
        }

        public static HexBytes FromHex(string hex)
        {
            return string.IsNullOrWhiteSpace(hex)
                            ? Empty
                            : !hex.StartsWith("0x", StringComparison.OrdinalIgnoreCase)
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
            var s = !hex.StartsWith("0x", StringComparison.OrdinalIgnoreCase) ? hex.AsSpan() : hex.AsSpan()[2..];
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
        public static HexBytes Empty => new(string.Empty, []);
    }
}
