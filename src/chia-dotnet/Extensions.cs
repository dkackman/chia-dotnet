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
        /// Format a number of bytes in human readable format 
        /// </summary>
        /// <param name="byteCount">The number of bytes</param>
        /// <returns>A human readable string</returns>
        /// <remarks>byteCount will be cast to ulong</remarks>
        public static string ToBytesString(this BigInteger byteCount)
        {
            return ((ulong)byteCount).ToBytesString();
        }

        /// <summary>
        /// Format a number of bytes in human readable format 
        /// </summary>
        /// <param name="byteCount">The number of bytes</param>
        /// <returns>A human readable string</returns>
        public static string ToBytesString(this uint byteCount)
        {
            return ((ulong)byteCount).ToBytesString();
        }

        /// <summary>
        /// Format a number of bytes in human readable format 
        /// </summary>
        /// <param name="byteCount">The number of bytes</param>
        /// <returns>A human readable string</returns>
        /// <remarks>Adapted from https://stackoverflow.com/questions/281640/how-do-i-get-a-human-readable-file-size-in-bytes-abbreviation-using-net </remarks>
        public static string ToBytesString(this ulong byteCount)
        {
            string[] suf = { "B", "KiB", "MiB", "GiB", "TiB", "PiB", "EiB", "YiB" };
            if (byteCount == 0)
            {
                return "0 " + suf[0];
            }

            var place = Convert.ToInt32(Math.Floor(Math.Log(byteCount, 1024)));
            var num = Math.Round(byteCount / Math.Pow(1024, place), 1);

            return num.ToString("N3") + " " + suf[place];
        }
    }
}
