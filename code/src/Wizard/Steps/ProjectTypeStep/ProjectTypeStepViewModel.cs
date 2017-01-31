using Microsoft.Templates.Core;
using Microsoft.Templates.Wizard.Host;
using Microsoft.Templates.Wizard.ViewModels;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.Templates.Wizard.Steps.ProjectTypeStep
{
    public class ProjectTypeStepViewModel : ObservableBase
    {
        private readonly WizardContext _context;

        public ProjectTypeStepViewModel(WizardContext context)
        {
            //TODO: VERIFY NOT NULL
            _context = context;
            _context.SaveState += OnContextSaveState;
        }

        private void OnContextSaveState(object sender, EventArgs e) => _context.SelectedProjectType = SelectedProjectType;

        public ObservableCollection<ProjectTypeViewModel> ProjectTypes { get; } = new ObservableCollection<ProjectTypeViewModel>();

        private ProjectTypeViewModel _selectedProjectType;
        public ProjectTypeViewModel SelectedProjectType
        {
            get => _selectedProjectType;
            set => SetProperty(ref _selectedProjectType, value);
        }
        
        //TODO: MAKE THIS METHOD TRULY ASYNC
        public async Task LoadDataAsync()
        {
            ProjectTypes.Clear();

            var projectTypes = _context.TemplatesRepository
                                                    .GetAll()
                                                    .Where(t => t.GetTemplateType() == TemplateType.Project && !String.IsNullOrWhiteSpace(t.GetProjectType()))
                                                    .Select(t => t.GetProjectType())
                                                    .Distinct()
                                                    .Select(t => new ProjectTypeViewModel(t))
                                                    .ToList();

            ProjectTypes.AddRange(projectTypes);

            SelectedProjectType = projectTypes.FirstOrDefault();

            await Task.FromResult(true);
        }
    }
}
