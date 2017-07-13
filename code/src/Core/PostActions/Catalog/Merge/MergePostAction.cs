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

using Microsoft.Templates.Core.Resources;

namespace Microsoft.Templates.Core.PostActions.Catalog.Merge
{
    public class MergePostAction : PostAction<MergeConfiguration>
    {
        private const string Suffix = "postaction";
        private const string NewSuffix = "failedpostaction";

        public const string Extension = "_" + Suffix + ".";
        public const string GlobalExtension = "$*_g" + Suffix + ".";

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
                    throw new FileNotFoundException(string.Format(StringRes.MergeFileNotFoundExceptionMessage, _config.FilePath));
                }
                else
                {
                    AddFailedMergePostActionsFileNotFound(originalFilePath);
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
                if (_config.FailOnError)
                {
                    throw new InvalidDataException(string.Format(StringRes.MergeLineNotFoundExceptionMessage, errorLine, originalFilePath));
                }
                else
                {
                    AddFailedMergePostActionsAddLineNotFound(originalFilePath, errorLine);
                }
            }
            else
            {
                Fs.EnsureFileEditable(originalFilePath);
                File.WriteAllLines(originalFilePath, result);

                // REFRESH PROJECT TO UN-DIRTY IT
                if ((Path.GetExtension(_config.FilePath).Equals(".csproj", StringComparison.OrdinalIgnoreCase)
                   || Path.GetExtension(_config.FilePath).Equals(".vbproj", StringComparison.OrdinalIgnoreCase))
                  && (GenContext.Current.OutputPath == GenContext.Current.ProjectPath))
                {
                    Gen.GenContext.ToolBox.Shell.RefreshProject();
                }
            }

            File.Delete(_config.FilePath);
        }

        private void AddFailedMergePostActionsFileNotFound(string originalFilePath)
        {
            var sourceFileName = originalFilePath.Replace(GenContext.Current.OutputPath + Path.DirectorySeparatorChar, string.Empty);
            var postactionFileName = _config.FilePath.Replace(GenContext.Current.OutputPath + Path.DirectorySeparatorChar, string.Empty);

            var description = string.Format(StringRes.FailedMergePostActionFileNotFound, sourceFileName);

            var failedFileName = GetFailedPostActionFileName();
            GenContext.Current.FailedMergePostActions.Add(new FailedMergePostAction(sourceFileName, _config.FilePath, failedFileName, description, MergeFailureType.FileNotFound));
            File.Copy(_config.FilePath, failedFileName, true);
        }

        private void AddFailedMergePostActionsAddLineNotFound(string originalFilePath, string errorLine)
        {
            var sourceFileName = originalFilePath.Replace(GenContext.Current.OutputPath + Path.DirectorySeparatorChar, string.Empty);

            var postactionFileName = _config.FilePath.Replace(GenContext.Current.OutputPath + Path.DirectorySeparatorChar, string.Empty);
            var description = string.Format(StringRes.FailedMergePostActionLineNotFound, errorLine.Trim(), sourceFileName);
            var failedFileName = GetFailedPostActionFileName();
            GenContext.Current.FailedMergePostActions.Add(new FailedMergePostAction(sourceFileName, _config.FilePath, failedFileName, description, MergeFailureType.LineNotFound));
            File.Copy(_config.FilePath, failedFileName, true);
        }

        private string GetFailedPostActionFileName()
        {
            var newFileName = Path.GetFileNameWithoutExtension(_config.FilePath).Replace(Suffix, NewSuffix);
            var folder = Path.GetDirectoryName(_config.FilePath);
            var extension = Path.GetExtension(_config.FilePath);

            var validator = new List<Validator>
            {
                new FileExistsValidator(Path.GetDirectoryName(_config.FilePath))
            };

            newFileName = Naming.Infer(newFileName, validator);
            return Path.Combine(folder, newFileName + extension);
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
