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
        public static IEnumerable<string> GetSupportedProjectTypes(string platform)
        {
            return GenContext.ToolBox.Repo.GetAll()
                .Where(t => t.GetTemplateType() == TemplateType.Project
                                && t.GetPlatform() == platform)
                .SelectMany(t => t.GetProjectTypeList())
                .Distinct();
        }

        public static IEnumerable<string> GetSupportedFx(string projectType, string platform)
        {
            return GenContext.ToolBox.Repo.GetAll()
                .Where(t => t.GetTemplateType() == TemplateType.Project
                                && t.GetProjectTypeList().Contains(projectType)
                                && t.GetPlatform() == platform)
                .SelectMany(t => t.GetFrameworkList())
                .Distinct();
        }

        public static IEnumerable<(LayoutItem Layout, ITemplateInfo Template)> GetLayoutTemplates(string projectType, string framework, string platform)
        {
            var projectTemplate = GetProjectTemplate(projectType, framework, platform);
            var layout = projectTemplate?.GetLayout();

            foreach (var item in layout)
            {
                var template = GenContext.ToolBox.Repo.Find(t => t.GroupIdentity == item.TemplateGroupIdentity && t.GetFrameworkList().Contains(framework) && t.GetPlatform() == platform);

                if (template == null)
                {
                    LogOrAlertException(string.Format(StringRes.ErrorLayoutNotFound, item.TemplateGroupIdentity, framework, platform));
                }
                else
                {
                    var templateType = template.GetTemplateType();

                    if (templateType != TemplateType.Page && templateType != TemplateType.Feature)
                    {
                        LogOrAlertException(string.Format(StringRes.ErrorLayoutType, template.Identity));
                    }
                    else
                    {
                        yield return (item, template);
                    }
                }
            }
        }

        public static IEnumerable<ITemplateInfo> GetAllDependencies(ITemplateInfo template, string framework, string platform)
        {
            return GetDependencies(template, framework, platform, new List<ITemplateInfo>());
        }

        private static IEnumerable<ITemplateInfo> GetDependencies(ITemplateInfo template, string framework, string platform, IList<ITemplateInfo> dependencyList)
        {
            var dependencies = template.GetDependencyList();

            foreach (var dependency in dependencies)
            {
                var dependencyTemplate = GenContext.ToolBox.Repo.Find(t => t.Identity == dependency && t.GetFrameworkList().Contains(framework) && t.GetPlatform() == platform);

                if (dependencyTemplate == null)
                {
                    LogOrAlertException(string.Format(StringRes.ErrorDependencyNotFound, dependency, framework, platform));
                }
                else
                {
                    var templateType = dependencyTemplate?.GetTemplateType();

                    if (templateType != TemplateType.Page && templateType != TemplateType.Feature)
                    {
                        LogOrAlertException(string.Format(StringRes.ErrorDependencyType, dependencyTemplate.Identity));
                    }
                    else if (dependencyTemplate.GetMultipleInstance())
                    {
                        LogOrAlertException(string.Format(StringRes.ErrorDependencyMultipleInstance, dependencyTemplate.Identity));
                    }
                    else if (dependencyList.Any(d => d.Identity == template.Identity && d.GetDependencyList().Contains(template.Identity)))
                    {
                        LogOrAlertException(string.Format(StringRes.ErrorDependencyCircularReference, template.Identity, dependencyTemplate.Identity));
                    }
                    else
                    {
                        if (!dependencyList.Contains(dependencyTemplate))
                        {
                            dependencyList.Add(dependencyTemplate);
                        }

                        GetDependencies(dependencyTemplate, framework, platform, dependencyList);
                    }
                }
            }

            return dependencyList;
        }

        public static IEnumerable<GenInfo> Compose(UserSelection userSelection)
        {
            var genQueue = new List<GenInfo>();

            if (string.IsNullOrEmpty(userSelection.ProjectType) || string.IsNullOrEmpty(userSelection.Framework))
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

        public static IEnumerable<TemplateLicense> GetAllLicences(ITemplateInfo template, string framework, string platform)
        {
            var templates = new List<ITemplateInfo>();
            templates.Add(template);
            templates.AddRange(GetAllDependencies(template, framework, platform));
            return templates.SelectMany(s => s.GetLicenses())
                .Distinct(new TemplateLicenseEqualityComparer())
                .ToList();
        }

        public static IEnumerable<GenInfo> ComposeNewItem(UserSelection userSelection)
        {
            var genQueue = new List<GenInfo>();

            if (string.IsNullOrEmpty(userSelection.ProjectType) || string.IsNullOrEmpty(userSelection.Framework))
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
            var projectTemplate = GetProjectTemplate(userSelection.ProjectType, userSelection.Framework, userSelection.Platform);
            var genProject = CreateGenInfo(GenContext.Current.ProjectName, projectTemplate, genQueue, false);

            genProject.Parameters.Add(GenParams.Username, Environment.UserName);
            genProject.Parameters.Add(GenParams.WizardVersion, string.Concat("v", GenContext.ToolBox.WizardVersion));
            genProject.Parameters.Add(GenParams.TemplatesVersion, string.Concat("v", GenContext.ToolBox.TemplatesVersion));
            genProject.Parameters.Add(GenParams.ProjectType, userSelection.ProjectType);
            genProject.Parameters.Add(GenParams.Framework, userSelection.Framework);
            genProject.Parameters.Add(GenParams.Platform, userSelection.Platform);
            genProject.Parameters.Add(GenParams.ProjectName, GenContext.Current.ProjectName);
        }

        private static ITemplateInfo GetProjectTemplate(string projectType, string framework, string platform)
        {
            return GenContext.ToolBox.Repo
                                    .Find(t => t.GetTemplateType() == TemplateType.Project
                                            && t.GetProjectTypeList().Any(p => p == projectType)
                                            && t.GetFrameworkList().Any(f => f == framework)
                                            && t.GetPlatform() == platform);
        }

        private static void AddTemplates(IEnumerable<(string name, ITemplateInfo template)> templates, List<GenInfo> genQueue, UserSelection userSelection, bool newItemGeneration)
        {
            foreach (var selectionItem in templates)
            {
                if (!genQueue.Any(t => t.Name == selectionItem.name && t.Template.Identity == selectionItem.template.Identity))
                {
                    AddDependencyTemplates(selectionItem, genQueue, userSelection, newItemGeneration);
                    var genInfo = CreateGenInfo(selectionItem.name, selectionItem.template, genQueue, newItemGeneration);
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

        private static void AddDependencyTemplates((string name, ITemplateInfo template) selectionItem, List<GenInfo> genQueue, UserSelection userSelection, bool newItemGeneration)
        {
            var dependencies = GetAllDependencies(selectionItem.template, userSelection.Framework, userSelection.Platform);

            foreach (var dependencyItem in dependencies)
            {
                var dependencyTemplate = userSelection.PagesAndFeatures.FirstOrDefault(f => f.template.Identity == dependencyItem.Identity);

                if (dependencyTemplate.template != null)
                {
                    if (!genQueue.Any(t => t.Name == dependencyTemplate.name && t.Template.Identity == dependencyTemplate.template.Identity))
                    {
                        var depGenInfo = CreateGenInfo(dependencyTemplate.name, dependencyTemplate.template, genQueue, newItemGeneration);
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
                new QueryableProperty("framework", userSelection.Framework),
                new QueryableProperty("page", string.Join("|", userSelection.Pages.Select(p => p.template.Identity))),
                new QueryableProperty("feature", string.Join("|", userSelection.Features.Select(p => p.template.Identity))),
            };

            var combinedQueue = new List<GenInfo>();

            foreach (var genItem in genQueue)
            {
                combinedQueue.Add(genItem);
                var compositionQueue = new List<GenInfo>();

                foreach (var compositionItem in compositionCatalog)
                {
                    if (compositionItem.template.GetLanguage() == userSelection.Language
                     && compositionItem.query.Match(genItem.Template, context))
                    {
                        AddTemplate(genItem, compositionQueue, compositionItem.template, userSelection, newItemGeneration);
                    }
                }

                combinedQueue.AddRange(compositionQueue.OrderBy(g => g.Template.GetCompositionOrder()));
            }

            return combinedQueue;
        }

        private static IEnumerable<(CompositionQuery query, ITemplateInfo template)> GetCompositionCatalog(string platform)
        {
            return GenContext.ToolBox.Repo
                                        .Get(t => t.GetTemplateType() == TemplateType.Composition && t.GetPlatform() == platform)
                                        .Select(t => (CompositionQuery.Parse(t.GetCompositionFilter()), t))
                                        .ToList();
        }

        private static void AddTemplate(GenInfo mainGenInfo, List<GenInfo> queue, ITemplateInfo targetTemplate, UserSelection userSelection, bool newItemGeneration)
        {
            if (targetTemplate != null)
            {
                foreach (var export in targetTemplate.GetExports())
                {
                    mainGenInfo.Parameters.Add(export.name, export.value);
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
