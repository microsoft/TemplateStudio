using Microsoft.Templates.Core;
using Microsoft.Templates.Wizard.Host;
using Microsoft.Templates.Wizard.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Microsoft.Templates.Wizard.Steps.SummaryStep
{
    public class SummaryStepViewModel : ObservableBase
    {
        private readonly WizardContext _context;

        public ObservableCollection<TemplatesSummaryViewModel> Templates { get; } = new ObservableCollection<TemplatesSummaryViewModel>();

        public SummaryStepViewModel(WizardContext context)
        {
            //TODO: VERIFY NOT NULL
            _context = context;
        }

        //TODO: MAKE THIS METHOD TRULY ASYNC
        public async Task LoadDataAsync()
        {
            AddTemplates(SummaryStepResources.ProjectsTitle, FilterTemplates(TemplateType.Project));
            AddTemplates(SummaryStepResources.PagesTitle, FilterTemplates(TemplateType.Page));
            AddTemplates(SummaryStepResources.FeaturesTitle, FilterTemplates(TemplateType.Feature));

            await Task.FromResult(true);
        }

        private void AddTemplates(string title, IEnumerable<string> items)
        {
            var templatesSummary = new TemplatesSummaryViewModel
            {
                Title = title
            };
            templatesSummary.Items.AddRange(items);
            Templates.Add(templatesSummary);
        }

        private IEnumerable<string> FilterTemplates(TemplateType templateType)
        {
            return _context.SelectedTemplates
                                    .SelectMany(t => t.Value)
                                    .Where(t => t.Template.GetTemplateType() == templateType)
                                    //TODO: REVIEW THIS
                                    .Select(t => $"{t.Template.Name} ({t.Template.GetFramework()})");
        }
    }
}
