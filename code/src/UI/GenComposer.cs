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
using System.Linq;

using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Composition;
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
                var template = GenContext.ToolBox.Repo.Find(t => t.GroupIdentity == item.templateGroupIdentity && t.GetFrameworkList().Contains(framework));

                if (template == null)
                {
                    LogOrAlertException(string.Format(StringRes.ExceptionLayoutNotFound, item.templateGroupIdentity, framework));
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
                var dependencyTemplate =  GenContext.ToolBox.Repo.Find(t => t.Identity == dependency && t.GetFrameworkList().Contains(framework));

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
                    else if (dependencyList.Any(d => d.Identity == template.Identity))
                    {
                        LogOrAlertException(string.Format(StringRes.ExceptionDependencyCircularReference, template.Identity, dependencyTemplate.Identity));
                    }
                    else
                    {
                        dependencyList.Add(dependencyTemplate);

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

            AddCompositionTemplates(genQueue, userSelection);            

            return genQueue;
        }        

        private static void AddProject(UserSelection userSelection, List<GenInfo> genQueue)
        {
            var projectTemplate = GetProjectTemplate(userSelection.ProjectType, userSelection.Framework);
            var genProject = CreateGenInfo(GenContext.Current.ProjectName, projectTemplate, genQueue);

            genProject.Parameters.Add(GenParams.Username, Environment.UserName);
            genProject.Parameters.Add(GenParams.WizardVersion, GenContext.ToolBox.WizardVersion);
            genProject.Parameters.Add(GenParams.TemplatesVersion, GenContext.ToolBox.TemplatesVersion);
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
                var genInfo = CreateGenInfo(selectionItem.name, selectionItem.template, genQueue);
                genInfo?.Parameters.Add(GenParams.HomePageName, userSelection.HomeName);
            }
        }

        private static void AddCompositionTemplates(List<GenInfo> genQueue, UserSelection userSelection)
        {
            var compositionCatalog = GetCompositionCatalog().ToList();
            var context = new QueryablePropertyDictionary();

            context.Add(new QueryableProperty("projectType", userSelection.ProjectType));
            context.Add(new QueryableProperty("framework", userSelection.Framework));

            var compositionQueue = new List<GenInfo>();

            foreach (var genItem in genQueue)
            {
                foreach (var compositionItem in compositionCatalog)
                {
                    if (compositionItem.query.Match(genItem.Template, context))
                    {                        
                        AddTemplate(genItem, compositionQueue, compositionItem.template, userSelection);
                    }

                }
            }

            genQueue.AddRange(compositionQueue);
        }

        private static IEnumerable<(CompositionQuery query, ITemplateInfo template)> GetCompositionCatalog()
        {
            return GenContext.ToolBox.Repo
                                        .Get(t => t.GetTemplateType() == TemplateType.Composition)
                                        .Select(t => (CompositionQuery.Parse(t.GetCompositionFilter()), t))
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
            var ns = GenContext.ToolBox.Shell.GetActiveNamespace();

            if (string.IsNullOrEmpty(ns))
            {
                ns = GenContext.Current.ProjectName;
            }

            genInfo.Parameters.Add(GenParams.RootNamespace, ns);

            //TODO: THIS SHOULD BE THE ITEM IN CONTEXT
            genInfo.Parameters.Add(GenParams.ItemNamespace, ns);
        }
    }
}
