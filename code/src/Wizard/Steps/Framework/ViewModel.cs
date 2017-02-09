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

namespace Microsoft.Templates.Wizard.Steps.Framework
{
    public class ViewModel : StepViewModel
    {
        public ObservableCollection<ProjectInfoViewModel> Frameworks { get; } = new ObservableCollection<ProjectInfoViewModel>();
        public override string PageTitle => Strings.PageTitle;

        public ViewModel(WizardContext context) : base(context)
        {
        }

        private ProjectInfoViewModel _selectedFramework;
        public ProjectInfoViewModel SelectedFramework
        {
            get => _selectedFramework;
            set => SetProperty(ref _selectedFramework, value);
        }

        public override async Task InitializeAsync()
        {
            Frameworks.Clear();

            foreach (var fx in GetSupportedFx(Context.State.ProjectType))
            {
                var pi = Context.TemplatesRepository.GetFrameworkTypeInfo(fx);
                if (pi != null)
                {
                    Frameworks.Add(new ProjectInfoViewModel(fx, pi));
                }
            }

            if (string.IsNullOrEmpty(Context.State.Framework))
            {
                SelectedFramework = Frameworks.FirstOrDefault();
            }
            else
            {
                SelectedFramework = Frameworks.FirstOrDefault(f => f.Name == Context.State.Framework);
            }

            await Task.FromResult(true);
        }

        public override void SaveState() => Context.State.Framework = SelectedFramework.Name;
        public override void CleanState() => Context.State.Framework = null;

        protected override Page GetPageInternal()
        {
            return new View();
        }

        private IEnumerable<string> GetSupportedFx(string projectType)
        {
            return Context.TemplatesRepository.GetAll()
                                                .Where(t => t.GetProjectType() == projectType)
                                                .SelectMany(t => t.GetFrameworkList())
                                                .Distinct();
        }
    }
}
