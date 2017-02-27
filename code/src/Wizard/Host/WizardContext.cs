using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core;
using Microsoft.Templates.Wizard.ViewModels;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Wizard.Host
{
    public class WizardState
    {
        public string ProjectType { get; set; }
        public string Framework { get; set; }
        public List<(string name, string templateName)> Pages { get; } = new List<(string name, string templateName)>();
        public List<(string name, string templateName)> DevFeatures { get; } = new List<(string name, string templateName)>();
        public List<(string name, string templateName)> ConsumerFeatures { get; } = new List<(string name, string templateName)>();
    }

    public class WizardContext : ObservableBase
    {
        public TemplatesRepository TemplatesRepository { get; }
        public GenShell Shell { get; }
        public WizardState State { get; } = new WizardState();


        public WizardContext(TemplatesRepository templatesRepository, GenShell shell)
        {
            TemplatesRepository = templatesRepository;
            Shell = shell;
        }

        private bool _canGoForward;
        public bool CanGoForward
        {
            get => _canGoForward;
            set => SetProperty(ref _canGoForward, value);
        }

        public IEnumerable<GenInfo> GetSelection()
        {
            //TODO: REVIEW THIS
            var selection = new List<GenInfo>();

            var projectTemplate = TemplatesRepository.Find(t => t.GetProjectType() == State.ProjectType && t.GetFrameworkList().Any(f => f == State.Framework));
            if (projectTemplate != null)
            {
                selection.Add(new GenInfo
                {
                    Name = Shell.ProjectName,
                    Template = projectTemplate
                });
            }

            if (State.Pages != null)
            {
                foreach (var p in State.Pages)
                {
                    var pageTemplate = TemplatesRepository.Find(t => t.Name == p.templateName);
                    if (pageTemplate != null)
                    {
                        selection.Add(new GenInfo
                        {
                            Name = p.name,
                            Template = pageTemplate
                        });
                    }
                } 
            }

            if (State.DevFeatures != null)
            {
                foreach (var f in State.DevFeatures)
                {
                    var featureTemplate = TemplatesRepository.Find(t => t.Name == f.templateName);
                    if (featureTemplate != null)
                    {
                        selection.Add(new GenInfo
                        {
                            Name = f.name,
                            Template = featureTemplate
                        });
                    }
                }
            }

            return selection;
        }
    }
}
