// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Templates.Core.Extensions;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Helpers;
using Microsoft.Templates.Core.Resources;

namespace Microsoft.Templates.Core.PostActions.Catalog.Merge
{
    public class MergePostAction : PostAction<MergeConfiguration>
    {
        public MergePostAction(string relatedTemplate, MergeConfiguration config)
            : base(relatedTemplate, config)
        {
        }

        internal override void ExecuteInternal()
        {
            string originalFilePath = GetFilePath();
            if (!File.Exists(originalFilePath))
            {
                HandleFileNotFound(originalFilePath, MergeConfiguration.Suffix);
                return;
            }

            var source = File.ReadAllLines(originalFilePath).ToList();
            var merge = File.ReadAllLines(Config.FilePath).ToList();

            IEnumerable<string> result = source.Merge(merge, out string errorLine);

            if (errorLine != string.Empty)
            {
                HandleLineNotFound(originalFilePath, errorLine);
                return;
            }

            Fs.EnsureFileEditable(originalFilePath);
            File.WriteAllLines(originalFilePath, result, Encoding.UTF8);

            File.Delete(Config.FilePath);
        }

        public void HandleFailedMergePostActions(string originalFileRelativePath, MergeFailureType mergeFailureType, string suffix, string errorMessage)
        {
            var validator = new List<Validator>
            {
                new FileExistsValidator(Path.GetDirectoryName(Config.FilePath)),
            };

            // Change filename from .postaction to .failedpostaction, .failedpostaction1,...
            var splittedFileName = Path.GetFileName(Config.FilePath).Split('.');
            splittedFileName[0] = Naming.Infer(splittedFileName[0].Replace(suffix, MergeConfiguration.FailedPostactionSuffix), validator);
            var failedFileName = Path.Combine(Path.GetDirectoryName(Config.FilePath), string.Join(".", splittedFileName));

            Fs.SafeMoveFile(Config.FilePath, failedFileName);

            // Add info to context
            GenContext.Current.FailedMergePostActions.Add(new FailedMergePostActionInfo(originalFileRelativePath, Config.FilePath, failedFileName.GetPathRelativeToGenerationParentPath(), failedFileName, errorMessage, mergeFailureType));
        }

        private string GetFilePath()
        {
            // TODO: Remove this when 3.0 is released, only necesary for legacy tests
            if (Path.GetFileName(Config.FilePath).StartsWith(MergeConfiguration.Extension, StringComparison.OrdinalIgnoreCase))
            {
                var extension = Path.GetExtension(Config.FilePath);
                var directory = Path.GetDirectoryName(Config.FilePath);

                return Directory.EnumerateFiles(directory, $"*{extension}").FirstOrDefault(f => !f.Contains(MergeConfiguration.Suffix));
            }
            else
            {
                var path = Regex.Replace(Config.FilePath, MergeConfiguration.PostactionRegex, ".");

                return path;
            }
        }

        protected void HandleFileNotFound(string originalFilePath, string suffix)
        {
            if (Config.FailOnError)
            {
                throw new FileNotFoundException(string.Format(StringRes.MergeFileNotFoundExceptionMessage, Config.FilePath, RelatedTemplate));
            }

            var relativeFilePath = originalFilePath.GetPathRelativeToGenerationParentPath();
            var errorMessage = string.Format(StringRes.FailedMergePostActionFileNotFound, relativeFilePath, RelatedTemplate);

            HandleFailedMergePostActions(relativeFilePath, MergeFailureType.FileNotFound, suffix, errorMessage);
        }

        private void HandleLineNotFound(string originalFilePath, string errorLine)
        {
            if (Config.FailOnError)
            {
                throw new InvalidDataException(string.Format(StringRes.MergeLineNotFoundExceptionMessage, errorLine, originalFilePath, RelatedTemplate));
            }

            var relativeFilePath = originalFilePath.GetPathRelativeToGenerationParentPath();
            var errorMessage = string.Format(StringRes.FailedMergePostActionLineNotFound, errorLine.Trim(), relativeFilePath, RelatedTemplate);

            HandleFailedMergePostActions(relativeFilePath, MergeFailureType.LineNotFound, MergeConfiguration.Suffix, errorMessage);
        }
    }
}
