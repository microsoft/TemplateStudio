// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using CommandLine;
using CommandLine.Text;

namespace TemplateValidator
{
    public class CommandLineOptions
    {
        [Option('f', "file", HelpText = "Verify a single config file.", SetName = "File", Required = true)]
        public string File { get; set; }

        [Option('d', "directories", HelpText = "Verify all the templates in the defined directories.", SetName = "Directory", Required = true)]
        public IEnumerable<string> Directories { get; set; }

        // Warnings should be used to provide guidance in the output but for issues that are optional to address.
        [Option("nowarn", Default = false, HelpText = "Do not show warnings.")]
        public bool NoWarnings { get; set; }
    }
}
