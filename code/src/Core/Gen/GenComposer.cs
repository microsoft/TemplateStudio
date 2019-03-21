// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Composition;
using Microsoft.Templates.Core.Resources;

namespace Microsoft.Templates.Core.Gen
{
    public class GenComposer
    {
        public static IEnumerable<GenInfo> Compose(UserSelection userSelection)
        {
            var genQueue = new List<GenInfo>();

            if (string.IsNullOrEmpty(userSelection.ProjectType) || string.IsNullOrEmpty(userSelection.FrontEndFramework))
            {
                return genQueue;
            }

            AddProject(userSelection, genQueue);
            AddTemplates(userSelection.Pages, genQueue, userSelection, false);
            AddTemplates(userSelection.Features, genQueue, userSelection, false);

            genQueue = AddInCompositionTemplates(genQueue, userSelection, false);

            return genQueue;
        }

        public static IEnumerable<TemplateLicense> GetAllLicences(UserSelection userSelection)
        {
            return Compose(userSelection)
                    .SelectMany(s => s.Template.GetLicenses())
                    .Distinct(new TemplateLicenseEqualityComparer())
                    .ToList();
        }

        public static IEnumerable<GenInfo> ComposeNewItem(UserSelection userSelection)
        {
            var genQueue = new List<GenInfo>();

            if (string.IsNullOrEmpty(userSelection.ProjectType) || string.IsNullOrEmpty(userSelection.FrontEndFramework))
            {
                return genQueue;
            }

            AddTemplates(userSelection.Pages, genQueue, userSelection, true);
            AddTemplates(userSelection.Features, genQueue, userSelection, true);

            genQueue = AddInCompositionTemplates(genQueue, userSelection, true);

            return genQueue;
        }

        private static void AddProject(UserSelection userSelection, List<GenInfo> genQueue)
        {
            var projectTemplates = GenContext.ToolBox.Repo.GetTemplates(TemplateType.Project, userSelection.Platform, userSelection.ProjectType, userSelection.FrontEndFramework, userSelection.BackEndFramework);

            foreach (var projectTemplate in projectTemplates)
            {
                var genProject = CreateGenInfo(GenContext.Current.ProjectName, projectTemplate, genQueue, false);

                genProject.Parameters.Add(GenParams.Username, Environment.UserName);
                genProject.Parameters.Add(GenParams.WizardVersion, string.Concat("v", GenContext.ToolBox.WizardVersion));
                genProject.Parameters.Add(GenParams.TemplatesVersion, string.Concat("v", GenContext.ToolBox.TemplatesVersion));
                genProject.Parameters.Add(GenParams.ProjectType, userSelection.ProjectType);
                genProject.Parameters.Add(GenParams.FrontEndFramework, userSelection.FrontEndFramework);
                genProject.Parameters.Add(GenParams.BackEndFramework, userSelection.BackEndFramework);
                genProject.Parameters.Add(GenParams.Platform, userSelection.Platform);
                genProject.Parameters.Add(GenParams.ProjectName, GenContext.Current.ProjectName);
            }
        }

        private static void AddTemplates(IEnumerable<UserSelectionItem> selectedTemplates, List<GenInfo> genQueue, UserSelection userSelection, bool newItemGeneration)
        {
            foreach (var selectedTemplate in selectedTemplates)
            {
                if (!genQueue.Any(t => t.Name == selectedTemplate.Name && t.Template.Identity == selectedTemplate.TemplateId))
                {
                    var template = GenContext.ToolBox.Repo.Find(t => t.Identity == selectedTemplate.TemplateId);
                    AddDependencyTemplates(template, genQueue, userSelection, newItemGeneration);
                    var genInfo = CreateGenInfo(selectedTemplate.Name, template, genQueue, newItemGeneration);
                    genInfo?.Parameters.Add(GenParams.HomePageName, userSelection.HomeName);
                    genInfo?.Parameters.Add(GenParams.ProjectName, GenContext.Current.ProjectName);
                    genInfo?.Parameters.Add(GenParams.Username, Environment.UserName);

                    foreach (var dependency in genInfo?.Template.GetDependencyList())
                    {
                        if (genInfo.Template.Parameters.Any(p => p.Name == dependency))
                        {
                            var dependencyName = genQueue.FirstOrDefault(t => t.Template.Identity == dependency).Name;
                            genInfo.Parameters.Add(dependency, dependencyName);
                        }
                    }
                }
            }
        }

        private static void AddDependencyTemplates(ITemplateInfo template, List<GenInfo> genQueue, UserSelection userSelection, bool newItemGeneration)
        {
            var dependencies = GenContext.ToolBox.Repo.GetDependencies(template, userSelection.Platform, userSelection.ProjectType, userSelection.FrontEndFramework, userSelection.BackEndFramework, new List<ITemplateInfo>());

            foreach (var dependencyItem in dependencies)
            {
                var dependencyTemplate = userSelection.PagesAndFeatures.FirstOrDefault(f => f.TemplateId == dependencyItem.Identity);

                if (dependencyTemplate != null)
                {
                    if (!genQueue.Any(t => t.Name == dependencyTemplate.Name && t.Template.Identity == dependencyTemplate.TemplateId))
                    {
                        var dependencyTemplateInfo = GenContext.ToolBox.Repo.Find(t => t.Identity == dependencyTemplate.TemplateId);
                        var depGenInfo = CreateGenInfo(dependencyTemplate.Name, dependencyTemplateInfo, genQueue, newItemGeneration);
                        depGenInfo?.Parameters.Add(GenParams.HomePageName, userSelection.HomeName);
                        depGenInfo?.Parameters.Add(GenParams.ProjectName, GenContext.Current.ProjectName);
                    }
                }
                else
                {
                    LogOrAlertException(string.Format(StringRes.ErrorDependencyMissing, dependencyItem.Identity));
                }
            }
        }

        private static List<GenInfo> AddInCompositionTemplates(List<GenInfo> genQueue, UserSelection userSelection, bool newItemGeneration)
        {
            var compositionCatalog = GetCompositionCatalog(userSelection.Platform).ToList();

            var context = new QueryablePropertyDictionary
            {
                new QueryableProperty("projecttype", userSelection.ProjectType),
                new QueryableProperty("page", string.Join("|", userSelection.Pages.Select(p => p.TemplateId))),
                new QueryableProperty("feature", string.Join("|", userSelection.Features.Select(p => p.TemplateId))),
            };

            if (!string.IsNullOrEmpty(userSelection.FrontEndFramework))
            {
                context.Add(new QueryableProperty("frontendframework", userSelection.FrontEndFramework));
            }

            if (!string.IsNullOrEmpty(userSelection.BackEndFramework))
            {
                context.Add(new QueryableProperty("backendframework", userSelection.BackEndFramework));
            }

            var combinedQueue = new List<GenInfo>();

            foreach (var genItem in genQueue)
            {
                combinedQueue.Add(genItem);
                var compositionQueue = new List<GenInfo>();

                foreach (var compositionItem in compositionCatalog)
                {
                    if (compositionItem.Template.GetLanguage() == userSelection.Language
                     && compositionItem.Query.Match(genItem.Template, context))
                    {
                        AddTemplate(genItem, compositionQueue, compositionItem.Template, userSelection, newItemGeneration);
                    }
                }

                combinedQueue.AddRange(compositionQueue.OrderBy(g => g.Template.GetCompositionOrder()));
            }

            return combinedQueue;
        }

        private static IEnumerable<CompositionInfo> GetCompositionCatalog(string platform)
        {
            return GenContext.ToolBox.Repo
                                        .Get(t => t.GetTemplateType() == TemplateType.Composition && t.GetPlatform() == platform)
                                        .Select(t => new CompositionInfo() { Query = CompositionQuery.Parse(t.GetCompositionFilter()), Template = t })
                                        .ToList();
        }

        private static void AddTemplate(GenInfo mainGenInfo, List<GenInfo> queue, ITemplateInfo targetTemplate, UserSelection userSelection, bool newItemGeneration)
        {
            if (targetTemplate != null)
            {
                foreach (var export in targetTemplate.GetExports())
                {
                    mainGenInfo.Parameters.Add(export.Key, export.Value);
                }

                var genInfo = CreateGenInfo(mainGenInfo.Name, targetTemplate, queue, newItemGeneration);
                genInfo?.Parameters.Add(GenParams.HomePageName, userSelection.HomeName);
                genInfo?.Parameters.Add(GenParams.ProjectName, GenContext.Current.ProjectName);
            }
        }

        private static void LogOrAlertException(string message)
        {
#if DEBUG
            throw new GenException(message);
#else
            Core.Diagnostics.AppHealth.Current.Error.TrackAsync(message).FireAndForget();
#endif
        }

        private static GenInfo CreateGenInfo(string name, ITemplateInfo template, List<GenInfo> queue, bool newItemGeneration)
        {
            var genInfo = new GenInfo(name, template);

            queue.Add(genInfo);

            AddDefaultParams(genInfo, newItemGeneration);

            return genInfo;
        }

        private static void AddDefaultParams(GenInfo genInfo, bool newItemGeneration)
        {
            var ns = string.Empty;

            if (newItemGeneration)
            {
                ns = GenContext.ToolBox.Shell.GetActiveProjectNamespace();
            }

            if (string.IsNullOrEmpty(ns))
            {
                ns = GenContext.Current.SafeProjectName;
            }

            // TODO: This is needed to make legacy tests work, remove once 3.1 is released
            genInfo.Parameters.Add(GenParams.ItemNamespace, ns);

            genInfo.Parameters.Add(GenParams.RootNamespace, ns);
        }
    }
}
