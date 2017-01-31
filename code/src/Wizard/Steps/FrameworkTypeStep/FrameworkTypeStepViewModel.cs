using Microsoft.Templates.Core;
using Microsoft.Templates.Wizard.Host;
using Microsoft.Templates.Wizard.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Wizard.Steps.FrameworkTypeStep
{
    public class FrameworkTypeStepViewModel : ObservableBase
    {
        private readonly WizardContext _context;

        public FrameworkTypeStepViewModel(WizardContext context)
        {
            //TODO: VERIFY NOT NULL
            _context = context;
            _context.SaveState += OnContextSaveState;
        }

        private void OnContextSaveState(object sender, EventArgs e)
        {
            _context.SelectedFrameworkType = SelectedFrameworkType;

            var type = this.GetType();
            if (!_context.SelectedTemplates.ContainsKey(type))
            {
                var template = _context.TemplatesRepository
                                    .GetAll()
                                    .FirstOrDefault(t => t.GetTemplateType() == TemplateType.Project &&
                                           t.GetProjectType() == _context.SelectedProjectType.Name &&
                                           t.GetFramework() == _context.SelectedFrameworkType.Name);
                if (template == null)
                {
                    throw new NullReferenceException("Project template not found.");
                }

                var genInfo = new GenInfo
                {
                    Name = _context.Shell.ProjectName,
                    Template = template
                };
                genInfo.Parameters.Add("UserName", Environment.UserName);

                _context.SelectedTemplates.Add(this.GetType(), new GenInfo[] { genInfo });
            }            
        }

        public ObservableCollection<ProjectInfoViewModel> FrameworkTypes { get; } = new ObservableCollection<ProjectInfoViewModel>();

        private ProjectInfoViewModel _selectedFrameworkType;
        public ProjectInfoViewModel SelectedFrameworkType
        {
            get => _selectedFrameworkType;
            set => SetProperty(ref _selectedFrameworkType, value);
        }

        //TODO: MAKE THIS METHOD TRULY ASYNC
        public async Task LoadDataAsync()
        {
            FrameworkTypes.Clear();

            var frameworkTypes = _context.TemplatesRepository
                                    .GetAll()
                                    .Where(t => t.GetTemplateType() == TemplateType.Project && !String.IsNullOrWhiteSpace(t.GetFramework()))
                                    .Select(t => t.GetFramework())
                                    .Distinct()
                                    .Select(t => new ProjectInfoViewModel(t, _context.TemplatesRepository.GetFrameworkTypeInfo(t)))
                                    .ToList();

            FrameworkTypes.AddRange(frameworkTypes);

            SelectedFrameworkType = frameworkTypes.FirstOrDefault();

            await Task.FromResult(true);
        }
    }
}
