using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Locations;
using Microsoft.Templates.Wizard.Host;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            AddProjects(userSelection, genQueue);
            AddPages(userSelection, genQueue);
            AddDevFeatures(userSelection, genQueue);
            AddConsumerFeatures(userSelection, genQueue);

            AddMissingDependencies(genQueue);
            return genQueue;
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

        private static void AddPages(WizardState userSelection, List<GenInfo> genQueue)
        {
            foreach (var page in userSelection.Pages)
            {
                var pageTemplate = GenContext.ToolBox.Repo.Find(t => t.Name == page.templateName);
                if (pageTemplate != null)
                {
                    var genPage = CreateGenInfo(page.name, pageTemplate, genQueue);

                    AddTemplate(genPage, genQueue, userSelection.Framework, "Page");
                    AddTemplate(genPage, genQueue, userSelection.Framework, "Page", userSelection.ProjectType);
                    AddTemplate(genPage, genQueue, userSelection.Framework, "Page", pageTemplate.Name);
                }
            }
        }

        private static void AddProjects(WizardState userSelection, List<GenInfo> genQueue)
        {
            var projectTemplate = GenContext.ToolBox.Repo
                                                        .Find(t => t.GetTemplateType() == TemplateType.Project
                                                            && t.GetProjectType() == userSelection.ProjectType
                                                            && t.GetFrameworkList().Any(f => f == userSelection.Framework));

            var genProject = CreateGenInfo(GenContext.Current.ProjectName, projectTemplate, genQueue);
            genProject.Parameters.Add(GenParams.Username, Environment.UserName);

            AddTemplate(genProject, genQueue, userSelection.Framework, "Project");
            AddTemplate(genProject, genQueue, userSelection.Framework, "Project", userSelection.ProjectType);
        }

        private static void AddDevFeatures(WizardState userSelection, List<GenInfo> genQueue)
        {
            foreach (var feature in userSelection.DevFeatures)
            {
                var featureTemplate = GenContext.ToolBox.Repo.Find(t => t.Name == feature.templateName);
                if (featureTemplate != null)
                {
                    var genFeature = CreateGenInfo(feature.name, featureTemplate, genQueue);
                    AddTemplate(genFeature, genQueue, userSelection.Framework, "Feature", featureTemplate.Name);
                }
            }
        }

        private static void AddConsumerFeatures(WizardState userSelection, List<GenInfo> genQueue)
        {
            foreach (var feature in userSelection.ConsumerFeatures)
            {
                var featureTemplate = GenContext.ToolBox.Repo.Find(t => t.Name == feature.templateName);
                if (featureTemplate != null)
                {
                    CreateGenInfo(feature.name, featureTemplate, genQueue);
                }
            }
        }

        private static void AddTemplate(GenInfo mainGenInfo, List<GenInfo> queue, params string[] predicates)
        {
            var predicate = string.Join(".", predicates.Select(p => p.ToLower()));

            AddTemplate(mainGenInfo, queue, t => t.Name.Equals(predicate, StringComparison.OrdinalIgnoreCase));
        }

        private static void AddTemplate(GenInfo mainGenInfo, List<GenInfo> queue, Func<ITemplateInfo, bool> predicate)
        {
            var targetTemplate = GenContext.ToolBox.Repo
                                                        .Get(t => t.GetTemplateType() == TemplateType.Framework)
                                                        .FirstOrDefault(predicate);

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
