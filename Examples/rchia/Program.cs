using System.Linq;
using System.Reflection;
using System.Diagnostics;

using CommandLine;

using chia.dotnet.console;

namespace rchia
{
    class Program
    {
        internal static readonly ClientFactory Factory = new("rchia");

        static int Main(string[] args)
        {
            var types = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.GetCustomAttribute<VerbAttribute>() != null).ToArray();

            Parser.Default.ParseArguments(args, types)
                  .WithParsed(Runner.Run)
                  .WithNotParsed(Runner.HandleErrors);

            Debug.WriteLine("Exit code {0}", Runner.ReturnCode);
            return Runner.ReturnCode;
        }
    }
}
