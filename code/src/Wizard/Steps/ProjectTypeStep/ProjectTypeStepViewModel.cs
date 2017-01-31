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
            _context.SaveState += OncontextSaveState;
        }

        private void OncontextSaveState(object sender, EventArgs e)
        {            
        }

        public ObservableCollection<ProjectTypeViewModel> ProjectTypes { get; } = new ObservableCollection<ProjectTypeViewModel>();

        private ProjectTypeViewModel _projectTypeSelected;
        public ProjectTypeViewModel ProjectTypeSelected
        {
            get { return _projectTypeSelected; }
            set
            {
                SetProperty(ref _projectTypeSelected, value);
                if (value != null)
                {
                    _context.CanGoForward = true;
                    ProjectType = value.ProjectType;
                }
            }
        }

        private string _projectType;
        public string ProjectType
        {
            get { return _projectType; }
            set
            {
                SetProperty(ref _projectType, value);
                if (string.IsNullOrWhiteSpace(value))
                {
                    _context.CanGoForward = false;
                }
                else
                {
                    _context.CanGoForward = true;
                }
            }
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

            ProjectTypeSelected = projectTypes.FirstOrDefault();

            await Task.FromResult(true);
        }
    }
}
