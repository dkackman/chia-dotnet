using System.Threading;
using System.Threading.Tasks;

using chia.dotnet;

namespace rchia.Show
{
    static class ExitTask
    {
        public static async Task Run(FullNodeProxy fullNode)
        {
            using var cts = new CancellationTokenSource(5000);
            await fullNode.StopNode(cts.Token);
        }
    }
}
