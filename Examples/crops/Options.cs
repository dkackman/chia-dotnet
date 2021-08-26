using System;
using System.Diagnostics;

using System.Threading.Tasks;

using CommandLine;

namespace crops
{
    public class SharedOptions
    {
        [Option('v', "verbose", HelpText = "Set output to verbose messages.")]
        public bool Verbose { get; set; }

        [Option('e', "EndpointUri", SetName = "Endpoint", HelpText = "The uri of the rpc endpoint, including the proper port and wss/https scheme prefix")]
        public string Uri { get; set; }

        [Option('c', "CertPath", SetName = "Endpoint", HelpText = "The full path to the .crt file to use for authentication")]
        public string CertPath { get; set; }

        [Option('k', "KeyPath", SetName = "Endpoint", HelpText = "The full path to the .key file to use for authentication")]
        public string KeyPath { get; set; }

        [Option('p', "ConfigPath", SetName = "Config", HelpText = "The full path to a chia config yaml file")]
        public string ConfigPath { get; set; }

        [Option('u', "UseDefaultConfig", SetName = "Config", HelpText = "Flag indicating to use the default path to the chia config file")]
        public bool UseDefaultConfig { get; set; }

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
        [Option('o', "ProneOld", HelpText = "Prune connections that havent sent data in the last 24 hours")]
        public bool ProneOld { get; set; }

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
                Message(e.Message, true);

                return -1;
            }
        }
    }
}
