// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using CommandLine;
using Localization.Options;
using Localization.Resources;

namespace Localization
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            var options = new CommandLineOptions();
            var parser = new Parser(s => s.MutuallyExclusive = true);
            string verb = null;
            object subOptions = null;

            if (args?.Any() == false || !parser.ParseArguments(args, options, (v, o) =>
            {
                verb = v;
                subOptions = o;
            }))
            {
                ShowHelp(verb, args);

                // TO-DO: Use options.GetUsage(verb) to auto-generate help
                // Console.WriteLine(options.GetUsage(verb));
                ExitWithError();
            }

            ProcessCommand(verb, subOptions);
        }

        private static void ShowHelp(string verb, string[] args)
        {
            if (!string.IsNullOrEmpty(verb) && verb.ToUpperInvariant() == "HELP" && args?.Count() > 1 && args[1] != null)
            {
                PrintHelp(args[1]);
            }
            else
            {
                PrintHelp(verb);
            }
        }

        private static void ProcessCommand(string verb, object options)
        {
            try
            {
                var tool = new LocalizationTool();

                switch (verb)
                {
                    case "ext":
                        tool.ExtractLocalizableItems(options as ExtractOptions);
                        break;
                    case "gen":
                        tool.GenerateTemplatesItems(options as GenerationOptions);
                        break;
                    case "merge":
                        tool.MergeLocalizableItems(options as MergeOptions);
                        break;
                    case "verify":
                        var result = tool.VerifyLocalizableItems(options as VerifyOptions);
                        if (!result)
                        {
                            ExitWithError();
                        }

                        break;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error executing command {verb}:");
                Console.Error.WriteLine(ex.ToString());
                ExitWithError();
            }
        }

        private static void PrintHelp(string verb)
        {
            Console.WriteLine();

            if (string.IsNullOrEmpty(verb))
            {
                Console.WriteLine(HelpMessages.Info);
                return;
            }

            switch (verb.ToUpperInvariant())
            {
                case "EXT":
                    Console.WriteLine(HelpMessages.ExtCommand);
                    break;
                case "GEN":
                    Console.WriteLine(HelpMessages.GenCommand);
                    break;
                case "MERGE":
                    Console.WriteLine(HelpMessages.MergeCommand);
                    break;
                case "VERIFY":
                    Console.WriteLine(HelpMessages.VerifyCommand);
                    break;
                case "HELP":
                    Console.WriteLine(HelpMessages.HelpCommand);
                    break;
                default:
                    Console.WriteLine(HelpMessages.UnknownCommand);
                    break;
            }
        }

        private static void ExitWithError()
        {
            Environment.Exit(Parser.DefaultExitCodeFail);
        }
    }
}
