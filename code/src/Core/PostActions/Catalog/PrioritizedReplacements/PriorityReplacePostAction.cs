// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Templates.Core.Diagnostics;

namespace Microsoft.Templates.Core.PostActions.Catalog.PrioritizedReplacements
{
    public class PriorityReplacePostAction : PostAction<string>
    {
        private const string Suffix = "priorityreplace";

        public const string Extension = "_" + Suffix + ".";
        public const string GlobalExtension = "$*_g" + Suffix + ".";
        public const string ReplaceRegex = @"(\$\S*)?(_" + Suffix + "|_g" + Suffix + @")\.";

        public PriorityReplacePostAction(string config) : base(config)
        {
        }

        public override void Execute()
        {
            string originalFilePath = GetFilePath();

            if (string.IsNullOrEmpty(originalFilePath))
            {
                AppHealth.Current.Info.TrackAsync($"No target exists so skipping '{_config}'").FireAndForget();
            }
            else
            {
                var source = File.ReadAllLines(originalFilePath).ToList();
                var replacements = File.ReadAllLines(_config).ToList();

                var result = source.MakePriorityReplacements(replacements);

                File.WriteAllLines(originalFilePath, result);
            }

            File.Delete(_config);

            //REFRESH PROJECT TO UN-DIRTY IT
            if (Path.GetExtension(_config).Equals(".csproj", StringComparison.OrdinalIgnoreCase))
            {
                Gen.GenContext.ToolBox.Shell.RefreshProject();
            }
        }

        private string GetFilePath()
        {
            if (Path.GetFileName(_config).StartsWith(Extension))
            {
                var extension = Path.GetExtension(_config);
                var directory = Path.GetDirectoryName(_config);

                return Directory.EnumerateFiles(directory, $"*{extension}").FirstOrDefault(f => !f.Contains(Suffix));
            }
            else
            {
                var path = Regex.Replace(_config, ReplaceRegex, ".");

                return (File.Exists(path) ? path : String.Empty);
            }
        }
    }
}
