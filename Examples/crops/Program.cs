using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Diagnostics;

using CommandLine;

namespace crops
{
    static class Program
    {
        private static int ReturnCode = 0;
        static int Main(string[] args)
        {
            var types = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.GetCustomAttribute<VerbAttribute>() != null).ToArray();

            Parser.Default.ParseArguments(args, types)
                  .WithParsed(Run)
                  .WithNotParsed(HandleErrors);

            Console.WriteLine("Exit code {0}", ReturnCode);
            return ReturnCode;
        }

        private static void Run(object obj)
        {
            if (obj is not IVerb verb)
            {
                throw new InvalidOperationException("Something unexpected happened");
            }

            var task = verb.Run();
            var awaiter = task.ConfigureAwait(false).GetAwaiter();
            ReturnCode = awaiter.GetResult();
        }

        static void HandleErrors(IEnumerable<Error> errs)
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
