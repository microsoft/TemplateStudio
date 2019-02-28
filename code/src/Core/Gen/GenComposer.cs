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
        private const string All = "all";

        public static IEnumerable<string> GetSupportedProjectTypes(string platform)
        {
            return GenContext.ToolBox.Repo.GetAll()
                .Where(t => t.GetTemplateType() == TemplateType.Project
                                && t.GetPlatform() == platform)
                .SelectMany(t => t.GetProjectTypeList())
                .Distinct();
        }

        public static IEnumerable<SupportedFramework> GetSupportedFx(string projectType, string platform)
        {
            var filtered = GenContext.ToolBox.Repo.GetAll()
                          .Where(t => t.GetTemplateType() == TemplateType.Project
                          && t.GetProjectTypeList().Contains(projectType)
                          && t.GetPlatform().Equals(platform, StringComparison.OrdinalIgnoreCase)).ToList();

            var result = new List<SupportedFramework>();
            result.AddRange(filtered.SelectMany(t => t.GetFrontEndFrameworkList()).Select(name => new SupportedFramework(name, FrameworkTypes.FrontEnd)).ToList());
            result.AddRange(filtered.SelectMany(t => t.GetBackEndFrameworkList()).Select(name => new SupportedFramework(name, FrameworkTypes.BackEnd)));
            result = result.Distinct().ToList();

            return result;
        }

        public static IEnumerable<ITemplateInfo> GetPages(string projectType, string platform, string frontEndFramework = null, string backEndFramework = null)
        {
            return GetTemplateTypeInfo(projectType, platform, TemplateType.Page, frontEndFramework, backEndFramework);
        }

        public static IEnumerable<ITemplateInfo> GetFeatures(string projectType, string platform, string frontEndFramework = null, string backEndFramework = null)
        {
            return GetTemplateTypeInfo(projectType, platform, TemplateType.Feature, frontEndFramework, backEndFramework);
        }

        private static IEnumerable<ITemplateInfo> GetTemplateTypeInfo(string projectType, string platform, TemplateType type, string frontEndFramework = null, string backEndFramework = null)
        {
            return GenContext.ToolBox.Repo.Get(t => t.GetTemplateType() == type
               && (t.GetProjectTypeList().Contains(projectType) || t.GetProjectTypeList().Contains(All))
               && t.GetPlatform().Equals(platform, StringComparison.OrdinalIgnoreCase)
               && IsMatchFrontEnd(t, frontEndFramework)
               && IsMatchBackEnd(t, backEndFramework)).ToList();
        }

        private static bool IsMatchFrontEnd(ITemplateInfo info, string frontEndFramework)
        {
            return string.IsNullOrEmpty(frontEndFramework)
                    || info.GetFrontEndFrameworkList().Contains(frontEndFramework, StringComparer.OrdinalIgnoreCase)
                    || info.GetFrontEndFrameworkList().Contains(All, StringComparer.OrdinalIgnoreCase);
        }

        private static bool IsMatchBackEnd(ITemplateInfo info, string backEndFramework)
        {
            return string.IsNullOrEmpty(backEndFramework)
                    || info.GetBackEndFrameworkList().Contains(backEndFramework, StringComparer.OrdinalIgnoreCase)
                    || info.GetBackEndFrameworkList().Contains(All, StringComparer.OrdinalIgnoreCase);
        }

        public static IEnumerable<LayoutInfo> GetLayoutTemplates(string projectType, string frontEndFramework, string backEndFramework, string platform)
        {
            var projectTemplate = GetProjectTemplate(projectType, frontEndFramework, backEndFramework, platform);
            var layout = projectTemplate?
                .GetLayout()
                .Where(l => l.ProjectType == null || l.ProjectType.GetMultiValue().Contains(projectType));

            if (layout != null)
            {
                foreach (var item in layout)
                {
                    var template = GenContext.ToolBox.Repo.Find(t => t.GroupIdentity == item.TemplateGroupIdentity
                                                            && IsMatchFrontEnd(t, frontEndFramework)
                                                            && IsMatchBackEnd(t, backEndFramework)
                                                            && t.GetPlatform() == platform);

                    if (template == null)
                    {
                        LogOrAlertException(string.Format(StringRes.ErrorLayoutNotFound, item.TemplateGroupIdentity, frontEndFramework, backEndFramework, platform));
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
                            yield return new LayoutInfo() { Layout = item, Template = template };
                        }
                    }
                }
            }
        }

        public static IEnumerable<ITemplateInfo> GetAllDependencies(ITemplateInfo template, string frontEndFramework, string backEndFramework, string platform)
        {
            return GetDependencies(template, frontEndFramework, backEndFramework, platform, new List<ITemplateInfo>());
        }

        private static IEnumerable<ITemplateInfo> GetDependencies(ITemplateInfo template, string frontEndFramework, string backEndFramework, string platform, IList<ITemplateInfo> dependencyList)
        {
            var dependencies = template.GetDependencyList();

            foreach (var dependency in dependencies)
            {
                var dependencyTemplate = GenContext.ToolBox.Repo.Find(t => t.Identity == dependency
                                                                     && IsMatchFrontEnd(t, frontEndFramework)
                                                                     && IsMatchBackEnd(t, backEndFramework)
                                                                     && t.GetPlatform() == platform);

                if (dependencyTemplate == null)
                {
                    LogOrAlertException(string.Format(StringRes.ErrorDependencyNotFound, dependency, frontEndFramework, backEndFramework, platform));
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

                        GetDependencies(dependencyTemplate, frontEndFramework, backEndFramework, platform, dependencyList);
                    }
                }
            }

            return dependencyList;
        }

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

        public static IEnumerable<TemplateLicense> GetAllLicences(ITemplateInfo template, string frontEndFramework, string backEndFramework, string platform)
        {
            var templates = new List<ITemplateInfo>();
            templates.Add(template);
            templates.AddRange(GetAllDependencies(template, frontEndFramework, backEndFramework, platform));
            return templates.SelectMany(s => s.GetLicenses())
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
            var projectTemplate = GetProjectTemplate(userSelection.ProjectType, userSelection.FrontEndFramework, userSelection.BackEndFramework, userSelection.Platform);
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

        private static ITemplateInfo GetProjectTemplate(string projectType, string frontEndFramework, string backEndFramework, string platform)
        {
            return GenContext.ToolBox.Repo
                                .Find(t => t.GetTemplateType() == TemplateType.Project
                                            && t.GetProjectTypeList().Any(p => p.Equals(projectType, StringComparison.OrdinalIgnoreCase))
                                            && IsMatchFrontEnd(t, frontEndFramework)
                                            && IsMatchBackEnd(t, backEndFramework)
                                            && t.GetPlatform().Equals(platform, StringComparison.OrdinalIgnoreCase));
        }

        private static void AddTemplates(IEnumerable<TemplateInfo> templates, List<GenInfo> genQueue, UserSelection userSelection, bool newItemGeneration)
        {
            foreach (var selectionItem in templates)
            {
                if (!genQueue.Any(t => t.Name == selectionItem.Name && t.Template.Identity == selectionItem.Template.Identity))
                {
                    AddDependencyTemplates(selectionItem, genQueue, userSelection, newItemGeneration);
                    var genInfo = CreateGenInfo(selectionItem.Name, selectionItem.Template, genQueue, newItemGeneration);
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

        private static void AddDependencyTemplates(TemplateInfo selectionItem, List<GenInfo> genQueue, UserSelection userSelection, bool newItemGeneration)
        {
            var dependencies = GetAllDependencies(selectionItem.Template, userSelection.FrontEndFramework, userSelection.BackEndFramework, userSelection.Platform);

            foreach (var dependencyItem in dependencies)
            {
                var dependencyTemplate = userSelection.PagesAndFeatures.FirstOrDefault(f => f.Template.Identity == dependencyItem.Identity);

                if (dependencyTemplate.Template != null)
                {
                    if (!genQueue.Any(t => t.Name == dependencyTemplate.Name && t.Template.Identity == dependencyTemplate.Template.Identity))
                    {
                        var depGenInfo = CreateGenInfo(dependencyTemplate.Name, dependencyTemplate.Template, genQueue, newItemGeneration);
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
                new QueryableProperty("page", string.Join("|", userSelection.Pages.Select(p => p.Template.Identity))),
                new QueryableProperty("feature", string.Join("|", userSelection.Features.Select(p => p.Template.Identity))),
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
