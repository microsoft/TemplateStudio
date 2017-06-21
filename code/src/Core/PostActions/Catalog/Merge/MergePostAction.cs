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
using System.Text.RegularExpressions;
using Microsoft.Templates.Core.Gen;

namespace Microsoft.Templates.Core.PostActions.Catalog.Merge
{
    public class MergePostAction : PostAction<MergeConfiguration>
    {
        private const string Suffix = "postaction";
        private const string NewSuffix = "failedpostaction";

        public const string Extension = "_" + Suffix + ".";
        public const string GlobalExtension = "$*_g" + Suffix + ".";
        public const string PostActionIntentExtension = ".md";

        public const string PostactionRegex = @"(\$\S*)?(_" + Suffix + "|_g" + Suffix + @")\.";
        public const string FailedPostactionRegex = @"(\$\S*)?(_" + NewSuffix + "|_g" + NewSuffix + @")(\d)?\.";

        public MergePostAction(MergeConfiguration config) : base(config)
        {
        }

        public override void Execute()
        {
            string originalFilePath = GetFilePath();

            if (!File.Exists(originalFilePath))
            {
                if (_config.FailOnError )
                {
                    throw new FileNotFoundException($"There is no merge target for file '{_config.FilePath}'");
                }
                else
                {
                    AddFileNotFoundGenerationWarning(originalFilePath);
                    File.Delete(_config.FilePath);
                    return;
                }
            }

            var source = File.ReadAllLines(originalFilePath).ToList();
            var merge = File.ReadAllLines(_config.FilePath).ToList();

            IEnumerable<string> result = source.HandleRemovals(merge);
            result = result.Merge(merge.RemoveRemovals(), out string errorLine);

            if (errorLine != string.Empty)
            {
                AddLineNotFoundGenerationWarning(originalFilePath, errorLine);
            }
            else
            {
                File.WriteAllLines(originalFilePath, result);
                // REFRESH PROJECT TO UN-DIRTY IT
                if (Path.GetExtension(_config.FilePath).Equals(".csproj", StringComparison.OrdinalIgnoreCase))
                {
                    Gen.GenContext.ToolBox.Shell.RefreshProject();
                }
            }

            File.Delete(_config.FilePath);
        }

        private void AddFileNotFoundGenerationWarning(string originalFilePath)
        {
            var sourceFileName = originalFilePath.Replace(GenContext.Current.OutputPath + Path.DirectorySeparatorChar, string.Empty);
            var postactionFileName = _config.FilePath.Replace(GenContext.Current.OutputPath + Path.DirectorySeparatorChar, string.Empty);

            var description = $"Could not find merge target for file '{postactionFileName}'. Please merge the content from the postaction file manually.";
            var intent = GetPostActionIntent();
            var failedFileName = GetFailedPostActionFileName();
            GenContext.Current.GenerationWarnings.Add(new GenerationWarning(sourceFileName, failedFileName, _config.FilePath, description, intent));
            File.Copy(_config.FilePath, failedFileName, true);
        }

        private void AddLineNotFoundGenerationWarning(string originalFilePath, string errorLine)
        {
            var sourceFileName = originalFilePath.Replace(GenContext.Current.OutputPath + Path.DirectorySeparatorChar, string.Empty);

            var postactionFileName = _config.FilePath.Replace(GenContext.Current.OutputPath + Path.DirectorySeparatorChar, string.Empty);
            var description = $"Could not find the expected line `{errorLine.Trim()}` in file '{sourceFileName}'. Please merge the content from the postaction file manually.";
            var intent = GetPostActionIntent();
            var failedFileName = GetFailedPostActionFileName();
            GenContext.Current.GenerationWarnings.Add(new GenerationWarning(sourceFileName, failedFileName, _config.FilePath, description, intent));
            File.Copy(_config.FilePath, failedFileName, true);
        }

        private string GetFailedPostActionFileName()
        {
            var newFileName = Path.GetFileNameWithoutExtension(_config.FilePath).Replace(Suffix, NewSuffix);
            var folder = Path.GetDirectoryName(_config.FilePath);
            var extension = Path.GetExtension(_config.FilePath);

            var validator = new List<Validator>()
            {
                new FileExistsValidator(Path.GetDirectoryName(_config.FilePath))
            };

            newFileName = Naming.Infer(newFileName, validator);
            return Path.Combine(folder, newFileName + extension);
        }

        private string GetPostActionIntent()
        {
            var intentFile = _config.FilePath.Replace(Path.GetExtension(_config.FilePath), PostActionIntentExtension);
            if (File.Exists(intentFile))
            {
                return File.ReadAllText(intentFile);
            }
            return string.Empty;
        }

        private string GetFilePath()
        {
            if (Path.GetFileName(_config.FilePath).StartsWith(Extension))
            {
                var extension = Path.GetExtension(_config.FilePath);
                var directory = Path.GetDirectoryName(_config.FilePath);

                return Directory.EnumerateFiles(directory, $"*{extension}").FirstOrDefault(f => !f.Contains(Suffix));
            }
            else
            {
                var path = Regex.Replace(_config.FilePath, PostactionRegex, ".");

                return path;
            }
        }
    }
}
