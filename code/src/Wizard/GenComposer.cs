using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core;
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
        private TemplatesRepository _repository;
        public GenShell Shell { get; }

        public GenComposer(GenShell shell) : this(shell, new TemplatesRepository(new RemoteTemplatesLocation()))
        {
        }

        public GenComposer(GenShell shell, TemplatesRepository repository)
        {
            Shell = shell;
            _repository = repository;
        }

        public IEnumerable<GenInfo> Compose(WizardState userSelection)
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

            return genQueue;
        }

        private void AddPages(WizardState userSelection, List<GenInfo> genQueue)
        {
            foreach (var page in userSelection.Pages)
            {
                var pageTemplate = _repository.Find(t => t.Name == page.templateName);
                if (pageTemplate != null)
                {
                    var genPage = CreateGenInfo(page.name, pageTemplate, genQueue);

                    AddTemplate(genPage, genQueue, userSelection.Framework, "Page");
                    AddTemplate(genPage, genQueue, userSelection.Framework, "Page", userSelection.ProjectType);
                    AddTemplate(genPage, genQueue, userSelection.Framework, "Page", pageTemplate.Name);
                }
            }
        }

        private void AddProjects(WizardState userSelection, List<GenInfo> genQueue)
        {
            var projectTemplate = _repository
                                        .Find(t => t.GetTemplateType() == TemplateType.Project
                                            && t.GetProjectType() == userSelection.ProjectType
                                            && t.GetFrameworkList().Any(f => f == userSelection.Framework));

            var genProject = CreateGenInfo(Shell.ProjectName, projectTemplate, genQueue);
            genProject.Parameters.Add(GenParams.Username, Environment.UserName);

            AddTemplate(genProject, genQueue, userSelection.Framework, "Project");
            AddTemplate(genProject, genQueue, userSelection.Framework, "Project", userSelection.ProjectType);
        }

        private void AddDevFeatures(WizardState userSelection, List<GenInfo> genQueue)
        {
            foreach (var feature in userSelection.DevFeatures)
            {
                var featureTemplate = _repository.Find(t => t.Name == feature.templateName);
                if (featureTemplate != null)
                {
                    var genFeature = CreateGenInfo(feature.name, featureTemplate, genQueue);
                    AddTemplate(genFeature, genQueue, userSelection.Framework, "Feature", featureTemplate.Name);
                }
            }
        }

        private void AddConsumerFeatures(WizardState userSelection, List<GenInfo> genQueue)
        {
            foreach (var feature in userSelection.ConsumerFeatures)
            {
                var featureTemplate = _repository.Find(t => t.Name == feature.templateName);
                if (featureTemplate != null)
                {
                    CreateGenInfo(feature.name, featureTemplate, genQueue);
                }
            }
        }

        private void AddTemplate(GenInfo mainGenInfo, List<GenInfo> queue, params string[] predicates)
        {
            var predicate = string.Join(".", predicates.Select(p => p.ToLower()));

            AddTemplate(mainGenInfo, queue, t => t.Name.Equals(predicate, StringComparison.OrdinalIgnoreCase));
        }

        private void AddTemplate(GenInfo mainGenInfo, List<GenInfo> queue, Func<ITemplateInfo, bool> predicate)
        {
            var targetTemplate = _repository
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

        private GenInfo CreateGenInfo(string name, ITemplateInfo template, List<GenInfo> queue)
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

        private void AddDefaultParams(GenInfo genInfo)
        {
            genInfo.Parameters.Add(GenParams.RootNamespace, Shell.GetActiveNamespace());
            genInfo.Parameters.Add(GenParams.ItemNamespace, Shell.GetActiveNamespace());
        }
    }
}
