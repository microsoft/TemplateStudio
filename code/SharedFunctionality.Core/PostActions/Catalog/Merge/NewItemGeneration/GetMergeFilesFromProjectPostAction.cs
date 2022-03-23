// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

using Microsoft.Templates.Core.Extensions;
using Microsoft.Templates.Core.Gen;

namespace Microsoft.Templates.Core.PostActions.Catalog.Merge
{
    public class GetMergeFilesFromProjectPostAction : PostAction<string>
    {
        public GetMergeFilesFromProjectPostAction(string relatedTemplate, string config)
            : base(relatedTemplate, config)
        {
        }

        internal override void ExecuteInternal()
        {
            if (Regex.IsMatch(Config, MergeConfiguration.GlobalExtension))
            {
                GetFileFromProject();
            }
            else
            {
                if (!CheckLocalMergeFileAvailable())
                {
                    GetFileFromProject();
                }
            }
        }

        private void GetFileFromProject()
        {
            var filePath = GetMergeFileFromDirectory(Path.GetDirectoryName(Config.GetDestinationPath()));
            var relFilePath = filePath.GetPathRelativeToDestinationParentPath();

            if (!GenContext.Current.MergeFilesFromProject.ContainsKey(relFilePath))
            {
                GenContext.Current.MergeFilesFromProject.Add(relFilePath, new List<MergeInfo>());
                if (File.Exists(filePath))
                {
                    var destFile = filePath.GetGenerationPath();
                    File.Copy(filePath, destFile, true);
                }
            }
        }

        private bool CheckLocalMergeFileAvailable()
        {
            var filePath = GetMergeFileFromDirectory(Path.GetDirectoryName(Config));
            return File.Exists(filePath) ? true : false;
        }

        private string GetMergeFileFromDirectory(string directory)
        {
            var filePath = Path.Combine(directory, Path.GetFileName(Config));
            var path = Regex.Replace(filePath, MergeConfiguration.PostactionAndSearchReplaceRegex, ".");

            return path;
        }
    }
}
