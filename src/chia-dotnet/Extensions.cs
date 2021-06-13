using System;
using System.Diagnostics;

namespace chia.dotnet
{
    static class Extensions
    {
        public static void Dump(this Exception e)
        {
            if (e != null)
            {
                Debug.WriteLine(e.Message);
                e.InnerException.Dump();
            }
        }
    }
}
