using System;
using System.Numerics;

namespace chia.dotnet
{
    /// <summary>
    /// Helper extension methods
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// There are 1 tillion mojo in a chia
        /// </summary>
        public const decimal OneTrillion = 1000000000000M;

        /// <summary>
        /// Converts an amount of mojo to the same amount in chia, converting from <see cref="ulong"/> to <see cref="decimal"/>
        /// </summary>
        /// <param name="mojo">The amount of mojo</param>
        /// <returns>The amount of chia</returns>
        public static decimal ToChia(this ulong mojo)
        {
            return mojo / OneTrillion;
        }

        /// <summary>
        /// Converts an amount of chia to the same amount in mojo, converting from <see cref="decimal"/> to <see cref="ulong"/>
        /// </summary>
        /// <param name="chia">The amount of chia</param>
        /// <returns>The amount of mojo</returns>
        public static ulong ToMojo(this decimal chia)
        {
            return decimal.ToUInt64(decimal.Floor(chia * OneTrillion));
        }

        /// <summary>
        /// Formats a value expressed in mojo to chia
        /// </summary>
        /// <param name="mojo">The amount of mojo</param>
        /// <returns>Formatted string expressed in a unit of chia</returns>
        public static string AsChia(this ulong mojo)
        {
            return mojo.ToChia().ToString();
        }

        /// <summary>
        /// Formats a value expressed in mojo to chia
        /// </summary>
        /// <param name="mojo">The amount of mojo</param>
        /// <param name="provider"> An object that supplies culture-specific formatting information.</param>
        /// <returns>Formatted string expressed in a unit of chia</returns>
        public static string AsChia(this ulong mojo, IFormatProvider? provider)
        {
            return mojo.ToChia().ToString(provider);
        }

        /// <summary>
        /// Formats a value expressed in mojo to chia
        /// </summary>
        /// <param name="mojo">The amount of mojo</param>
        /// <param name="format">A numeric format string.</param>
        /// <returns>Formatted string expressed in a unit of chia</returns>
        public static string AsChia(this ulong mojo, string? format)
        {
            return mojo.ToChia().ToString(format);
        }

        /// <summary>
        /// Formats a value expressed in mojo to chia
        /// </summary>
        /// <param name="mojo">The amount of mojo</param>
        /// <param name="format">A numeric format string.</param>
        /// <param name="provider"> An object that supplies culture-specific formatting information.</param>
        /// <returns>Formatted string expressed in a unit of chia</returns>
        public static string AsChia(this ulong mojo, string? format, IFormatProvider? provider)
        {
            return mojo.ToChia().ToString(format, provider);
        }

        /// <summary>
        /// Format a number of bytes in human readable format 
        /// </summary>
        /// <param name="byteCount">The number of bytes</param>
        /// <param name="format">Return string culture format</param>
        /// <returns>A human readable string</returns>
        /// <remarks>Adapted from https://stackoverflow.com/questions/281640/how-do-i-get-a-human-readable-file-size-in-bytes-abbreviation-using-net </remarks>
        public static string ToBytesString(this BigInteger byteCount, string format = "N3")
        {
            string[] suffixes = ["B", "KiB", "MiB", "GiB", "TiB", "PiB", "EiB", "YiB"];
            if (byteCount.IsZero)
            {
                return $"{0.0.ToString(format)} {suffixes[0]}";
            }

            var abs = BigInteger.Abs(byteCount); // in case byteCount is negative
            var place = Convert.ToInt32(Math.Floor(BigInteger.Log(abs, 1024)));
            var pow = Math.Pow(1024, place);

            // since we need to do this with integer math, get the quotient and remainder
            var quotient = BigInteger.DivRem(abs, new BigInteger(pow), out var remainder);
            // convert the remainder to a ratio and add both back together as doubles, putting the sign back
            var num = byteCount.Sign * (Math.Floor((double)quotient) + ((double)remainder / pow));

            return $"{num.ToString(format)} {suffixes[place]}";
        }

        /// <summary>
        /// Format a number of bytes in human readable format 
        /// </summary>
        /// <param name="byteCount">The number of bytes</param>
        /// <param name="format">Return string culture format</param>
        /// <returns>A human readable string</returns>
        /// <remarks>Adapted from https://stackoverflow.com/questions/281640/how-do-i-get-a-human-readable-file-size-in-bytes-abbreviation-using-net </remarks>
        public static string ToBytesString(this UInt128 byteCount, string format = "N3")
        {
            string[] suffixes = ["B", "KiB", "MiB", "GiB", "TiB", "PiB", "EiB", "YiB"];
            if (byteCount == 0)
            {
                return $"{0.0.ToString(format)} {suffixes[0]}";
            }

            var abs = BigInteger.Abs(byteCount); // in case byteCount is negative
            var place = Convert.ToInt32(Math.Floor(BigInteger.Log(abs, 1024)));
            var pow = Math.Pow(1024, place);

            // since we need to do this with integer math, get the quotient and remainder
            var quotient = BigInteger.DivRem(abs, new BigInteger(pow), out var remainder);
            // convert the remainder to a ratio and add both back together as doubles, putting the sign back
            var num = UInt128.Sign(byteCount) * (Math.Floor((double)quotient) + ((double)remainder / pow));

            return $"{num.ToString(format)} {suffixes[place]}";
        }

        /// <summary>
        /// Format a number of bytes in human readable format 
        /// </summary>
        /// <param name="byteCount">The number of bytes</param>
        /// <param name="format">Return string culture format</param>         
        /// <returns>A human readable string</returns>
        /// <remarks>Adapted from https://stackoverflow.com/questions/281640/how-do-i-get-a-human-readable-file-size-in-bytes-abbreviation-using-net </remarks>
        public static string ToBytesString(this ulong byteCount, string format = "N3")
        {
            string[] suffixes = ["B", "KiB", "MiB", "GiB", "TiB", "PiB", "EiB", "YiB"];
            if (byteCount == 0)
            {
                return $"{0.0.ToString(format)} {suffixes[0]}";
            }

            var place = Convert.ToInt32(Math.Floor(Math.Log(byteCount, 1024)));
            var num = Math.Round(byteCount / Math.Pow(1024, place), 1);

            return $"{num.ToString(format)} {suffixes[place]}";
        }

        /// <summary>
        /// Format a number of bytes in human readable format 
        /// </summary>
        /// <param name="byteCount">The number of bytes</param>
        /// <param name="format">Return string culture format</param>         
        /// <returns>A human readable string</returns>
        public static string ToBytesString(this uint byteCount, string format = "N3")
        {
            return ((ulong)byteCount).ToBytesString(format);
        }

        /// <summary>
        /// Format a number of bytes in human readable format 
        /// </summary>
        /// <param name="byteCount">The number of bytes</param>
        /// <param name="format">Return string culture format</param>
        /// <returns>A human readable string</returns>
        public static string ToBytesString(this int byteCount, string format = "N3")
        {
            return ((long)byteCount).ToBytesString(format);
        }

        /// <summary>
        /// Format a number of bytes in human readable format 
        /// </summary>
        /// <param name="byteCount">The number of bytes</param>
        /// <param name="format">Return string culture format</param>
        /// <returns>A human readable string</returns>
        public static string ToBytesString(this long byteCount, string format = "N3")
        {
            string[] suffixes = ["B", "KiB", "MiB", "GiB", "TiB", "PiB", "EiB", "YiB"];
            if (byteCount == 0)
            {
                return $"{0.0.ToString(format)} {suffixes[0]}";
            }

            var abs = Math.Abs(byteCount); // in case byteCount is negative
            var place = Convert.ToInt32(Math.Floor(Math.Log(abs, 1024)));
            var num = Math.Sign(byteCount) * Math.Round(abs / Math.Pow(1024, place), 1);

            return $"{num.ToString(format)} {suffixes[place]}";
        }

        /// <summary>
        /// Format a number of bytes in human readable format 
        /// </summary>
        /// <param name="byteCount">The number of bytes</param>
        /// <param name="format">Return string culture format</param>
        /// <returns>A human readable string</returns>
        public static string ToBytesString(this double byteCount, string format = "N3")
        {
            string[] suffixes = ["B", "KiB", "MiB", "GiB", "TiB", "PiB", "EiB", "YiB"];
            if (byteCount == 0)
            {
                return $"{0.0.ToString(format)} {suffixes[0]}";
            }

            var abs = Math.Abs(byteCount); // in case byteCount is negative
            var place = Convert.ToInt32(Math.Floor(Math.Log(abs, 1024)));
            var num = Math.Sign(byteCount) * Math.Round(abs / Math.Pow(1024, place), 1);

            return $"{num.ToString(format)} {suffixes[place]}";
        }
    }
}
