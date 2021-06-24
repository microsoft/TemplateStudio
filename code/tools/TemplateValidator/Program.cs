// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;

namespace TemplateValidator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var parserResult = Parser.Default.ParseArguments<CommandLineOptions>(args);
            parserResult
            .WithParsed<CommandLineOptions>(options => RunCommand(options))
            .WithNotParsed(error => GetHelpText());
        }

        private static string GetHelpText()
        {
            return new HelpText
            {
                Heading = HeadingInfo.Default,
                Copyright = CopyrightInfo.Default,
                AdditionalNewLineAfterOption = true,
                AddDashesToOption = true,
            };
        }

        private static int RunCommand(CommandLineOptions options)
        {
            Console.WriteLine(GetHelpText());

            VerifierResult results = null;

            if (!string.IsNullOrWhiteSpace(options.File))
            {
                Console.WriteLine(options.File);

                results = TemplateJsonVerifier.VerifyTemplatePathAsync(options.File).Result;
            }
            else if (options.Directories.Any())
            {
                foreach (var directory in options.Directories)
                {
                    Console.WriteLine(directory);
                }

                results = TemplateFolderVerifier.VerifyTemplateFolders(!options.NoWarnings, options.Directories);
            }

            if (results != null)
            {
                foreach (var result in results.Messages)
                {
                    Console.WriteLine(result);
                }
            }

            return 0;
        }
    }
}
