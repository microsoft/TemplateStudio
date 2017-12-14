// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;
using CommandLine;

namespace WtsTool.CommandOptions
{
    public enum PackageTask
    {
        Info,
        Extract,
        Create,
        None,
        Prepare
    }

    [Verb("package-task", Hidden = false, HelpText = "Operations with template package file (.mstx)")]
    public class PackageOptions : CommonOptions
    {
        [Option('i', "info", Required = true, HelpText = "Gets the information from the package file specified.", SetName = "Info")]
        public string Info { get; set; }

        [Option('x', "extract", Required = true, HelpText = "Extract the contents from the package specified", SetName = "Extract")]
        public string Extract { get; set; }

        [Option('n', "create-new", Required = true, HelpText = "Creates a new templates package with the contents in the folder specified in this option.", SetName = "Create")]
        public string CreateNew { get; set; }

        [Option('c', "cert", Required = false, HelpText = "Certificate thumbprint to be used in the package creation", SetName = "Create")]
        public string CertThumbprint { get; set; }

        [Option('f', "out-file", Required = false, HelpText = "Output file name (myfile.mstx) to be used for package creation", SetName = "Create", Default = "Templates.mstx")]
        public string OutFile { get; set; }

        [Option('d', "destination-dir", Required = false, HelpText = "Destionation directory path to be used for extraction", SetName = "Extract", Default = ".")]
        public string DestionationDir { get; set; }

        [Option('p', "prepare-dir", Required = true, HelpText = "Makes a copy of the specified folder applying filters (if any). Overwrite contents if already exists.", SetName = "Prepare")]
        public string PrepareDir { get; set; }

        [Option('e', "exclude", Required = false, HelpText = "Apply the exclusion filters to the prepare directory by excluding the directories with names matching the specified patterns (C# Regular expresions)", Separator = ';', SetName = "Prepare")]
        public IEnumerable<string> Exclusions { get; set; }

        [Option('v', "version", Required = true, HelpText = "Version number (mayor.minor.build.revision) to be prepared.", Separator = ';', SetName = "Prepare")]
        public string Version { get; set; }
    }
}
