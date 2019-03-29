// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.TemplateEngine.Edge.Template;
using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Helpers;
using Microsoft.Templates.Core.PostActions;
using Microsoft.Templates.Core.Resources;
using Microsoft.Templates.Core.Templates;

namespace Microsoft.Templates.Core.Gen
{
    public abstract class GenController
    {
        public PostActionFactory PostactionFactory { get; internal set; }

        internal async Task<Dictionary<string, TemplateCreationResult>> GenerateItemsAsync(IEnumerable<GenInfo> genItems, bool isTempGeneration)
        {
            var genResults = new Dictionary<string, TemplateCreationResult>();

            var chrono = Stopwatch.StartNew();
            foreach (var genInfo in genItems)
            {
                if (genInfo.Template == null)
                {
                    continue;
                }

                var statusText = GetStatusText(genInfo);

                if (!string.IsNullOrEmpty(statusText))
                {
                    GenContext.ToolBox.Shell.ShowStatusBarMessage(statusText);
                }

                AppHealth.Current.Info.TrackAsync($"Generating the template {genInfo.Template.Name} to {genInfo.GenerationPath}.").FireAndForget();

                var result = await CodeGen.Instance.Creator.InstantiateAsync(genInfo.Template, genInfo.Name, null, genInfo.GenerationPath, genInfo.Parameters, false, false, null);

                genResults.Add($"{genInfo.Template.Identity}_{genInfo.Name}", result);

                if (result.Status != CreationResultStatus.Success)
                {
                    throw new GenException(genInfo.Name, genInfo.Template.Name, result.Message);
                }

                ReplaceParamsInFilePath(genInfo.GenerationPath, genInfo.Parameters);

                ExecutePostActions(genInfo, result);
            }

            chrono.Stop();
            CalculateGenerationTime(chrono.Elapsed.TotalSeconds);

            ExecuteGlobalPostActions();

            return genResults;
        }

        private void ReplaceParamsInFilePath(string generationPath, Dictionary<string, string> genParameters)
        {
            var parameterReplacements = new FileRenameParameterReplacements(genParameters);

            var filesToMove = Directory.EnumerateFiles(generationPath, "*", SearchOption.AllDirectories)
                .ToList()
                .Where(file => parameterReplacements.FileRenameParams.Any(param => file.Contains(param.Key)));

            if (filesToMove != null && filesToMove.Count() > 0)
            {
                foreach (var file in filesToMove)
                {
                    var newPath = parameterReplacements.ReplaceInPath(file);

                    Fs.EnsureFolder(Directory.GetParent(newPath).FullName);
                    Fs.SafeMoveFile(file, newPath);
                }
            }

            var directoriesToDelete = Directory.EnumerateDirectories(generationPath, "*", SearchOption.AllDirectories)
                .ToList()
                .Where(file => parameterReplacements.FileRenameParams.Any(param => file.Contains(param.Key)));

            if (directoriesToDelete != null && directoriesToDelete.Count() > 0)
            {
                foreach (var d in directoriesToDelete)
                {
                    Fs.SafeDeleteDirectory(d, false);
                }
            }
        }

        private static void CalculateGenerationTime(double totalTime)
        {
            var generationTime = totalTime;
            if (GenContext.Current.ProjectMetrics.ContainsKey(ProjectMetricsEnum.AddProjectToSolution))
            {
                generationTime = totalTime - GenContext.Current.ProjectMetrics[ProjectMetricsEnum.AddProjectToSolution];
            }

            GenContext.Current.ProjectMetrics[ProjectMetricsEnum.Generation] = generationTime;
        }

        internal void ExecutePostActions(GenInfo genInfo, TemplateCreationResult generationResult)
        {
            // Get post actions from template
            var postActions = PostactionFactory.FindPostActions(genInfo, generationResult);

            foreach (var postAction in postActions)
            {
                postAction.Execute();
            }
        }

        internal void ExecuteGlobalPostActions()
        {
            var postActions = PostactionFactory.FindGlobalPostActions();

            foreach (var postAction in postActions)
            {
                postAction.Execute();
            }
        }

        internal string GetStatusText(GenInfo genInfo)
        {
            switch (genInfo.Template.GetTemplateType())
            {
                case TemplateType.Project:
                    return string.Format(StringRes.StatusBarGeneratingProjectMessage, genInfo.Name);
                case TemplateType.Page:
                    return string.Format(StringRes.StatusBarGeneratingPageMessage, $"{genInfo.Name} ({genInfo.Template.Name})");
                case TemplateType.Feature:
                    return string.Format(StringRes.StatusBarGeneratingFeatureMessage, $"{genInfo.Name} ({genInfo.Template.Name})");
                default:
                    return null;
            }
        }

        internal static void VerifyGenContextPaths()
        {
            if (string.IsNullOrEmpty(GenContext.Current.GenerationOutputPath))
            {
                throw new ArgumentNullException(nameof(GenContext.Current.GenerationOutputPath));
            }

            if (string.IsNullOrEmpty(GenContext.Current.DestinationPath))
            {
                throw new ArgumentNullException(nameof(GenContext.Current.DestinationPath));
            }
        }
    }
}
