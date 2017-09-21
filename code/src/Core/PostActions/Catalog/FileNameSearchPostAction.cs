// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core.PostActions.Catalog.Merge;

namespace Microsoft.Templates.Core.PostActions.Catalog
{
    public class FileNameSearchPostAction : PostAction<string>
    {
        public const string FileNameStart = "$SEARCH";
        public const string FileNamePattern = FileNameStart + "*.*";
        public const string SearchRegex = @"\$SEARCH([0-9]{1})\$";

        public FileNameSearchPostAction(string config, ITemplateInfo template) : base(config)
        {
            var numeral = Regex.Match(Path.GetFileNameWithoutExtension(config), SearchRegex).Groups[1].Value;

            SearchValue = template.GetFileNameSearch(numeral);
        }

        private string SearchValue { get; }

        public override void Execute()
        {
            var dirOfInterest = Path.GetDirectoryName(_config);

            // Getting the extension this way and not using Path.GetExtension() as it treats ".cs" as the extension of "file.xaml.cs" when we need ".xaml.cs"
            var extOfInterest = Path.GetFileName(_config).Substring(Path.GetFileName(_config).IndexOf(".", StringComparison.Ordinal));

            string targetFileName = null;

            foreach (var file in new DirectoryInfo(dirOfInterest).GetFiles($"*{extOfInterest}"))
            {
                if (file.FullName != _config
                 && File.ReadAllText(file.FullName).Contains(SearchValue))
                {
                    targetFileName = file.FullName;
                    break;
                }
            }

            if (!string.IsNullOrWhiteSpace(targetFileName))
            {
                var replacement = Path.GetFileName(targetFileName).Replace(extOfInterest, string.Empty);

                var newFileName = _config.Replace(Path.GetFileName(_config).Replace(extOfInterest, string.Empty), $"{replacement}_postaction");

                File.Move(_config, newFileName);

                if (extOfInterest == ".xaml.cs")
                {
                    // Need to set the class name correctly for merge to work as it won't have been renamed like other files would
                    var targetFileLines = File.ReadAllLines(targetFileName);

                    var classDefinitionLine = string.Empty;

                    foreach (var fileLine in targetFileLines)
                    {
                        if (fileLine.Contains(" class ") && !fileLine.Contains("//"))
                        {
                            classDefinitionLine = fileLine;
                            break;
                        }
                    }

                    var mergefileLines = File.ReadAllLines(newFileName);

                    for (var i = 0; i < mergefileLines.Length; i++)
                    {
                        var mergefileLine = mergefileLines[i];
                        if (mergefileLine.Contains(" class "))
                        {
                            mergefileLines[i] = classDefinitionLine;
                            File.WriteAllLines(newFileName, mergefileLines);
                            break;
                        }
                    }
                }

                var mergeRenamedFileAction = new MergePostAction(new MergeConfiguration(newFileName, true));

                mergeRenamedFileAction.Execute();
            }
        }
    }
}
