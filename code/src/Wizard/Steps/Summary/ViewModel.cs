using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Microsoft.Templates.Wizard.Host;
using Microsoft.Templates.Core;
using System.Collections.ObjectModel;

namespace Microsoft.Templates.Wizard.Steps.Summary
{
    public class ViewModel : StepViewModel
    {
        public ObservableCollection<SummaryGroupViewModel> Templates { get; } = new ObservableCollection<SummaryGroupViewModel>();
        public ObservableCollection<SummaryLicenceViewModel> Licences { get; } = new ObservableCollection<SummaryLicenceViewModel>();
        public override string PageTitle => Strings.PagesTitle;

        public ViewModel(WizardContext context) : base(context)
        {
#if DEBUG
            TermsAccepted = true;
#else
            TermsAccepted = false;
#endif
        }

        private bool _termsAccepted;
        public bool TermsAccepted
        {
            get { return _termsAccepted; }
            set
            {
                SetProperty(ref _termsAccepted, value);
                Context.CanGoForward = value;
            }
        }

        public override async Task InitializeAsync()
        {
            AddTemplates(Strings.ProjectsTitle, FilterTemplates(TemplateType.Project));
            AddTemplates(Strings.PagesTitle, FilterTemplates(TemplateType.Page));
            AddTemplates(Strings.FeaturesTitle, FilterTemplates(TemplateType.Feature));

            AddLicences();

            await Task.FromResult(true);
        }

        protected override Page GetPageInternal()
        {
            return new View();
        }

        private void AddTemplates(string title, IEnumerable<SummaryItemViewModel> items)
        {
            var templatesSummary = new SummaryGroupViewModel
            {
                Title = title
            };
            templatesSummary.Items.AddRange(items);
            Templates.Add(templatesSummary);
        }

        private IEnumerable<SummaryItemViewModel> FilterTemplates(TemplateType templateType)
        {
            return Context.GetSelection()
                                .Where(t => t.Template != null)
                                .Where(t => t.Template.GetTemplateType() == templateType)
                                .Select(t => new SummaryItemViewModel(t));
        }

        private void AddLicences()
        {
            var selection = Context.GetSelection()
                                            .Where(t => t.Template != null);

            foreach (var selected in selection)
            {
                foreach (var templateLic in selected.Template.GetLicences())
                {
                    var lic = Licences.FirstOrDefault(l => l.Text == templateLic.text);
                    if (lic == null)
                    {
                        lic = new SummaryLicenceViewModel
                        {
                            Text = templateLic.text,
                            Url = templateLic.url
                        };
                        Licences.Add(lic);
                    }
                    lic.UsedIn.Add(selected.Name);
                }
            }
        }
    }
}
