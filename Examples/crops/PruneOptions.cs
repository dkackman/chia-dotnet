using System;
using System.Threading.Tasks;

using chia.dotnet.console;

using CommandLine;

namespace crops
{
    [Verb("prune", HelpText = "Prune stale connections - by default nodes with smaller peak heights")]
    public class PruneOptions : SharedOptions
    {
        [Option('o', "prune-old", HelpText = "Prune connections that haven't sent data reccently instead of by height")]
        public bool ProneOld { get; set; }

        [Option('a', "age", Default = 12, HelpText = "Maximum age, in hours, when using prune-old")]
        public int Age { get; set; } = 12;

        public override async Task<int> Run()
        {
            try
            {
                var pruner = new Pruner();
                await pruner.Prune(this);

                return 0;
            }
            catch (Exception e)
            {
                Message(e.Message, true);

                return -1;
            }
        }
    }
}
