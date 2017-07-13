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
using Microsoft.Templates.Core.PostActions.Catalog.Merge;
using Microsoft.Templates.Core.Resources;

namespace Microsoft.Templates.Core.PostActions.Catalog
{
    public class CreateSyncSummaryPostAction : PostAction<TempGenerationResult>
    {
        public CreateSyncSummaryPostAction(TempGenerationResult config) : base(config)
        {
        }

        public override void Execute()
        {
            var fileName = Path.Combine(GenContext.Current.OutputPath, StringRes.SyncSummaryFileName);

            var sb = new StringBuilder();

            sb.AppendLine(StringRes.MarkdownHeader);
            sb.AppendLine();
            sb.AppendLine(StringRes.SyncSummaryHeader);
            sb.AppendLine(StringRes.SyncSummaryDescription);
            sb.AppendLine();
            sb.AppendLine(string.Format(StringRes.SyncSummaryTempFolder, GenContext.Current.OutputPath));
            sb.AppendLine();

            if (_config.NewFiles.Any())
            {
                sb.AppendLine(StringRes.SyncSummaryNewFiles);
                sb.AppendLine(StringRes.SyncSummaryNewFilesDescription);

                foreach (var newFile in _config.NewFiles)
                {
                    var newFilePath = Path.Combine(GenContext.Current.ProjectPath, newFile);

                    sb.AppendLine(GetLinkToProjectFile(newFile, newFilePath));
                }
                sb.AppendLine();
            }

            if (_config.ModifiedFiles.Any())
            {
                sb.AppendLine(StringRes.SyncSummaryModifiedFiles);
                sb.AppendLine(StringRes.SyncSummaryModifiedFilesDescription);
                sb.AppendLine();

                foreach (var file in _config.ModifiedFiles)
                {
                    var mergeFile = GenContext.Current.MergeFilesFromProject.FirstOrDefault(m => m.Key == file);

                    if (mergeFile.Value != null)
                    {
                        var modifiedFilePath = Path.Combine(GenContext.Current.ProjectPath, mergeFile.Key);

                        sb.AppendLine(string.Format(StringRes.SyncSummaryMergeFile, mergeFile.Key));

                        if (!GenContext.Current.FailedMergePostActions.Any(w => w.FileName == mergeFile.Key))
                        {
                            sb.AppendLine(string.Format(StringRes.SyncSummaryMergeFilePreview, mergeFile.Key, FormatFilePath(modifiedFilePath)));
                            sb.AppendLine();

                            foreach (var mergeInfo in mergeFile.Value)
                            {
                                sb.AppendLine($"```{mergeInfo.Format}");
                                sb.AppendLine(mergeInfo.PostActionCode);
                                sb.AppendLine("```");

                                sb.AppendLine();
                            }
                        }
                    }
                }
            }

            var failedMergeFiles = GenContext.Current.MergeFilesFromProject.Where(f => GenContext.Current.FailedMergePostActions.Any(m => m.FileName == f.Key));
            if (failedMergeFiles.Any())
            {
                sb.AppendLine(StringRes.SyncSummaryFailedMerges);
                sb.AppendLine(StringRes.SyncSummaryFailedMergesDescription);
                sb.AppendLine();

                foreach (var failedMergeFile in failedMergeFiles)
                {
                    sb.AppendLine(string.Format(StringRes.SyncSummaryMergeFile, failedMergeFile.Key));

                    var failedMergePostActions = GenContext.Current.FailedMergePostActions.Where(w => w.FileName == failedMergeFile.Key);

                    sb.AppendLine(StringRes.SyncSummaryMergeFileError);
                    foreach (var failedMergePostAction in failedMergePostActions)
                    {
                        sb.AppendLine($"* {failedMergePostAction.Description}");
                        sb.AppendLine();
                    }

                    foreach (var mergeInfo in failedMergeFile.Value)
                    {
                        sb.AppendLine($"```{mergeInfo.Format}");
                        sb.AppendLine(mergeInfo.PostActionCode);
                        sb.AppendLine("```");

                        sb.AppendLine();
                    }
                }
            }
            if (_config.ConflictingFiles.Any())
            {
                sb.AppendLine(StringRes.SyncSummaryConflictingFiles);
                sb.AppendLine(StringRes.SyncSummaryConflictingFilesDescription);
                sb.AppendLine();
                foreach (var conflictFile in _config.ConflictingFiles)
                {
                    var conflictFilePathProject = Path.Combine(GenContext.Current.ProjectPath, conflictFile);

                    sb.AppendLine(GetCompareLink(conflictFile, conflictFilePathProject));
                }
            }

            File.WriteAllText(fileName, sb.ToString());

            GenContext.Current.FilesToOpen.Add(fileName);

            GenContext.Current.FailedMergePostActions.Clear();
            GenContext.Current.MergeFilesFromProject.Clear();
        }

        private static string GetLinkToProjectFile(string fileName, string filePath)
        {
            return $"* [{fileName}]({FormatFilePath(filePath)})";
        }

        private static string GetCompareLink(string fileName, string filePath)
        {
            return $"* {StringRes.SyncSummaryTempGenerationFile}: [{fileName}]({fileName}), {StringRes.SyncSummaryProjectFile}: [{fileName}]({FormatFilePath(filePath)})";
        }

        private static string FormatFilePath(string filePath)
        {
            return $"about:/{filePath.Replace(" ", "%20").Replace(@"\", "/")}";
        }
    }
}
