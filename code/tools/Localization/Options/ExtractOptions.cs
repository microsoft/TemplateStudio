// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommandLine;

namespace Localization.Options
{
    [Verb("ext", HelpText = "Extract localizable items for different cultures.")]
    public class ExtractOptions
    {
        [Option(
            'o',
            "original",
            HelpText = "path to the folder that contains source files for data extraction (it's root project folder).",
            Required = true)]
        public string OriginalSourceDirectory { get; set; }

        [Option(
            'a',
            "actual",
            HelpText = "path to the folder that contains source files for data extraction (it's root project folder).",
            Required = true)]
        public string ActualSourceDirectory { get; set; }

        [Option(
            'd',
            "dest",
            HelpText = "path to the folder in which will be saved all extracted items.",
            Required = true)]
        public string DestinationDirectory { get; set; }
    }
}
