// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.IO;
using System.Text.RegularExpressions;

using Microsoft.Templates.Core.Extensions;
using Microsoft.Templates.Core.Gen;

namespace Microsoft.Templates.Core.PostActions.Catalog.Merge
{
    public class GenerateMergeInfoPostAction : PostAction<string>
    {
        private const string UserFriendlyPostActionMacroStartGroup = "Block to be included";

        public const string PostactionRegex = @"(\$\S*)?(_" + MergeConfiguration.Suffix + "|_" + MergeConfiguration.SearchReplaceSuffix + "||_g" + MergeConfiguration.Suffix + @")\.";

        public GenerateMergeInfoPostAction(string relatedTemplate, string config)
            : base(relatedTemplate, config)
        {
        }

        internal override void ExecuteInternal()
        {
            var postAction = File.ReadAllText(Config);
            if (Regex.IsMatch(Config, MergeConfiguration.PostactionRegex))
            {
                postAction = postAction.AsUserFriendlyPostAction();
            }

            var sourceFile = Regex.Replace(Config, PostactionRegex, ".");
            var mergeType = GetMergeType();
            var relFilePath = sourceFile.GetPathRelativeToGenerationParentPath();

            // Ignore postactions that only cleanup code
            if (postAction.Contains(PostactionFormatter.UserFriendlyPostActionMacroStartGroup) || postAction.Contains(SearchAndReplacePostAction.Divider) || postAction.Contains(MergeConfiguration.ResourceDictionaryMatch))
            {
                if (GenContext.Current.MergeFilesFromProject.ContainsKey(relFilePath))
                {
                    var mergeFile = GenContext.Current.MergeFilesFromProject[relFilePath];

                    mergeFile.Add(new MergeInfo { Format = mergeType, PostActionCode = postAction });
                }
            }
        }

        private string GetMergeType()
        {
            switch (Path.GetExtension(Config).ToUpperInvariant())
            {
                case ".CS":
                    return "CSHARP";
                case ".VB":
                    return "VB.NET";
                case ".CSPROJ":
                case ".VBPROJ":
                case ".XAML":
                    return "XML";
                default:
                    return string.Empty;
            }
        }
    }
}
