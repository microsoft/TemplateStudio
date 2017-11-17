// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;
using CommandLine;

namespace WtsTool.CommandOptions
{
    public enum PackageAction
    {
        Info,
        Extract,
        Create,
        None
    }

    [Verb("package", Hidden = false, HelpText = "Operations with template package file (.mstx)")]
    public class PackageOptions
    {
        [Option('i', "info", Required = true, HelpText = "Gets the information from the package file specified.", SetName = "Info")]
        public string Info { get; set; }

        [Option('x', "extract", Required = true, HelpText = "Extract the contents from the package specified", SetName = "Extract")]
        public string Extract { get; set; }

        [Option('n', "create-new", Required = true, HelpText = "Creates a new templates package with the contents in the folder specified", SetName = "Create")]
        public string CreateNew { get; set; }

        [Option('c', "cert", Required = false, HelpText = "Certificate thumbprint to be used in the package creation", SetName = "Create")]
        public string CertThumbprint { get; set; }

        [Option('f', "out-file", Required = false, HelpText = "Output file name (myfile.mstx) to be used for package creation", SetName = "Create", Default = "Templates.mstx")]
        public string OutFile { get; set; }

        [Option('p', "out-path", Required = false, HelpText = "Output path to be used for extraction", SetName = "Extract", Default = ".")]
        public string OutPath { get; set; }
    }
}
