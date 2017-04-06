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
using System.Text;
using System.Threading.Tasks;

using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Locations;
using Microsoft.Templates.Wizard.Host;
using Microsoft.Templates.Core.Composition;

namespace Microsoft.Templates.Wizard
{
    public class GenComposer
    {
        public static IEnumerable<GenInfo> Compose(WizardState userSelection)
        {
            var genQueue = new List<GenInfo>();

            if (string.IsNullOrEmpty(userSelection.ProjectType))
            {
                return genQueue;
            }

            AddProject(userSelection, genQueue);
            AddTemplates(userSelection.Pages, genQueue);
            AddTemplates(userSelection.DevFeatures, genQueue);
            AddTemplates(userSelection.ConsumerFeatures, genQueue);

            AddMissingDependencies(genQueue);

            AddCompositionTemplates(genQueue, userSelection);

            return genQueue;
        }

        private static void AddProject(WizardState userSelection, List<GenInfo> genQueue)
        {
            var projectTemplate = GenContext.ToolBox.Repo
                                                        .Find(t => t.GetTemplateType() == TemplateType.Project
                                                            && t.GetProjectType() == userSelection.ProjectType
                                                            && t.GetFrameworkList().Any(f => f == userSelection.Framework));

            var genProject = CreateGenInfo(GenContext.Current.ProjectName, projectTemplate, genQueue);
            genProject.Parameters.Add(GenParams.Username, Environment.UserName);
        }

        private static void AddTemplates(IEnumerable<(string name, ITemplateInfo template)> userSelection, List<GenInfo> genQueue)
        {
            foreach (var selectionItem in userSelection)
            {
                CreateGenInfo(selectionItem.name, selectionItem.template, genQueue);
            }
        }

        private static void AddMissingDependencies(List<GenInfo> genQueue)
        {
            var currentGenQueue = genQueue.ToList();
            foreach (var item in currentGenQueue)
            {
                var dependencies = item.Template.GetDependencyList();
                foreach (var dependency in dependencies)
                {
                    if (!genQueue.ToList().Any(g => g.Template.Identity == dependency))
                    {
                        var dependencyTemplate = GenContext.ToolBox.Repo.Find(t => t.Identity == dependency);
                        CreateGenInfo(dependencyTemplate.Name, dependencyTemplate, genQueue);
                    }
                }
            }
        }

        private static void AddCompositionTemplates(List<GenInfo> genQueue, WizardState userSelection)
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
                        AddTemplate(genItem, compositionQueue, compositionItem.template);
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

        private static void AddTemplate(GenInfo mainGenInfo, List<GenInfo> queue, ITemplateInfo targetTemplate)
        {
            if (targetTemplate != null)
            {
                foreach (var export in targetTemplate.GetExports())
                {
                    mainGenInfo.Parameters.Add(export.name, export.value);
                }

                CreateGenInfo(mainGenInfo.Name, targetTemplate, queue);
            }
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
