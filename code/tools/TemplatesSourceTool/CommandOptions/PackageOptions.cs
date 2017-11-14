using CommandLine;
using System;
using System.Collections.Generic;
using System.Text;

namespace TemplatesSourceTool.CommandOptions
{
    [Verb("package", HelpText = "Operations with template package file (.mstx)")]
    public class PackageOptions
    {

        [Option('i', "info", Required = false, HelpText = "Gets info from the package file specified.", SetName = "Out")]
        public string Info { get; set; }

        [Option('x', "extract", Required = false, HelpText = "Extract templates package content to the specified directory.", SetName = "Out")]
        public string Extract { get; set; }

        [Option('c', "create", Required = false, HelpText = "Create a templates package with the content existing in the the specified directory.", SetName = "In")]
        public string Create { get; set; }

    }
}
