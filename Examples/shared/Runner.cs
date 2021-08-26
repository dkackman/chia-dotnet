using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

using CommandLine;

namespace chia.dotnet.console
{
    internal static class Runner
    {
        public static int ReturnCode = 0;

        public static void Run(object obj)
        {
            if (obj is not IVerb verb)
            {
                throw new InvalidOperationException("Something unexpected happened");
            }

            var task = verb.Run();
            var awaiter = task.ConfigureAwait(false).GetAwaiter();
            ReturnCode = awaiter.GetResult();
        }

        public static void HandleErrors(IEnumerable<Error> errs)
        {
            if (!errs.Any(x => x is HelpRequestedError or VersionRequestedError or HelpVerbRequestedError))
            {
                ReturnCode = -1;
                foreach (var error in errs)
                {
                    Debug.WriteLine(error.ToString());
                }
            }
        }
    }
}
