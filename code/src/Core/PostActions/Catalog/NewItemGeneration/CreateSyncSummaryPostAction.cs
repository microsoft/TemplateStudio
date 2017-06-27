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

namespace Microsoft.Templates.Core.PostActions.Catalog
{
    public class CreateSyncSummaryPostAction : PostAction<TempGenerationResult>
    {
        public CreateSyncSummaryPostAction(TempGenerationResult config) : base(config)
        {
        }

        public override void Execute()
        {
            var fileName = Path.Combine(GenContext.Current.OutputPath, Strings.Resources.SyncSummaryFileName);

            var sb = new StringBuilder();

            sb.AppendLine(Strings.Resources.SyncSummaryHeader);
            sb.AppendLine(Strings.Resources.SyncSummaryDescription);

            if (_config.NewFiles.Any())
            {
                sb.AppendLine(Strings.Resources.SyncSummaryNewFiles);
                sb.AppendLine(Strings.Resources.SyncSummaryNewFilesDescription);
                sb.AppendLine();

                foreach (var newFile in _config.NewFiles)
                {
                    var newFilePath = Path.Combine(GenContext.Current.ProjectPath, newFile);

                    sb.AppendLine(GetLinkToProjectFile(newFile, newFilePath));
                }
            }

            if (GenContext.Current.MergeFilesFromProject.Any())
            {
                sb.AppendLine(Strings.Resources.SyncSummaryModifiedFiles);
                sb.AppendLine(Strings.Resources.SyncSummaryModifiedFilesDescription);

                foreach (var mergeFile in GenContext.Current.MergeFilesFromProject)
                {
                    var modifiedFilePath = Path.Combine(GenContext.Current.ProjectPath, mergeFile.Key);

                    sb.AppendLine(string.Format(Strings.Resources.SyncSummaryMergeFile, mergeFile.Key));
                    sb.AppendLine();

                    if (!GenContext.Current.FailedMergePostActions.Any(w => w.FileName == mergeFile.Key))
                    {
                        sb.AppendLine(string.Format(Strings.Resources.SyncSummaryMergeFilePreview, mergeFile.Key, Uri.EscapeUriString(modifiedFilePath)));
                        sb.AppendLine();
                    }
                    else
                    {
                        var failedMergePostActions = GenContext.Current.FailedMergePostActions.Where(w => w.FileName == mergeFile.Key);
                        if (failedMergePostActions.Count() == 1)
                        {
                            sb.AppendLine(string.Format(Strings.Resources.SyncSummaryMergeFileError, failedMergePostActions.First()?.Description));
                        }
                        else
                        {
                            sb.AppendLine(Strings.Resources.SyncSummaryMergeFileErrors);
                            foreach (var failedMergePostAction in failedMergePostActions)
                            {
                                sb.AppendLine($"* {failedMergePostAction.Description}");
                                sb.AppendLine();
                            }
                        }
                    }

                    foreach (var mergeInfo in mergeFile.Value)
                    {
                        if (!string.IsNullOrEmpty(mergeInfo.Intent))
                        {
                            sb.AppendLine(mergeInfo.Intent);
                        }
                        sb.AppendLine();

                        sb.AppendLine($"```{mergeInfo.Format}");
                        sb.AppendLine(mergeInfo.PostActionCode);
                        sb.AppendLine("```");

                        sb.AppendLine();
                    }
                }
            }
            if (_config.ConflictingFiles.Any())
            {
                sb.AppendLine(Strings.Resources.SyncSummaryConflictingFiles);
                sb.AppendLine(Strings.Resources.SyncSummaryConflictingFilesDescription);
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
            return $"* [{fileName}]({Uri.EscapeUriString(filePath)})";
        }

        private static string GetCompareLink(string fileName, string filePath)
        {
            return $"* {Strings.Resources.SyncSummaryTempGenerationFile}: [{fileName}]({fileName}), {Strings.Resources.SyncSummaryProjectFile}: [{fileName}]({Uri.EscapeUriString(filePath)})";
        }
    }
}
