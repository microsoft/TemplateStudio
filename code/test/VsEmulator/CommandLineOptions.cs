// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommandLine;

namespace Microsoft.Templates.VsEmulator
{
    public class CommandLineOptions
    {
        [Option('c', "culture", Required = true, HelpText = "Specify a culture. e.g. en-US")]
        public string Culture { get; set; }

        [Option('l', "proglang", Required = true, HelpText = "C# or VisualBasic")]
        public string ProgLang { get; set; }

        [Option('n', "projectName", Default = "", Required = false, HelpText = "A random value will be generated if none provided")]
        public string ProjectName { get; set; }

        [Option('u', "ui", Default = "Project", Required = false, HelpText = "Project, Page, or Feature. ")]
        public string UI { get; set; }
    }
}
