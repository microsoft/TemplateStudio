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
using System.Threading.Tasks;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Resources;

namespace Microsoft.Templates.Core.PostActions.Catalog
{
    public class CreateSummaryPostAction : PostAction<TempGenerationResult>
    {
        public CreateSummaryPostAction(TempGenerationResult config) : base(config)
        {
        }

        public override void Execute()
        {
            var fileName = GetFileName();

            var newFiles = GetNewFiles();
            var modifiedFiles = GetModifiedFiles();
            var conflictingFiles = GetConflictingFiles();
            var unchangedFiles = GetUnchangedFiles();

            if (_config.SyncGeneration)
            {
                File.WriteAllText(fileName, string.Format(StringRes.SyncSummaryTemplate, GenContext.Current.OutputPath, newFiles, modifiedFiles, conflictingFiles));
            }
            else
            {
                File.WriteAllText(fileName, string.Format(StringRes.SyncInstructionsTemplate, GenContext.Current.OutputPath, newFiles, modifiedFiles, conflictingFiles, unchangedFiles));
            }

            GenContext.Current.FilesToOpen.Add(fileName);

            GenContext.Current.FailedMergePostActions.Clear();
            GenContext.Current.MergeFilesFromProject.Clear();
        }

        private string GetFileName()
        {
            if (_config.SyncGeneration)
            {
                return Path.Combine(GenContext.Current.OutputPath, "testsummary.md");
            }
            else
            {
                return Path.Combine(GenContext.Current.OutputPath, "teststeps.md");
            }
        }

        private string GetModifiedFiles()
        {
            if (GenContext.Current.MergeFilesFromProject.Any())
            {
                var sb = new StringBuilder();

                foreach (var mergeFile in GenContext.Current.MergeFilesFromProject)
                {
                    sb.AppendLine(GetMergeFileDescription(mergeFile));
                }
                return sb.ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        private string GetMergeFileDescription(KeyValuePair<string, List<Merge.MergeInfo>> mergeFile)
        {
            var sb = new StringBuilder();

            foreach (var mergeInfo in mergeFile.Value)
            {
                sb.AppendLine($"```{mergeInfo.Format}");
                sb.AppendLine(mergeInfo.PostActionCode);
                sb.AppendLine("```");

                sb.AppendLine();
            }

            if (_config.SyncGeneration)
            {
                return string.Format(StringRes.SyncSummaryModifiedFilesTemplate, mergeFile.Key, sb.ToString(), GetMergeResult(mergeFile));
            }
            else
            {
                return string.Format(StringRes.SyncInstructionsModifiedFilesTemplate, mergeFile.Key, sb.ToString(), GetMergeResult(mergeFile));
            }
        }

        private string GetMergeResult(KeyValuePair<string, List<Merge.MergeInfo>> mergeFile)
        {
            if (!GenContext.Current.FailedMergePostActions.Any(w => w.FileName == mergeFile.Key))
            {
                return GetLinkToFile(mergeFile.Key);
            }
            else
            {
                var sb = new StringBuilder();
                var failedMergePostActions = GenContext.Current.FailedMergePostActions.Where(w => w.FileName == mergeFile.Key);

                foreach (var failedMergePostAction in failedMergePostActions)
                {
                    sb.AppendLine();
                    sb.AppendLine($"* {failedMergePostAction.Description}");
                }
                return sb.ToString();
            }
        }

        private string GetNewFiles()
        {
            if (_config.NewFiles.Any())
            {
                var newFiles = new StringBuilder();
                foreach (var newFile in _config.NewFiles)
                {
                    newFiles.AppendLine($"* {GetLinkToFile(newFile)}");
                }
                return newFiles.ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        private string GetUnchangedFiles()
        {
            if (_config.UnchangedFiles.Any())
            {
                var sb = new StringBuilder();
                foreach (var unchangedFile in _config.UnchangedFiles)
                {
                    sb.AppendLine($"* {GetLinkToFile(unchangedFile)}");
                }
                return sb.ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        private string GetConflictingFiles()
        {
            if (_config.ConflictingFiles.Any())
            {
                var sb = new StringBuilder();
                foreach (var conflictFile in _config.ConflictingFiles)
                {
                    sb.AppendLine(GetCompareLink(conflictFile));
                }
                return sb.ToString();
            }
            else
            {
                return string.Empty;
            }
        }

        private string GetLinkToFile(string fileName)
        {
            if (_config.SyncGeneration)
            {
                var filePath = Path.Combine(GenContext.Current.ProjectPath, fileName);
                return $"[{fileName}]({FormatFilePath(filePath)})";
            }
            else
            {
                return $"[{fileName}]({fileName})";
            }
        }

        private string GetCompareLink(string fileName)
        {
            var filePath = Path.Combine(GenContext.Current.ProjectPath, fileName);
            return $"* {StringRes.SyncSummaryTempGenerationFile}: [{fileName}]({fileName}), {StringRes.SyncSummaryProjectFile}: [{fileName}]({FormatFilePath(filePath)})";
        }

        private static string FormatFilePath(string filePath)
        {
            return $"about:/{filePath.Replace(" ", "%20").Replace(@"\", "/")}";
        }
    }
}
