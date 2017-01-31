using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core;
using Microsoft.Templates.Wizard.Host;
using Microsoft.Templates.Wizard.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Templates.Wizard.Steps.ProjectsStep
{
    public class ProjectsStepViewModel : ObservableBase
    {
        private readonly WizardContext _context;

        public ProjectsStepViewModel(WizardContext context)
        {
            //TODO: VERIFY NOT NULL
            _context = context;
        }

        public ObservableCollection<TemplateViewModel> Templates { get; } = new ObservableCollection<TemplateViewModel>();

        private TemplateViewModel _templateSelected;
        public TemplateViewModel TemplateSelected
        {
            get { return _templateSelected; }
            set
            {
                SetProperty(ref _templateSelected, value);

                if (value != null)
                {
                    AddSelectionToContext(value);
                    _context.CanGoForward = true; 
                }
            }
        }

        //TODO: MAKE THIS METHOD TRULY ASYNC
        public async Task LoadDataAsync()
        {
            Templates.Clear();

            var projectTemplates = _context.TemplatesRepository
                                                    .GetAll()
                                                    .Where(f => f.GetTemplateType() == TemplateType.Project)
                                                    .Select(t => new TemplateViewModel(t))
                                                    .OrderBy(t => t.Order)
                                                    .ToList();

            Templates.AddRange(projectTemplates);

            TemplateSelected = projectTemplates.FirstOrDefault();

            await Task.FromResult(true);
        }

        private void AddSelectionToContext(TemplateViewModel template)
        {
            if (_context.SelectedTemplates.ContainsKey(this.GetType()))
            {
                _context.SelectedTemplates.Remove(this.GetType());
            }
			var genInfo = new GenInfo
			{
				Name = _context.Shell.ProjectName,
				Template = template.Info
			};
			genInfo.Parameters.Add("UserName", Environment.UserName);

			_context.SelectedTemplates.Add(this.GetType(), new GenInfo[] { genInfo });

		}
    }
}
