// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommandLine;

namespace Localization.Options
{
    [Verb("merge", HelpText = "Merge localizable items to projects and templates.")]

    public class MergeOptions
    {
        [Option(
            's',
            "source",
            HelpText = "path to the folder that contains localized files.",
            Required = true)]
        public string SourceDirectory { get; set; }

        [Option(
            'd',
            "dest",
            HelpText = "path to the folder in which will be merged localized files.",
            Required = true)]
        public string DestinationDirectory { get; set; }
    }
}
