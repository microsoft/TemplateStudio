// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using CommandLine;

namespace Localization.Options
{
    public class GenerationOptions
    {
        [Option(
            's',
            "source",
            HelpText = "path to the folder that contains source files for Project Templates(it's root project folder).",
            Required = true)]
        public string SourceDirectory { get; set; }

        [Option(
            'd',
            "dest",
            HelpText = "path to the folder in which will be saved all localized Project Templates (parent for CSharp.UWP.2017.Solution directory).",
            Required = true)]
        public string DestinationDirectory { get; set; }
    }
}
