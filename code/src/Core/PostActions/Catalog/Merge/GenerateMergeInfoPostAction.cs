// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.Templates.Core.Gen;

namespace Microsoft.Templates.Core.PostActions.Catalog.Merge
{
    public class GenerateMergeInfoPostAction : PostAction<string>
    {
        private const string Suffix = "postaction";
        private const string NewSuffix = "failedpostaction";

        public const string Extension = "_" + Suffix + ".";
        public const string PostactionRegex = @"(\$\S*)?(_" + Suffix + "|_g" + Suffix + @")\.";

        public GenerateMergeInfoPostAction(string config) : base(config)
        {
        }

        public override void Execute()
        {
            var fileName = _config;
            var postAction = File.ReadAllText(_config).AsUserFriendlyPostAction();
            var sourceFile = GetFilePath();
            var mergeType = GetMergeType();
            var relFilePath = sourceFile.Replace(GenContext.Current.OutputPath + Path.DirectorySeparatorChar, string.Empty);

            if (GenContext.Current.MergeFilesFromProject.ContainsKey(relFilePath))
            {
                var mergeFile = GenContext.Current.MergeFilesFromProject[relFilePath];

                mergeFile.Add(new MergeInfo() { Format = mergeType, PostActionCode = postAction });
            }
        }

        private string GetMergeType()
        {
            switch (Path.GetExtension(_config).ToLowerInvariant())
            {
                case ".cs":
                    return "csharp";
                case ".csproj":
                case ".xaml":
                    return "xml";
                default:
                    return string.Empty;
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
                var path = Regex.Replace(_config, PostactionRegex, ".");

                return path;
            }
        }
    }
}
