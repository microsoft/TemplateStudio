// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.TemplateEngine.Abstractions;
using Microsoft.TemplateEngine.Edge.Template;
using Microsoft.Templates.Core.Diagnostics;
using Microsoft.Templates.Core.Helpers;
using Microsoft.Templates.Core.Naming;
using Microsoft.Templates.Core.PostActions;
using Microsoft.Templates.Core.Templates;
using Microsoft.Templates.Resources;

namespace Microsoft.Templates.Core.Gen
{
    public abstract class GenController
    {
        public PostActionFactory PostactionFactory { get; internal set; }

        internal async Task<Dictionary<string, ITemplateCreationResult>> GenerateItemsAsync(IEnumerable<GenInfo> genItems)
        {
            var genResults = new Dictionary<string, ITemplateCreationResult>();

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
                    GenContext.ToolBox.Shell.UI.ShowStatusBarMessage(statusText);
                }

                AppHealth.Current.Info.TrackAsync($"Generating the template {genInfo.Template.Name} to {genInfo.GenerationPath}.").FireAndForget();

                var result = await CodeGen.Instance.Creator.InstantiateAsync(genInfo.Template, genInfo.Name, null, genInfo.GenerationPath, genInfo.Parameters);

                genResults.Add($"{genInfo.Template.Identity}_{genInfo.Name}", result);

                if (result.Status != CreationResultStatus.Success)
                {
                    throw new GenException(genInfo.Name, genInfo.Template.Name, result.ErrorMessage);
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

        internal void ExecutePostActions(GenInfo genInfo, ITemplateCreationResult generationResult)
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
                case TemplateType.Service:
                    return string.Format(StringRes.StatusBarGeneratingServiceMessage, $"{genInfo.Name} ({genInfo.Template.Name})");
                case TemplateType.Testing:
                    return string.Format(StringRes.StatusBarGeneratingTestingMessage, $"{genInfo.Name} ({genInfo.Template.Name})");
                default:
                    return null;
            }
        }

        internal static void ValidateUserSelection(UserSelection userSelection, bool isNewProject)
        {
            if (isNewProject)
            {
                var rootDir = Directory.GetParent(Directory.GetParent(GenContext.Current.DestinationPath).FullName).FullName;

                var projectNameService = new ProjectNameService(GenContext.ToolBox.Repo.ProjectNameValidationConfig, () => Fs.GetExistingFolderNames(rootDir));

                var result = projectNameService.Validate(GenContext.Current.ProjectName);
                {
                    if (!result.IsValid)
                    {
                        var errors = string.Join(Environment.NewLine, result.Errors.Select(e => $"Error: {e.ErrorType}; Validator: {e.ValidatorName}."));
                        throw new InvalidDataException(string.Format(StringRes.ErrorProjectNameValidationFailed, GenContext.Current.ProjectName, Environment.NewLine + errors));
                    }
                }
            }

            foreach (var item in userSelection.Items)
            {
                var template = GenContext.ToolBox.Repo.Find(t => t.Identity == item.TemplateId);

                if (template.GetItemNameEditable())
                {
                    Func<IEnumerable<string>> existingNames = () => userSelection.Items.Where(t => t.TemplateId != template.Identity).Select(n => n.Name);

                    var itemNameService = new ItemNameService(GenContext.ToolBox.Repo.ItemNameValidationConfig, existingNames);

                    var validationResult = itemNameService.Validate(item.Name);
                    {
                        if (!validationResult.IsValid)
                        {
                            var errors = string.Join(Environment.NewLine, validationResult.Errors.Select(e => $"Error: {e.ErrorType}; Validator: {e.ValidatorName}."));
                            throw new InvalidDataException(string.Format(StringRes.ErrorNamingValidationFailed, item.Name, Environment.NewLine + errors));
                        }
                    }
                }

                var dependencies = GenContext.ToolBox.Repo.GetDependencies(template, userSelection.Context, new List<ITemplateInfo>());

                foreach (var dependency in dependencies)
                {
                    if (!userSelection.Items.Any(i => i.TemplateId == dependency.Identity))
                    {
                        throw new InvalidDataException(string.Format(StringRes.ErrorDependencyMissing, dependency));
                    }
                }

                var requirements = GenContext.ToolBox.Repo.GetRequirements(template, userSelection.Context);

                if (requirements.Count() > 0 && !userSelection.Items.Any(i => requirements.Select(r => r.Identity).Contains(i.TemplateId)))
                {
                    throw new InvalidDataException(string.Format(StringRes.ErrorRequirementMissing, requirements.Select(r => r.Name).Aggregate((i, j) => $"{i},{j}")));
                }

                var exclusionTemplates = GenContext.ToolBox.Repo.GetExclusions(template, userSelection.Context);

                foreach (var exclusion in exclusionTemplates)
                {
                    if (userSelection.Items.Any(i => i.TemplateId == exclusion.Identity))
                    {
                        throw new InvalidDataException(string.Format(StringRes.ErrorExcludedTemplatesFound, exclusion.Name));
                    }
                }
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
