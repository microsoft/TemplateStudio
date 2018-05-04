// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Resources;

namespace Microsoft.Templates.Core.PostActions.Catalog.Merge
{
    public class SearchAndReplacePostAction : PostAction<MergeConfiguration>
    {
        private const string Divider = "^^^-searchabove-replacebelow-vvv";

        public const string PostactionRegex = @"(\$\S*)?(_" + MergeConfiguration.SearchReplaceSuffix + @")\.";

        public SearchAndReplacePostAction(string relatedTemplate, MergeConfiguration config)
            : base(relatedTemplate, config)
        {
        }

        internal override void ExecuteInternal()
        {
            string originalFilePath = GetFilePath();

            if (!File.Exists(originalFilePath))
            {
                if (Config.FailOnError)
                {
                    throw new FileNotFoundException(string.Format(StringRes.MergeFileNotFoundExceptionMessage, originalFilePath, RelatedTemplate));
                }
                else
                {
                    AddFailedMergePostActionsFileNotFound(originalFilePath);
                    File.Delete(Config.FilePath);
                    return;
                }
            }

            var source = File.ReadAllLines(originalFilePath).ToList();
            var instructions = File.ReadAllLines(Config.FilePath).ToList();

            var search = new List<string>();
            var replace = new List<string>();

            var foundDivider = false;

            foreach (var instruction in instructions)
            {
                if (instruction == Divider)
                {
                    foundDivider = true;
                }
                else
                {
                    if (foundDivider)
                    {
                        replace.Add(instruction);
                    }
                    else
                    {
                        search.Add(instruction);
                    }
                }
            }

            var result = string.Join(Environment.NewLine, source);
            result = result.Replace(string.Join(Environment.NewLine, search), string.Join(Environment.NewLine, replace));

            File.WriteAllLines(originalFilePath, result.Split(new[] { Environment.NewLine }, StringSplitOptions.None));
            File.Delete(Config.FilePath);

            // REFRESH PROJECT TO UN-DIRTY IT
            if (Path.GetExtension(Config.FilePath).EndsWith("proj", StringComparison.OrdinalIgnoreCase))
            {
                GenContext.ToolBox.Shell.RefreshProject();
            }
        }

        private string GetFilePath()
        {
            if (Path.GetFileName(Config.FilePath).StartsWith(MergeConfiguration.SearchReplaceExtension, StringComparison.Ordinal))
            {
                var extension = Path.GetExtension(Config.FilePath);
                var directory = Path.GetDirectoryName(Config.FilePath);

                return Directory.EnumerateFiles(directory, $"*{extension}").FirstOrDefault(f => !f.Contains(MergeConfiguration.SearchReplaceSuffix));
            }
            else
            {
                var path = Regex.Replace(Config.FilePath, PostactionRegex, ".");

                return path;
            }
        }

        private void AddFailedMergePostActionsFileNotFound(string originalFilePath)
        {
            var description = string.Format(StringRes.FailedMergePostActionFileNotFound, GetRelativePath(originalFilePath), RelatedTemplate);
            AddFailedMergePostActions(originalFilePath, MergeFailureType.FileNotFound, description);
        }

        protected void AddFailedMergePostActions(string originalFilePath, MergeFailureType mergeFailureType, string description)
        {
            var sourceFileName = GetRelativePath(originalFilePath);
            var postactionFileName = GetRelativePath(Config.FilePath);

            var failedFileName = GetFailedPostActionFileName();
            GenContext.Current.FailedMergePostActions.Add(new FailedMergePostActionInfo(sourceFileName, Config.FilePath, GetRelativePath(failedFileName), description, mergeFailureType));
            File.Copy(Config.FilePath, failedFileName, true);
        }

        protected string GetRelativePath(string path)
        {
            if (GenContext.Current.OutputPath == GenContext.Current.TempGenerationPath)
            {
                return path.Replace(GenContext.Current.OutputPath + Path.DirectorySeparatorChar, string.Empty);
            }
            else
            {
                return path.Replace(Directory.GetParent(GenContext.Current.OutputPath).FullName + Path.DirectorySeparatorChar, string.Empty);
            }
        }

        private string GetFailedPostActionFileName()
        {
            var newFileName = Path.GetFileNameWithoutExtension(Config.FilePath).Replace(MergeConfiguration.SearchReplaceSuffix, MergeConfiguration.NewSuffix);
            var folder = Path.GetDirectoryName(Config.FilePath);
            var extension = Path.GetExtension(Config.FilePath);

            var validator = new List<Validator>
            {
                new FileExistsValidator(Path.GetDirectoryName(Config.FilePath))
            };

            newFileName = Naming.Infer(newFileName, validator);
            return Path.Combine(folder, newFileName + extension);
        }
    }
}
