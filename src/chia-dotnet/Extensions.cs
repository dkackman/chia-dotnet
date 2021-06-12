using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
