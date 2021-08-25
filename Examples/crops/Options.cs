using System;
using System.Diagnostics;

using System.Threading.Tasks;

using CommandLine;

namespace crops
{
    public class SharedOptions
    {
        [Option('v', "verbose", Required = false, HelpText = "Set output to verbose messages.")]
        public bool Verbose { get; set; }

        internal void Message(string msg, bool important = false)
        {
            if (Verbose || important)
            {
                Console.WriteLine(msg);
            }

            Debug.WriteLine(msg);
        }
    }

    [Verb("prune", HelpText = "Prune connections that have a lower peak height than the node")]
    public class PruneOptions : SharedOptions, IVerb
    {
        [Option('u', Required = true, HelpText = "The uri of the rpc endpoint, including the proper port and wss/https scheme prefix")]
        public string Uri { get; set; }

        [Option('c', Required = true, HelpText = "The full path to the .crt file to use for authentication")]
        public string CertPath { get; set; }

        [Option('k', Required = true, HelpText = "The full path to the .key file to use for authentication")]
        public string KeyPath { get; set; }

        public async Task<int> Run()
        {
            try
            {
                var pruner = new Pruner();
                await pruner.Prune(this);

                return 0;
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
                return -1;
            }
        }
    }
}
