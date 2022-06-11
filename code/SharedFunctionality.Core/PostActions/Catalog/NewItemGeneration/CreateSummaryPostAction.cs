// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.PostActions.Catalog.Merge;
using Microsoft.Templates.SharedResources;

namespace Microsoft.Templates.Core.PostActions.Catalog
{
    public class CreateSummaryPostAction : PostAction<TempGenerationResult>
    {
        public CreateSummaryPostAction(TempGenerationResult config)
            : base(config)
        {
        }

        internal override void ExecuteInternal()
        {
            var parentGenerationOutputPath = Directory.GetParent(GenContext.Current.GenerationOutputPath).FullName;
            var fileName = GetFileName(parentGenerationOutputPath);
            if (Config.SyncGeneration)
            {
                var newFiles = BuildNewFilesSection(Resources.SyncSummarySectionNewFiles);

                var modifiedFiles = BuildMergeFileSection(Resources.SyncSummarySectionModifiedFiles, Resources.SyncSummaryTemplateModifiedFile, GenContext.Current.MergeFilesFromProject.Where(f => !GenContext.Current.FailedMergePostActions.Any(m => m.FileName == f.Key)));
                var failedMergeFiles = BuildMergeFileSection(Resources.SyncSummarySectionFailedMergeFiles, Resources.SyncSummaryTemplateFailedMerges, GenContext.Current.MergeFilesFromProject.Where(f => GenContext.Current.FailedMergePostActions.Any(m => m.FileName == f.Key)));
                var conflictingFiles = BuildConflictingFilesSection(Resources.SyncSummarySectionConflictingFiles);

                File.WriteAllText(fileName, string.Format(Resources.SyncSummaryTemplate, parentGenerationOutputPath, newFiles, modifiedFiles, failedMergeFiles, conflictingFiles));
            }
            else
            {
                var newFiles = BuildNewFilesSection(Resources.SyncInstructionsSectionNewFiles);
                var modifiedFiles = BuildMergeFileSection(Resources.SyncInstructionsSectionModifiedFiles, Resources.SyncInstructionsTemplateModifiedFile, GenContext.Current.MergeFilesFromProject);
                var conflictingFiles = BuildConflictingFilesSection(Resources.SyncInstructionsSectionConflictingFiles);
                var unchangedFiles = BuildUnchangedFilesSection(Resources.SyncInstructionsSectionUnchangedFiles);

                File.WriteAllText(fileName, string.Format(Resources.SyncInstructionsTemplate, parentGenerationOutputPath, newFiles, modifiedFiles, conflictingFiles, unchangedFiles));
            }

            GenContext.Current.FilesToOpen.Add(fileName);

            GenContext.Current.FailedMergePostActions.Clear();
            GenContext.Current.MergeFilesFromProject.Clear();
        }

        private string GetFileName(string path)
        {
            if (Config.SyncGeneration)
            {
                return Path.Combine(path, Resources.SyncSummaryFileName);
            }
            else
            {
                return Path.Combine(path, Resources.SyncInstructionsFileName);
            }
        }

        private string BuildMergeFileSection(string sectionTemplate, string modifiedFileTemplate, IEnumerable<KeyValuePair<string, List<MergeInfo>>> mergeFiles)
        {
            if (mergeFiles.Any())
            {
                var sb = new StringBuilder();

                foreach (var mergeFile in mergeFiles)
                {
                    sb.AppendLine(GetMergeFileDescription(mergeFile, modifiedFileTemplate));
                }

                return string.Format(sectionTemplate, sb.ToString());
            }
            else
            {
                return string.Empty;
            }
        }

        private string GetMergeFileDescription(KeyValuePair<string, List<Merge.MergeInfo>> mergeFile, string modifiedFileTemplate)
        {
            var sb = new StringBuilder();

            foreach (var mergeInfo in mergeFile.Value)
            {
                sb.AppendLine($"```{mergeInfo.Format}");
                sb.AppendLine(mergeInfo.PostActionCode);
                sb.AppendLine("```");
            }

            return string.Format(modifiedFileTemplate, mergeFile.Key, sb.ToString(), GetMergeResult(mergeFile));
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

        private string BuildNewFilesSection(string sectionTemplate)
        {
            if (Config.NewFiles.Any())
            {
                return string.Format(sectionTemplate, GetFileList(Config.NewFiles));
            }
            else
            {
                return string.Empty;
            }
        }

        private string BuildUnchangedFilesSection(string sectionTemplate)
        {
            if (Config.UnchangedFiles.Any())
            {
                return string.Format(sectionTemplate, GetFileList(Config.UnchangedFiles));
            }
            else
            {
                return string.Empty;
            }
        }

        private string BuildConflictingFilesSection(string sectionTemplate)
        {
            if (Config.ConflictingFiles.Any())
            {
                var sb = new StringBuilder();
                foreach (var conflictFile in Config.ConflictingFiles)
                {
                    sb.AppendLine(GetCompareLink(conflictFile));
                }

                return string.Format(sectionTemplate, sb.ToString());
            }
            else
            {
                return string.Empty;
            }
        }

        private StringBuilder GetFileList(List<string> files)
        {
            var fileList = new StringBuilder();
            foreach (var file in files)
            {
                fileList.AppendLine($"* {GetLinkToFile(file)}");
            }

            return fileList;
        }

        private string GetLinkToFile(string fileName)
        {
            if (Config.SyncGeneration)
            {
                var filePath = Path.Combine(GetDestinationParent(), fileName);
                return $"[{fileName}]({FormatFilePath(filePath)})";
            }
            else
            {
                return $"[{fileName}]({fileName})";
            }
        }

        private static string FormatFilePath(string filePath)
        {
            return $"about:/{filePath.Replace(" ", "%20").Replace(@"\", "/")}";
        }

        private string GetCompareLink(string fileName)
        {
            var filePath = Path.Combine(GetDestinationParent(), fileName);
            return $"* {Resources.SyncSummaryTempGenerationFile}: [{fileName}]({fileName}), {Resources.SyncSummaryProjectFile}: [{fileName}]({FormatFilePath(filePath)})";
        }

        private string GetDestinationParent() => Directory.GetParent(GenContext.Current.DestinationPath).FullName;
    }
}
