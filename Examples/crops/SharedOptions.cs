using System;
using System.Diagnostics;
using System.Threading.Tasks;

using CommandLine;

namespace crops
{
    public abstract class SharedOptions : IVerb
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

        public void Message(string msg, bool important = false)
        {
            if (Verbose || important)
            {
                Console.WriteLine(msg);
            }

            Debug.WriteLine(msg);
        }

        public abstract Task<int> Run();
    }
}
