// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.TemplateEngine.Edge.Template;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.PostActions;
using Microsoft.Templates.UI.Resources;
using Microsoft.Templates.UI.V2ViewModels.Common;

namespace Microsoft.Templates.UI
{
    public abstract class GenController
    {
        public PostActionFactory PostactionFactory { get; internal set; }

        internal async Task<Dictionary<string, TemplateCreationResult>> GenerateItemsAsync(IEnumerable<GenInfo> genItems)
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

                AppHealth.Current.Info.TrackAsync($"Generating the template {genInfo.Template.Name} to {GenContext.Current.OutputPath}.").FireAndForget();

                var result = await CodeGen.Instance.Creator.InstantiateAsync(genInfo.Template, genInfo.Name, null, GenContext.Current.OutputPath, genInfo.Parameters, false, false, null);

                genResults.Add($"{genInfo.Template.Identity}_{genInfo.Name}", result);

                if (result.Status != CreationResultStatus.Success)
                {
                    throw new GenException(genInfo.Name, genInfo.Template.Name, result.Message);
                }

                ExecutePostActions(genInfo, result);
            }

            chrono.Stop();
            CalculateGenerationTime(chrono.Elapsed.TotalSeconds);

            ExecuteGlobalPostActions();

            return genResults;
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
                    return string.Format(StringRes.GeneratingProjectMessage, genInfo.Name);
                case TemplateType.Page:
                    return string.Format(StringRes.GeneratingPageMessage, $"{genInfo.Name} ({genInfo.Template.Name})");
                case TemplateType.Feature:
                    return string.Format(StringRes.GeneratingFeatureMessage, $"{genInfo.Name} ({genInfo.Template.Name})");
                default:
                    return null;
            }
        }

        internal void ShowError(Exception ex, UserSelection userSelection = null)
        {
            AppHealth.Current.Error.TrackAsync(ex.ToString()).FireAndForget();
            AppHealth.Current.Exception.TrackAsync(ex, userSelection?.ToString()).FireAndForget();

            var vm = new ErrorDialogViewModel(ex);
            var error = new V2Views.Common.ErrorDialog(vm);

            GenContext.ToolBox.Shell.ShowModal(error);
        }

        internal void CleanStatusBar()
        {
            GenContext.ToolBox.Shell.ShowStatusBarMessage(string.Empty);
        }
    }
}
