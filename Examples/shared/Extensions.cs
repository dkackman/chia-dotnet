using System;
using System.Numerics;

namespace chia.dotnet.console
{
    public static class Extensions
    {
        public static string ToBytesString(this BigInteger byteCount)
        {
            return ((ulong)byteCount).ToBytesString();
        }

        public static string ToBytesString(this uint byteCount)
        {
            return ((ulong)byteCount).ToBytesString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="byteCount"></param>
        /// <returns></returns>
        /// <remarks>from https://stackoverflow.com/questions/281640/how-do-i-get-a-human-readable-file-size-in-bytes-abbreviation-using-net </remarks>
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
