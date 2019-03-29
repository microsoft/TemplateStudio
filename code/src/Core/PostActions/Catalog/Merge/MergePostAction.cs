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

            var originalEncoding = GetEncoding(originalFilePath);

            // Only check encoding on new project, might have changed on right click
            if (GenContext.Current.GenerationOutputPath == GenContext.Current.DestinationPath)
            {
                var otherEncoding = GetEncoding(Config.FilePath);

                if (originalEncoding.EncodingName != otherEncoding.EncodingName
                    || !Enumerable.SequenceEqual(originalEncoding.GetPreamble(), otherEncoding.GetPreamble()))
                {
                    HandleMismatchedEncodings(originalFilePath, Config.FilePath, originalEncoding, otherEncoding);
                    return;
                }
            }

            IEnumerable<string> result = source.Merge(merge, out string errorLine);

            if (errorLine != string.Empty)
            {
                HandleLineNotFound(originalFilePath, errorLine);
                return;
            }

            Fs.EnsureFileEditable(originalFilePath);

            File.WriteAllLines(originalFilePath, result, originalEncoding);

            File.Delete(Config.FilePath);
        }

        protected void HandleMismatchedEncodings(string originalFilePath, string otherFilePath, Encoding originalEncoding, Encoding otherEncoding)
        {
            var relativeFilePath = originalFilePath.GetPathRelativeToGenerationParentPath();
            var otherRelativePath = otherFilePath.GetPathRelativeToGenerationParentPath();

            var errorMessage = string.Format(StringRes.FailedMergePostActionMismatchedEncoding, relativeFilePath, originalEncoding.EncodingName, otherRelativePath, otherEncoding.EncodingName);

            if (Config.FailOnError)
            {
                throw new InvalidDataException(errorMessage);
            }

            HandleFailedMergePostActions(relativeFilePath, MergeFailureType.MismatchedEncoding, MergeConfiguration.Suffix, errorMessage);
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

        protected Encoding GetEncoding(string originalFilePath)
        {
            // Will read the file, and look at the BOM to check the encoding.
            using (var reader = new StreamReader(File.OpenRead(originalFilePath), true))
            {
                var bytes = File.ReadAllBytes(originalFilePath);
                var encoding = reader.CurrentEncoding;

                // The preamble is the first couple of bytes that may be appended to define an encoding.
                var preamble = encoding.GetPreamble();

                // We preserve the read encoding unless there is no BOM, if it is UTF-8 we return the non BOM encoding.
                if (preamble == null || preamble.Length == 0 || preamble.Where((p, i) => p != bytes[i]).Any())
                {
                    if (encoding.EncodingName == Encoding.UTF8.EncodingName)
                    {
                        return new UTF8Encoding(false);
                    }
                }

                return encoding;
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
