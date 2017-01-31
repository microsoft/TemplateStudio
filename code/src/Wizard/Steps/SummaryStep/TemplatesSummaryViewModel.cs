using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core;
using Microsoft.Templates.Wizard.Dialog;
using Microsoft.Templates.Wizard.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Microsoft.Templates.Wizard.Steps.SummaryStep
{
    public class TemplatesSummaryViewModel : ObservableBase
    {
        private string _title;
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        public ObservableCollection<TemplatesSummaryItemViewModel> Items { get; } = new ObservableCollection<TemplatesSummaryItemViewModel>();
    }

    public class TemplatesSummaryItemViewModel : ObservableBase
    {
        private readonly string _licence;

        public TemplatesSummaryItemViewModel(GenInfo genInfo)
        {
            //TODO: CHECK NULLS
            Name = genInfo.Name;
            TemplateName = genInfo.Template.Name;
            Author = genInfo.Template.Author;
            Framework = genInfo.Template.GetFramework();

            _licence = genInfo.Template.GetLicenceTerms();
            LicenceVisibility = string.IsNullOrWhiteSpace(_licence) ? Visibility.Collapsed : Visibility.Visible;
        }

        public ICommand ViewLicenceCommand => new RelayCommand(ShowLicence);

        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        private string _templateName;
        public string TemplateName
        {
            get { return _templateName; }
            set { SetProperty(ref _templateName, value); }
        }

        private string _author;
        public string Author
        {
            get { return _author; }
            set { SetProperty(ref _author, value); }
        }

        private string _framework;
        public string Framework
        {
            get { return _framework; }
            set { SetProperty(ref _framework, value); }
        }

        private Visibility _licenceVisibility;
        public Visibility LicenceVisibility
        {
            get { return _licenceVisibility; }
            set { SetProperty(ref _licenceVisibility, value); }
        }

        private void ShowLicence()
        {
            MessageBox.Show(_licence, string.Format(SummaryStepResources.LicenceDialogCaption, TemplateName));
        }
    }
}
