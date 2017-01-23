using Microsoft.Templates.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Microsoft.Templates.Wizard.ViewModels
{
    public class AddProjectViewModel : ObservableBase
    {
        private readonly TemplatesRepository _templatesRepository;

        public ObservableCollection<TemplateViewModel> Templates { get; } = new ObservableCollection<TemplateViewModel>();

        private TemplateViewModel _templateSelected;
        public TemplateViewModel TemplateSelected
        {
            get { return _templateSelected; }
            set
            {
                SetProperty(ref _templateSelected, value);
            }
        }

        public AddProjectViewModel(TemplatesRepository templatesRepository)
        {
            _templatesRepository = templatesRepository;
        }

        public async Task LoadDataAsync()
        {
            Templates.Clear();

            var projectTemplates = _templatesRepository
                                                .GetAll()
                                                .Where(f => f.GetTemplateType() == TemplateType.Project)
                                                .Select(t => new TemplateViewModel(t))
                                                .OrderBy(t => t.Order)
                                                .ToList();

            Templates.AddRange(projectTemplates);

            TemplateSelected = projectTemplates.FirstOrDefault();

            await Task.FromResult(true);
        }
    }
}
