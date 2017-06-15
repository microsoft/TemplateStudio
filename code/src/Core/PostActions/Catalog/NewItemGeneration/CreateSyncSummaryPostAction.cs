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
    public class CreateSyncSummaryPostAction : PostAction
    {
        public CreateSyncSummaryPostAction() : base()
        {
        }

        public override void Execute()
        {
            var fileName = Path.Combine(GenContext.Current.OutputPath, "GenerationSummary.md");

            var sb = new StringBuilder();

            sb.AppendLine("# Generation summary");
            sb.AppendLine("The following changes have been incorporated in your project");

            sb.AppendLine("## Modified files: ");

            foreach (var mergeFile in GenContext.Current.MergeFilesFromProject)
            {
                var modifiedFilePath = Path.Combine(GenContext.Current.ProjectPath, mergeFile.Key);

                sb.AppendLine($"### Changes in File '{mergeFile.Key}':");
                sb.AppendLine();

                if (File.Exists(modifiedFilePath))
                {
                    sb.AppendLine($"See the final result: [{mergeFile.Key}]({Uri.EscapeUriString(modifiedFilePath)})");
                    sb.AppendLine();
                    sb.AppendLine($"The following changes were applied:");
                }
                else
                {
                    // TODO: Postaction failed, show info
                    sb.AppendLine($"The changes could not be applied, please apply the following changes manually:");
                    sb.AppendLine();
                }
                foreach (var mergeInfo in mergeFile.Value)
                {
                    sb.AppendLine(mergeInfo.Intent);
                    sb.AppendLine();

                    sb.AppendLine($"```{mergeInfo.Format}");
                    sb.AppendLine(mergeInfo.PostActionCode);
                    sb.AppendLine("```");

                    sb.AppendLine();
                }
            }
            if (GenContext.Current.NewFiles.Any())
            {
                sb.AppendLine($"## New files:");
                sb.AppendLine();

                foreach (var newFile in GenContext.Current.NewFiles)
                {
                    var newFilePath = Path.Combine(GenContext.Current.ProjectPath, newFile);

                    sb.AppendLine(GetLinkToProjectFile(newFile, newFilePath));
                }
            }
            if (GenContext.Current.ConflictFiles.Any())
            {
                sb.AppendLine($"## Conflicting files:");
                sb.AppendLine();
                foreach (var conflictFile in GenContext.Current.ConflictFiles)
                {
                    var conflictFilePath = Path.Combine(GenContext.Current.ProjectPath, conflictFile);

                    sb.AppendLine(GetLinkToProjectFile(conflictFile, conflictFilePath));
                }
            }

            File.WriteAllText(fileName, sb.ToString());

            GenContext.Current.FilesToOpen.Add(fileName);

            GenContext.Current.NewFiles.Clear();
            GenContext.Current.ConflictFiles.Clear();
            GenContext.Current.GenerationWarnings.Clear();
            GenContext.Current.MergeFilesFromProject.Clear();
            GenContext.Current.UnchangedFiles.Clear();
        }

        private static string GetLinkToProjectFile(string fileName, string filePath)
        {
            return $"* [{fileName}]({Uri.EscapeUriString(filePath)})";
        }
    }
}
