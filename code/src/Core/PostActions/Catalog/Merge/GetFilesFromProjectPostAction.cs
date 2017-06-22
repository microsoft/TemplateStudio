// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Templates.Core.Gen;

namespace Microsoft.Templates.Core.PostActions.Catalog.Merge
{
    public class GetFilesFromProjectPostAction : PostAction<string>
    {
        public GetFilesFromProjectPostAction(string config) : base(config)
        {
        }

        public override void Execute()
        {
            if (Regex.IsMatch(_config, MergePostAction.PostactionRegex))
            {
                var filePath = GetMergeFileFromDirectory(Path.GetDirectoryName(_config.Replace(GenContext.Current.OutputPath, GenContext.Current.ProjectPath)));
                var relFilePath = filePath.Replace(GenContext.Current.ProjectPath + Path.DirectorySeparatorChar, string.Empty);

                if (File.Exists(filePath) && !GenContext.Current.MergeFilesFromProject.ContainsKey(relFilePath))
                {
                    GenContext.Current.MergeFilesFromProject.Add(relFilePath, new List<MergeInfo>());

                    var destFile = filePath.Replace(GenContext.Current.ProjectPath, GenContext.Current.OutputPath);
                    File.Copy(filePath, destFile, true);
                }
                else if (!CheckLocalMergeFileAvailable())
                {
                    // Check if file is available locally, if not, add as missing merge file from project
                    GenContext.Current.MergeFilesFromProject.Add(relFilePath, new List<MergeInfo>());
                }
            }
            else
            {
                var relFilePath = _config.Replace(GenContext.Current.OutputPath + Path.DirectorySeparatorChar, string.Empty);
                if (!GenContext.Current.MergeFilesFromProject.ContainsKey(relFilePath))
                {
                    var projFile = _config.Replace(GenContext.Current.OutputPath, GenContext.Current.ProjectPath);
                    if (File.Exists(projFile))
                    {
                        File.Copy(projFile, _config, true);
                    }
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
