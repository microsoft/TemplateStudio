// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommandLine;

namespace Localization.Options
{
    public class ExtractOptions
    {
        [Option(
            's',
            "source",
            HelpText = "path to the folder that contains source files for data extraction (it's root project folder).",
            Required = true)]
        public string SourceDirectory { get; set; }

        [Option(
            'd',
            "dest",
            HelpText = "path to the folder in which will be saved all extracted items.",
            Required = true)]
        public string DestinationDirectory { get; set; }

        [Option(
            't',
            "tag",
            MutuallyExclusiveSet = "extractMode",
            HelpText = "indicate the tag name which marks  the commit from where to look for modified files.")]
        public string TagName { get; set; }

        [Option(
            'c',
            "commit",
            MutuallyExclusiveSet = "extractMode",
            HelpText = "commit from where to look for modified files")]
        public string CommitSha { get; set; }

        public ExtractorMode ExtractorMode
        {
            get
            {
                if (!string.IsNullOrEmpty(TagName))
                {
                    return ExtractorMode.TagName;
                }

                if (!string.IsNullOrEmpty(CommitSha))
                {
                    return ExtractorMode.Commit;
                }

                return ExtractorMode.All;
            }
        }
    }
}
