using System;
using System.Diagnostics;
using System.Threading.Tasks;

using CommandLine;

namespace chia.dotnet.console
{
    public abstract class SharedOptions : IVerb
    {
        [Option('v', "verbose", HelpText = "Set output to verbose messages.")]
        public bool Verbose { get; set; }

        [Option("endpoint-uri", SetName = "Endpoint", HelpText = "The uri of the rpc endpoint, including the proper port and wss/https scheme prefix")]
        public string Uri { get; set; }

        [Option("cert-path", SetName = "Endpoint", HelpText = "The full path to the .crt file to use for authentication")]
        public string CertPath { get; set; }

        [Option("key-path", SetName = "Endpoint", HelpText = "The full path to the .key file to use for authentication")]
        public string KeyPath { get; set; }

        [Option("config-path", SetName = "Config", HelpText = "The full path to a chia config yaml file for endpoints")]
        public string ConfigPath { get; set; }

        [Option("use-default-config", SetName = "Config", HelpText = "Flag indicating to use the default chia config for endpoints")]
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
