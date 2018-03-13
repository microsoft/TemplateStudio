// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Microsoft.Templates.Core.PostActions.Catalog
{
    public class SearchAndReplacePostAction : PostAction<string>
    {
        private const string Divider = "^^^-searchabove-replacebelow-vvv";
        private const string Suffix = "searchreplace";

        public const string Extension = "_" + Suffix + ".";

        public const string PostactionRegex = @"(\$\S*)?(_" + Suffix + @")\.";

        public SearchAndReplacePostAction(string relatedTemplate, string config)
            : base(relatedTemplate, config)
        {
        }

        internal override void ExecuteInternal()
        {
            string originalFilePath = GetFilePath();

            if (string.IsNullOrEmpty(originalFilePath))
            {
                throw new FileNotFoundException($"There is no merge target for file '{Config}'");
            }

            var source = File.ReadAllLines(originalFilePath).ToList();
            var instructions = File.ReadAllLines(Config).ToList();

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
            result = result.Replace(string.Join(Environment.NewLine, search), string.Join(Environment.NewLine, replace));

            File.WriteAllLines(originalFilePath, result.Split(new[] { Environment.NewLine }, StringSplitOptions.None));
            File.Delete(Config);

            // REFRESH PROJECT TO UN-DIRTY IT
            if (Path.GetExtension(Config).EndsWith("proj", StringComparison.OrdinalIgnoreCase))
            {
                Gen.GenContext.ToolBox.Shell.RefreshProject();
            }
        }

        private string GetFilePath()
        {
            if (Path.GetFileName(Config).StartsWith(Extension, StringComparison.Ordinal))
            {
                var extension = Path.GetExtension(Config);
                var directory = Path.GetDirectoryName(Config);

                return Directory.EnumerateFiles(directory, $"*{extension}").FirstOrDefault(f => !f.Contains(Suffix));
            }
            else
            {
                var path = Regex.Replace(Config, PostactionRegex, ".");

                return File.Exists(path) ? path : string.Empty;
            }
        }
    }
}
