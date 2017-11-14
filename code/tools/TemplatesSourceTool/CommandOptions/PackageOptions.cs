using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace TemplatesSourceTool.CommandOptions
{
    public enum PackageAction
    {
        Info,
        Extract,
        Create
    }

    [Verb("package", HelpText = "Operations with template package file (.mstx)")]
    public class PackageOptions
    {
        [Option('a', "action", Required = true, HelpText = "Defines the action to perform with the package file or directory specified. Values: Info, Extract or Create")]
        public PackageAction Action { get; set; }

        [Option('s', "source", Required = true, HelpText = "Defines the source (file or directory) to perform the action. Info and Extract only works with a templates package file (*.mstx). Create only work with a directory as target")]
        public string Source { get; set; }

        [Option('c', "cert", Required = false, HelpText = "Certificate thumbprint to be used in the package creation")]
        public string CertThumbprint { get; set; }
    }
}
