// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.Templates.Core.Extensions;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Resources;

namespace Microsoft.Templates.Core.PostActions.Catalog.Merge
{
    public class SearchAndReplacePostAction : MergePostAction
    {
        public const string Divider = "^^^-searchabove-replacebelow-vvv";

        public const string PostactionRegex = @"(\$\S*)?(_" + MergeConfiguration.SearchReplaceSuffix + @")\.\d?\.?";

        public SearchAndReplacePostAction(string relatedTemplate, MergeConfiguration config)
            : base(relatedTemplate, config)
        {
        }

        internal override void ExecuteInternal()
        {
            string originalFilePath = Regex.Replace(Config.FilePath, PostactionRegex, ".");

            if (!File.Exists(originalFilePath))
            {
                HandleFileNotFound(originalFilePath, MergeConfiguration.SearchReplaceSuffix);
                return;
            }

            var source = File.ReadAllLines(originalFilePath).ToList();
            var instructions = File.ReadAllLines(Config.FilePath).ToList();

            var originalEncoding = GetEncoding(originalFilePath);

            // Only check encoding on new project, might have changed on right click
            if (GenContext.Current.GenerationOutputPath == GenContext.Current.DestinationPath)
            {
                var otherEncoding = GetEncoding(Config.FilePath);

                if (originalEncoding.EncodingName != otherEncoding.EncodingName
                    || !Enumerable.SequenceEqual(originalEncoding.GetPreamble(), otherEncoding.GetPreamble()))
                {
                    HandleMismatchedEncodings(originalFilePath, Config.FilePath, originalEncoding, otherEncoding);
                    return;
                }
            }

            var search = new List<string>();
            var replace = new List<string>();

            var foundDivider = false;

            foreach (var instruction in instructions)
            {
                if (instruction == Divider)
                {
                    foundDivider = true;
                }
                else
                {
                    if (foundDivider)
                    {
                        replace.Add(instruction);
                    }
                    else
                    {
                        search.Add(instruction);
                    }
                }
            }

            var result = string.Join(Environment.NewLine, source);
            if (!result.Contains(search.First().Trim()) && !result.Contains(replace.First().Trim()))
            {
                HandleLineNotFound(originalFilePath, search.First());
            }

            result = result.Replace(string.Join(Environment.NewLine, search), string.Join(Environment.NewLine, replace));

            File.WriteAllLines(originalFilePath, result.Split(new[] { Environment.NewLine }, StringSplitOptions.None), originalEncoding);
            File.Delete(Config.FilePath);
        }
    }
}
