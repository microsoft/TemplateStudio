// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using CommandLine;

namespace Localization.Options
{
    [Verb("verify", HelpText = "Verify if exist localizable items for different cultures.")]

    public class VerifyOptions
    {
        [Option(
            's',
            "source",
            HelpText = "path to the folder that contains source files for verify (it's root project folder).",
            Required = true)]
        public string SourceDirectory { get; set; }
    }
}
