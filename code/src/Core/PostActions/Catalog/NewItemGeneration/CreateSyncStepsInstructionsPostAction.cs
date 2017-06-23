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
    public class CreateSyncStepsInstructionsPostAction : PostAction<TempGenerationResult>
    {
        public CreateSyncStepsInstructionsPostAction(TempGenerationResult config) : base(config)
        {
        }

        public override void Execute()
        {
            var fileName = Path.Combine(GenContext.Current.OutputPath, "Steps to include new item generation.md");

            var sb = new StringBuilder();

            sb.AppendLine("# Steps to include new item generation");
            sb.AppendLine("Please follow the indicated steps to include the new item into you project:");
            sb.AppendLine();

            if (_config.NewFiles.Any())
            {
                sb.AppendLine($"## New files:");
                sb.AppendLine("Copy and add the following files to your project:");
                foreach (var newFile in _config.NewFiles)
                {
                    sb.AppendLine(GetLinkToLocalFile(newFile));
                }
                sb.AppendLine();
            }

            if (GenContext.Current.MergeFilesFromProject.Any())
            {
                sb.AppendLine("## Modified files: ");
                sb.AppendLine("To integrate your new item with the existing project apply the following changes: ");
                sb.AppendLine();

                foreach (var mergeFile in GenContext.Current.MergeFilesFromProject)
                {
                    sb.AppendLine($"### Changes required in file '{mergeFile.Key}':");
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

                    if (!GenContext.Current.FailedMergePostActions.Any(w => w.FileName == mergeFile.Key))
                    {
                        sb.AppendLine($"Preview the changes in: [{mergeFile.Key}]({mergeFile.Key})");
                        sb.AppendLine();
                    }
                    else
                    {
                        var failedMergePostActions = GenContext.Current.FailedMergePostActions.Where(w => w.FileName == mergeFile.Key);

                        if (failedMergePostActions.Count() == 1)
                        {
                            sb.AppendLine($"The changes could not be integrated: {failedMergePostActions.First()?.Description}");
                        }
                        else
                        {
                            sb.AppendLine("The changes could not be integrated. The following warnings were generated:");
                            foreach (var failedMergePostAction in failedMergePostActions)
                            {
                                sb.AppendLine($"* {failedMergePostAction.Description}");
                                sb.AppendLine();
                            }
                        }
                    }
                }
            }

            if (_config.ConflictingFiles.Any())
            {
                sb.AppendLine($"## Conflicting files:");
                sb.AppendLine("These files already exist in your project and were also generated as part of the new item.");
                sb.AppendLine();
                sb.AppendLine("Please compare and make sure everything is in the right place.");
                sb.AppendLine();

                foreach (var conflictFile in _config.ConflictingFiles)
                {
                    sb.AppendLine(GetLinkToLocalFile(conflictFile));
                }
            }

            if (_config.UnchangedFiles.Any())
            {
                sb.AppendLine($"## Unchanged files:");
                sb.AppendLine("These files already exist in your project, no action is necessary:");
                foreach (var unchangedFile in _config.UnchangedFiles)
                {
                    sb.AppendLine(GetLinkToLocalFile(unchangedFile));
                }
            }

            File.WriteAllText(fileName, sb.ToString());

            GenContext.Current.FilesToOpen.Add(fileName);

            GenContext.Current.FailedMergePostActions.Clear();
            GenContext.Current.MergeFilesFromProject.Clear();
        }

        private static string GetLinkToLocalFile(string fileName)
        {
            return $"* [{fileName}]({fileName})";
        }
    }
}
