using System;
using System.Threading.Tasks;

using chia.dotnet;
using chia.dotnet.console;

using rchia.Show;

using CommandLine;

namespace rchia
{
    [Verb("show", HelpText = "Shows various properties of a full node")]
    class ShowVerb : SharedOptions
    {
        [Option('s', "state", SetName = "state", HelpText = "Show the current state of the blockchain")]
        public bool State { get; set; }

        [Option('e', "exit-node", SetName = "exit-node", HelpText = "Shut down the running Full Node")]
        public bool Exit { get; set; }

        [Option('c', "connections", SetName = "connections", HelpText = "List nodes connected to this Full Node")]
        public bool Connections { get; set; }

        [Option('a', "add-connection", SetName = "add-connection", HelpText = "Connect to another Full Node by ip:port")]
        public string AddConnection { get; set; }

        [Option('r', "remove-connection", SetName = "remove-connection", HelpText = "Remove a Node by the first 8 characters of NodeID")]
        public string RemoveConnection { get; set; }

        public override async Task<int> Run()
        {
            try
            {
                using var rpcClient = await Program.Factory.CreateRpcClient(this, ServiceNames.FullNode);
                var fullNode = new FullNodeProxy(rpcClient, Program.Factory.OriginService);

                if (State)
                {
                    await StateTask.Run(fullNode);
                }
                else if (Exit)
                {
                    await ExitTask.Run(fullNode);
                }

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
