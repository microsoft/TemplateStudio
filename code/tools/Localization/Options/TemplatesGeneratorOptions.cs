// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using CommandLine;

namespace Localization.Options
{
    public class TemplatesGeneratorOptions
    {
        [Option(
            's',
            "source",
            HelpText = "path to the folder that contains templates folder",
            Required = true)]
        public string SourceDirectory { get; set; }
    }
}
