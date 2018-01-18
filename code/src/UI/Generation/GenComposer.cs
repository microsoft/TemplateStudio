// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Composition;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.UI.Resources;

namespace Microsoft.Templates.UI
{
    public class GenComposer
    {
        public static IEnumerable<string> GetSupportedFx(string projectType)
        {
            return GenContext.ToolBox.Repo.GetAll()
                .Where(t => t.GetProjectTypeList().Contains(projectType))
                .SelectMany(t => t.GetFrameworkList())
                .Distinct();
        }

        public static IEnumerable<(LayoutItem Layout, ITemplateInfo Template)> GetLayoutTemplates(string projectType, string framework)
        {
            var projectTemplate = GetProjectTemplate(projectType, framework);
            var layout = projectTemplate?.GetLayout();

            foreach (var item in layout)
            {
                var template = GenContext.ToolBox.Repo.Find(t => t.GroupIdentity == item.TemplateGroupIdentity && t.GetFrameworkList().Contains(framework));

                if (template == null)
                {
                    LogOrAlertException(string.Format(StringRes.ExceptionLayoutNotFound, item.TemplateGroupIdentity, framework));
                }
                else
                {
                    var templateType = template.GetTemplateType();

                    if (templateType != TemplateType.Page && templateType != TemplateType.Feature)
                    {
                        LogOrAlertException(string.Format(StringRes.ExceptionLayoutType, template.Identity));
                    }
                    else
                    {
                        yield return (item, template);
                    }
                }
            }
        }

        public static IEnumerable<ITemplateInfo> GetAllDependencies(ITemplateInfo template, string framework)
        {
            return GetDependencies(template, framework, new List<ITemplateInfo>());
        }

        private static IEnumerable<ITemplateInfo> GetDependencies(ITemplateInfo template, string framework, IList<ITemplateInfo> dependencyList)
        {
            var dependencies = template.GetDependencyList();

            foreach (var dependency in dependencies)
            {
                var dependencyTemplate = GenContext.ToolBox.Repo.Find(t => t.Identity == dependency && t.GetFrameworkList().Contains(framework));

                if (dependencyTemplate == null)
                {
                    LogOrAlertException(string.Format(StringRes.ExceptionDependencyNotFound, dependency, framework));
                }
                else
                {
                    var templateType = dependencyTemplate?.GetTemplateType();

                    if (templateType != TemplateType.Page && templateType != TemplateType.Feature)
                    {
                        LogOrAlertException(string.Format(StringRes.ExceptionDependencyType, dependencyTemplate.Identity));
                    }
                    else if (dependencyTemplate.GetMultipleInstance())
                    {
                        LogOrAlertException(string.Format(StringRes.ExceptionDependencyMultipleInstance, dependencyTemplate.Identity));
                    }
                    else if (dependencyList.Any(d => d.Identity == template.Identity && d.GetDependencyList().Contains(template.Identity)))
                    {
                        LogOrAlertException(string.Format(StringRes.ExceptionDependencyCircularReference, template.Identity, dependencyTemplate.Identity));
                    }
                    else
                    {
                        if (!dependencyList.Contains(dependencyTemplate))
                        {
                            dependencyList.Add(dependencyTemplate);
                        }

                        GetDependencies(dependencyTemplate, framework, dependencyList);
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
            AddTemplates(userSelection.Pages, genQueue, userSelection);
            AddTemplates(userSelection.Features, genQueue, userSelection);

            genQueue = AddInCompositionTemplates(genQueue, userSelection);

            return genQueue;
        }

        public static IEnumerable<TemplateLicense> GetAllLicences(UserSelection userSelection)
        {
            return Compose(userSelection)
                    .SelectMany(s => s.Template.GetLicenses())
                    .Distinct(new TemplateLicenseEqualityComparer())
                    .ToList();
        }

        public static IEnumerable<TemplateLicense> GetAllLicences(ITemplateInfo template, string framework)
        {
            return GetAllDependencies(template, framework)
                .SelectMany(s => s.GetLicenses())
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

            AddTemplates(userSelection.Pages, genQueue, userSelection);
            AddTemplates(userSelection.Features, genQueue, userSelection);

            genQueue = AddInCompositionTemplates(genQueue, userSelection);

            return genQueue;
        }

        private static void AddProject(UserSelection userSelection, List<GenInfo> genQueue)
        {
            var projectTemplate = GetProjectTemplate(userSelection.ProjectType, userSelection.Framework);
            var genProject = CreateGenInfo(GenContext.Current.ProjectName, projectTemplate, genQueue);

            genProject.Parameters.Add(GenParams.Username, Environment.UserName);
            genProject.Parameters.Add(GenParams.WizardVersion, string.Concat("v", GenContext.ToolBox.WizardVersion));
            genProject.Parameters.Add(GenParams.TemplatesVersion, string.Concat("v", GenContext.ToolBox.TemplatesVersion));
            genProject.Parameters.Add(GenParams.ProjectType, userSelection.ProjectType);
            genProject.Parameters.Add(GenParams.Framework, userSelection.Framework);
        }

        private static ITemplateInfo GetProjectTemplate(string projectType, string framework)
        {
            return GenContext.ToolBox.Repo
                                    .Find(t => t.GetTemplateType() == TemplateType.Project
                                            && t.GetProjectTypeList().Any(p => p == projectType)
                                            && t.GetFrameworkList().Any(f => f == framework));
        }

        private static void AddTemplates(IEnumerable<(string name, ITemplateInfo template)> templates, List<GenInfo> genQueue, UserSelection userSelection)
        {
            foreach (var selectionItem in templates)
            {
                if (!genQueue.Any(t => t.Name == selectionItem.name && t.Template.Identity == selectionItem.template.Identity))
                {
                    AddDependencyTemplates(selectionItem, genQueue, userSelection);
                    var genInfo = CreateGenInfo(selectionItem.name, selectionItem.template, genQueue);
                    genInfo?.Parameters.Add(GenParams.HomePageName, userSelection.HomeName);

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

        private static void AddDependencyTemplates((string name, ITemplateInfo template) selectionItem, List<GenInfo> genQueue, UserSelection userSelection)
        {
            var dependencies = GetAllDependencies(selectionItem.template, userSelection.Framework);

            foreach (var dependencyItem in dependencies)
            {
                var dependencyTemplate = userSelection.PagesAndFeatures.FirstOrDefault(f => f.template.Identity == dependencyItem.Identity);

                if (dependencyTemplate.template != null)
                {
                    if (!genQueue.Any(t => t.Name == dependencyTemplate.name && t.Template.Identity == dependencyTemplate.template.Identity))
                    {
                        var depGenInfo = CreateGenInfo(dependencyTemplate.name, dependencyTemplate.template, genQueue);
                        depGenInfo?.Parameters.Add(GenParams.HomePageName, userSelection.HomeName);
                    }
                }
                else
                {
                    LogOrAlertException(string.Format(StringRes.ExceptionDependencyMissing, dependencyItem.Identity));
                }
            }
        }

        private static List<GenInfo> AddInCompositionTemplates(List<GenInfo> genQueue, UserSelection userSelection)
        {
            var compositionCatalog = GetCompositionCatalog().ToList();
            var context = new QueryablePropertyDictionary
            {
                new QueryableProperty("projectType", userSelection.ProjectType),
                new QueryableProperty("framework", userSelection.Framework)
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
                        AddTemplate(genItem, compositionQueue, compositionItem.template, userSelection);
                    }
                }

                combinedQueue.AddRange(compositionQueue.OrderBy(g => g.Template.GetCompositionOrder()));
            }

            return combinedQueue;
        }

        private static IEnumerable<(CompositionQuery query, ITemplateInfo template)> GetCompositionCatalog()
        {
            return GenContext.ToolBox.Repo
                                        .Get(t => t.GetTemplateType() == TemplateType.Composition)
#pragma warning disable SA1008 // Opening parenthesis must be spaced correctly - StyleCop can't handle Tuples
                                        .Select(t => (CompositionQuery.Parse(t.GetCompositionFilter()), t))
#pragma warning restore SA1008 // Opening parenthesis must be spaced correctly
                                        .ToList();
        }

        private static void AddTemplate(GenInfo mainGenInfo, List<GenInfo> queue, ITemplateInfo targetTemplate, UserSelection userSelection)
        {
            if (targetTemplate != null)
            {
                foreach (var export in targetTemplate.GetExports())
                {
                    mainGenInfo.Parameters.Add(export.name, export.value);
                }

                var genInfo = CreateGenInfo(mainGenInfo.Name, targetTemplate, queue);
                genInfo?.Parameters.Add(GenParams.HomePageName, userSelection.HomeName);
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

        private static GenInfo CreateGenInfo(string name, ITemplateInfo template, List<GenInfo> queue)
        {
            var genInfo = new GenInfo
            {
                Name = name,
                Template = template
            };

            queue.Add(genInfo);

            AddDefaultParams(genInfo);

            return genInfo;
        }

        private static void AddDefaultParams(GenInfo genInfo)
        {
            var ns = GenContext.ToolBox.Shell.GetActiveProjectNamespace();

            if (string.IsNullOrEmpty(ns))
            {
                ns = GenContext.Current.ProjectName;
            }

            genInfo.Parameters.Add(GenParams.RootNamespace, ns);

            // TODO: THIS SHOULD BE THE ITEM IN CONTEXT
            genInfo.Parameters.Add(GenParams.ItemNamespace, ns);
        }
    }
}
