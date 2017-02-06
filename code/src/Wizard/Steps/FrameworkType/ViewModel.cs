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
            var templatesByProjectType = Context.TemplatesRepository
                                            .GetAll()
                                            .Where(t =>
                                                t.GetTemplateType() == TemplateType.Project && t.GetProjectType() == GetSelectedProjectType()).ToList();
            List<string> frameworkTypeNames = new List<string>();
            templatesByProjectType.ForEach(t => frameworkTypeNames.AddRange(t.GetFrameworkList()));
            FrameworkTypes.AddRange(frameworkTypeNames.Select(ft => new ProjectInfoViewModel(ft, Context.TemplatesRepository.GetFrameworkTypeInfo(ft))));

            SelectedFrameworkType = FrameworkTypes.FirstOrDefault();
            await Task.FromResult(true);
        }

        private string GetSelectedProjectType()
        {
            return Context.GetState<ProjectType.ViewModel, string>();
        }

        public override void SaveState()
        {
            var projectType = Context.GetState<ProjectType.ViewModel, string>();
            var template = Context.TemplatesRepository.GetAll()
                                                        .FirstOrDefault(t => t.GetTemplateType() == TemplateType.Project
                                                            && t.GetProjectType() == projectType
                                                            && t.GetFrameworkList().Contains(SelectedFrameworkType.Name));
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
            genInfo.Parameters.Add(GenInfo.FrameworkPrameterName, SelectedFrameworkType.Name);


            Context.SetState(this, genInfo);
        }

        protected override Page GetPageInternal()
        {
            return new View();
        }
    }
}
