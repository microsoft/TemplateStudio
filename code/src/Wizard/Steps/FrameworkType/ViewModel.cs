using Microsoft.Templates.Core;
using Microsoft.Templates.Wizard.Host;
using Microsoft.Templates.Wizard.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Microsoft.Templates.Wizard.Steps.FrameworkType
{
    public class ViewModel : StepViewModel
    {
        public ObservableCollection<ProjectInfoViewModel> FrameworkTypes { get; } = new ObservableCollection<ProjectInfoViewModel>();
        public override string PageTitle => Strings.PageTitle;

        public ViewModel(WizardContext context) : base(context)
        {
        }

        private ProjectInfoViewModel _selectedFrameworkType;
        public ProjectInfoViewModel SelectedFrameworkType
        {
            get => _selectedFrameworkType;
            set => SetProperty(ref _selectedFrameworkType, value);
        }

        public override async Task InitializeAsync()
        {
            FrameworkTypes.Clear();

            var frameworkTypes = Context.TemplatesRepository
                                    .GetAll()
                                    .Where(t => t.GetTemplateType() == TemplateType.Project && !String.IsNullOrWhiteSpace(t.GetFramework()))
                                    .Select(t => t.GetFramework())
                                    .Distinct()
                                    .Select(t => new ProjectInfoViewModel(t, Context.TemplatesRepository.GetFrameworkTypeInfo(t)))
                                    .ToList();

            FrameworkTypes.AddRange(frameworkTypes);

            var savedProject = Context.GetState<ViewModel, GenInfo>();

            if (savedProject == null)
            {
                SelectedFrameworkType = frameworkTypes.FirstOrDefault();
            }
            else
            {
                SelectedFrameworkType = frameworkTypes.FirstOrDefault(f => f.Name == savedProject.Template.GetFramework());
            }

            await Task.FromResult(true);
        }

        public override void SaveState()
        {
            var projectType = Context.GetState<ProjectType.ViewModel, string>();
            var template = Context.TemplatesRepository.GetAll()
                                                        .FirstOrDefault(t => t.GetTemplateType() == TemplateType.Project 
                                                            && t.GetProjectType() == projectType 
                                                            && t.GetFramework() == SelectedFrameworkType.Name);
            if (template == null)
            {
                throw new NullReferenceException($"Project template not found for framework '{SelectedFrameworkType.Name}'");
            }

            var genInfo = new GenInfo
            {
                Name = Context.Shell.ProjectName,
                Template = template
            };
            genInfo.Parameters.Add("UserName", Environment.UserName);

            Context.SetState(this, genInfo);
        }

        protected override Page GetPageInternal()
        {
            return new View();
        }
    }
}
