// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Templates.Core.Gen;

namespace Microsoft.Templates.Core.PostActions.Catalog.Merge
{
    public class GetMergeFilesFromProjectPostAction : PostAction<string>
    {
        public GetMergeFilesFromProjectPostAction(string config) : base(config)
        {
        }

        public override void Execute()
        {
            if (Regex.IsMatch(_config, MergePostAction.GlobalExtension))
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
            var filePath = GetMergeFileFromDirectory(Path.GetDirectoryName(_config.Replace(GenContext.Current.OutputPath, GenContext.Current.ProjectPath)));
            var relFilePath = filePath.Replace(GenContext.Current.ProjectPath + Path.DirectorySeparatorChar, string.Empty);

            if (!GenContext.Current.MergeFilesFromProject.ContainsKey(relFilePath))
            {
                GenContext.Current.MergeFilesFromProject.Add(relFilePath, new List<MergeInfo>());
                if (File.Exists(filePath))
                {
                    var destFile = filePath.Replace(GenContext.Current.ProjectPath, GenContext.Current.OutputPath);
                    File.Copy(filePath, destFile, true);
                }
            }
        }

        private bool CheckLocalMergeFileAvailable()
        {
            var filePath = GetMergeFileFromDirectory(Path.GetDirectoryName(_config));
            return File.Exists(filePath) ? true : false;
        }

        private string GetMergeFileFromDirectory(string directory)
        {
            if (Path.GetFileName(_config).StartsWith(MergePostAction.Extension))
            {
                var extension = Path.GetExtension(_config);

                return Directory.EnumerateFiles(directory, $"*{extension}").FirstOrDefault(f => !Regex.IsMatch(f, MergePostAction.PostactionRegex));
            }
            else
            {
                var filePath = Path.Combine(directory, Path.GetFileName(_config));
                var path = Regex.Replace(filePath, MergePostAction.PostactionRegex, ".");

                return path;
            }
        }
    }
}
