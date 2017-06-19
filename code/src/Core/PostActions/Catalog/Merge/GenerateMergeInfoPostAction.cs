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
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Templates.Core.Gen;

namespace Microsoft.Templates.Core.PostActions.Catalog.Merge
{
    public class GenerateMergeInfoPostAction : PostAction<string>
    {
        private const string Suffix = "postaction";
        private const string NewSuffix = "failedpostaction";

        public const string Extension = "_" + Suffix + ".";
        public const string PostactionRegex = @"(\$\S*)?(_" + Suffix + "|_g" + Suffix + @")\.";

        public const string PostActionIntentExtension = ".md";

        public GenerateMergeInfoPostAction(string config) : base(config)
        {
        }

        public override void Execute()
        {
            var fileName = _config;
            var postAction = File.ReadAllText(_config).GetUserFriendlyPostAction();
            var intent = GetPostActionIntent();
            var sourceFile = GetFilePath();
            var mergeType = GetMergeType();
            var relFilePath = sourceFile.Replace(GenContext.Current.OutputPath + Path.DirectorySeparatorChar, string.Empty);

            if (GenContext.Current.MergeFilesFromProject.ContainsKey(relFilePath))
            {
                var mergeFile = GenContext.Current.MergeFilesFromProject[relFilePath];

                mergeFile.Add(new MergeInfo() { Intent = intent, Format = mergeType, PostActionCode = postAction });
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

        private string GetPostActionIntent()
        {
            var intentFile = _config.Replace(Path.GetExtension(_config), PostActionIntentExtension);
            if (File.Exists(intentFile))
            {
                return File.ReadAllText(intentFile);
            }
            return string.Empty;
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
