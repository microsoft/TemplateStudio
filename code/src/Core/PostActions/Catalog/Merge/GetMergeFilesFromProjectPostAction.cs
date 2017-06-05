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

using Microsoft.Templates.Core.Gen;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Microsoft.Templates.Core.PostActions.Catalog.Merge
{

    public class GetMergeFilesFromProjectPostAction : PostAction<string>
    {
        private const string Suffix = "postaction";

        public const string Extension = "_" + Suffix + ".";
        public const string GlobalExtension = "$*_g" + Suffix + ".";
        public const string PostactionRegex = @"(\$\S*)?(_" + Suffix + "|_g" + Suffix + @")\.(?!md$)";

        public GetMergeFilesFromProjectPostAction(string config) : base(config)
        {
        }

        public override void Execute()
        {
            var filePath = String.Empty;
            var isGlobalMergePostAction = _config.Contains(GlobalExtension);

            if (isGlobalMergePostAction)
            {
                filePath = GetFileFromProject();
                if (string.IsNullOrEmpty(filePath))
                {
                    filePath = GetFilePath();
                }
            }
            else
            {
                filePath = GetFilePath();

                if (string.IsNullOrEmpty(filePath))
                {
                    filePath = GetFileFromProject();
                }
            }

            if (string.IsNullOrEmpty(filePath))
            {
                //TODO: Handle this
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

                return (File.Exists(path) ? path : String.Empty);
            }
        }

        private string GetFileFromProject()
        {
            var file = string.Empty;
            if (Path.GetFileName(_config).StartsWith(Extension))
            {
                var extension = Path.GetExtension(_config);
                var directory = GenContext.Current.ProjectPath;

                file = Directory.EnumerateFiles(directory, $"*{extension}").FirstOrDefault(f => !f.Contains(Suffix));
            }
            else
            {
                file = Regex.Replace(_config, PostactionRegex, ".").Replace(GenContext.Current.OutputPath, GenContext.Current.ProjectPath);
            }

            if (File.Exists(file))
            {
                var destFile = file.Replace(GenContext.Current.ProjectPath, GenContext.Current.OutputPath);
                File.Copy(file, destFile);
                GenContext.Current.MergeFilesFromProject.Add(file);
                return destFile;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
