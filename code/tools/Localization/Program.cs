// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using CommandLine;
using CommandLine.Text;
using Localization.Options;
using Localization.Resources;

namespace Localization
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            var tool = new LocalizationTool();

            var parserResult = Parser.Default.ParseArguments<ExtractOptions, GenerationOptions, MergeOptions, VerifyOptions>(args);

            parserResult
            .WithParsed<ExtractOptions>(options => tool.ExtractLocalizableItems(options))
            .WithParsed<GenerationOptions>(options => tool.GenerateTemplatesItems(options))
            .WithParsed<MergeOptions>(options => tool.MergeLocalizableItems(options))
            .WithParsed<VerifyOptions>(options => tool.VerifyLocalizableItems(options))
            .WithNotParsed(error =>
            {
                var appTitle = new HelpText
                {
                    Heading = HeadingInfo.Default,
                    Copyright = CopyrightInfo.Default,
                    AdditionalNewLineAfterOption = true,
                    AddDashesToOption = true,
                };
            });
        }
    }
}
