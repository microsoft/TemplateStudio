// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Threading.Tasks;
using CommandLine.Text;

namespace TemplateValidator
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Task.Run(async () =>
            {
                ////var analyzer = new SimpleJsonAnalyzer();

                ////var sampleTemplate = File.ReadAllText("../../Templates/Pages/Blank/.template.config/template.json");

                ////var results = await analyzer.AnalyzeJsonAsync(sampleTemplate, typeof(VerificationTemplateInfo));

                ////var pathToFolder = "../../Templates";
                ////var pathToJson = "../../Templates/Pages/Blank/.template.config/template.json";

                ////var results = await TemplateJsonVerifier.VerifyTemplatePathAsync(pathToJson);

                var options = new CommandLineOptions();

                if (CommandLine.Parser.Default.ParseArguments(args, options))
                {
                    var appTitle = new HelpText
                    {
                        Heading = HeadingInfo.Default,
                        Copyright = CopyrightInfo.Default,
                        AdditionalNewLineAfterOption = true,
                        AddDashesToOption = true
                    };

                    Console.WriteLine(appTitle);

                    VerifierResult results = null;

                    if (!string.IsNullOrWhiteSpace(options.File))
                    {
                        results = await TemplateJsonVerifier.VerifyTemplatePathAsync(options.File);
                    }
                    else if (options.Directories.Any())
                    {
                        results = TemplateFolderVerifier.VerifyTemplateFolders(!options.NoWarnings, options.Directories.ToArray());
                    }
                    else
                    {
                        Console.WriteLine(options.GetUsage());
                    }

                    if (results != null)
                    {
                        foreach (var result in results.Messages)
                        {
                            Console.WriteLine(result);
                        }
                    }
                }
                else
                {
                    Console.WriteLine(options.GetUsage());
                }
            }).Wait();

            Console.ReadKey(true);
        }
    }
}
