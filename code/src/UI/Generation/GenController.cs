// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.TemplateEngine.Edge.Template;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.PostActions;
using Microsoft.Templates.UI.Resources;
using Microsoft.Templates.UI.Views.Common;

namespace Microsoft.Templates.UI
{
    public abstract class GenController
    {
        internal PostActionFactory _postactionFactory;

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
            var postActions = _postactionFactory.FindPostActions(genInfo, generationResult);

            foreach (var postAction in postActions)
            {
                postAction.Execute();
            }
        }

        internal void ExecuteGlobalPostActions()
        {
            var postActions = _postactionFactory.FindGlobalPostActions();

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

            var error = new ErrorDialog(ex);

            GenContext.ToolBox.Shell.ShowModal(error);
        }
        internal bool VerifyNetVersion()
        {
            if (!DotNetVersion.IsAllowed())
            {
                var error = new ErrorDialog(new Exception(string.Format("Windows Template Studio extension requires .NET Framework version {0}. Please download .NET Framework {0} Developer Pack from https://www.microsoft.com/net/targeting.", DotNetVersion.MinimumAllowedVersionLabel)));
                GenContext.ToolBox.Shell.ShowModal(error);
                return false;
            }
            return true;
        }

        internal void CleanStatusBar()
        {
            GenContext.ToolBox.Shell.ShowStatusBarMessage(string.Empty);
        }
    }
}
