using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

using chia.dotnet;
using chia.dotnet.console;

namespace rchia.Show
{
    static class StateTask
    {
        public static async Task Run(FullNodeProxy fullNode)
        {
            using var cts = new CancellationTokenSource(5000);
            var state = await fullNode.GetBlockchainState(cts.Token);
            var peakHash = state.Peak is not null ? state.Peak.HeaderHash : "";

            if (state.Sync.Synced)
            {
                Console.WriteLine("Current Blockchain Status: Full Node Synced");
                Console.WriteLine($"Peak: Hash:{peakHash}");
            }
            else if (state.Peak is not null && state.Sync.SyncMode)
            {
                Console.WriteLine($"Current Blockchain Status: Syncing {state.Sync.SyncProgressPercent}.");
                Console.WriteLine($"Peak: Hash:{peakHash}");
            }
            else if (state.Peak is not null)
            {
                Console.WriteLine($"Current Blockchain Status: Not Synced. Peak height: {state.Peak.Height}");
            }
            else
            {
                Console.WriteLine("Searching for an initial chain");
                Console.WriteLine("You may be able to expedite with 'chia show -a host:port' using a known node.");
            }

            Console.WriteLine("");

            if (state.Peak is not null)
            {
                var time = state.Peak.DateTimestamp.HasValue ? state.Peak.DateTimestamp.Value.ToLocalTime().ToString("U") : "unknown";
                Console.WriteLine($"\tTime: {time}\t\tHeight:\t{state.Peak.Height}");
            }

            Console.WriteLine("");
            Console.WriteLine($"Estimated network space: {state.Space.ToBytesString()}");
            Console.WriteLine($"Current difficulty: {state.Difficulty}");
            Console.WriteLine($"Current VDF sub_slot_iters: {state.SubSlotIters}");

            var totalIters = state.Peak is not null ? state.Peak.TotalIters : 0;
            Console.WriteLine($"Total iterations since the start of the blockchain: {totalIters}");
            Console.WriteLine("");
            Console.WriteLine("  Height: | Hash:");

            if (state.Peak is not null)
            {
                var blocks = new List<BlockRecord>();

                var block = await fullNode.GetBlockRecord(state.Peak.HeaderHash, cts.Token);
                while (block is not null && blocks.Count < 10 && block.Height > 0)
                {
                    using var cts1 = new CancellationTokenSource(1000);
                    blocks.Add(block);
                    block = await fullNode.GetBlockRecord(block.PrevHash, cts.Token);
                }

                foreach (var b in blocks)
                {
                    Console.WriteLine($"   {b.Height} | {b.HeaderHash}");
                }
            }

        }
    }
}
