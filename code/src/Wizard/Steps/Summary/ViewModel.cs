using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Microsoft.Templates.Wizard.Host;
using Microsoft.Templates.Core;
using System.Collections.ObjectModel;
using Microsoft.Templates.Core.Gen;

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

        private string _selectedFramework;
        public string SelectedFramework
        {
            get { return _selectedFramework; }
            set { SetProperty(ref _selectedFramework, value); }
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
            SelectedFramework = Context.State.Framework;

            //TODO: REVIEW THIS
            var allTemplates = GenComposer.Compose(Context.State);

            AddTemplates(Strings.ProjectsTitle, FilterTemplates(allTemplates, TemplateType.Project));
            AddTemplates(Strings.PagesTitle, FilterTemplates(allTemplates, TemplateType.Page));
            AddTemplates(Strings.DevFeaturesTitle, FilterTemplates(allTemplates, TemplateType.DevFeature));
            AddTemplates(Strings.ConsumerFeaturesTitle, FilterTemplates(allTemplates, TemplateType.ConsumerFeature));

            AddLicences(allTemplates);

            await Task.FromResult(true);
        }

        public override void SaveState()
        {
            //NOTHING TO DO
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

        private IEnumerable<SummaryItemViewModel> FilterTemplates(IEnumerable<GenInfo> selection, TemplateType templateType)
        {
            return selection
                        .Where(t => t.Template != null)
                        .Where(t => t.Template.GetTemplateType() == templateType)
                        .Select(t => new SummaryItemViewModel(t));
        }

        private void AddLicences(IEnumerable<GenInfo> selection)
        {
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
