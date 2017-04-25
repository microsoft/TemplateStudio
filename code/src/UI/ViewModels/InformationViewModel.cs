using Microsoft.Templates.Core.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.TemplateEngine.Abstractions;
using Microsoft.Templates.Core;
using System.Windows.Input;
using Microsoft.Templates.UI.Views;
using Microsoft.Templates.UI.Resources;
using System.Windows;
using System.Collections.ObjectModel;

namespace Microsoft.Templates.UI.ViewModels
{
    public class InformationViewModel : Observable
    {
        private InformationWindow _infoWindow;

        private Visibility _informationVisibility = Visibility.Collapsed;
        public Visibility InformationVisibility
        {
            get => _informationVisibility;
            set => SetProperty(ref _informationVisibility, value);
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _informationType;
        public string InformationType
        {
            get => _informationType;
            set => SetProperty(ref _informationType, value);
        }

        private string _version;
        public string Version
        {
            get => _version;
            set => SetProperty(ref _version, value);
        }

        private string _author;
        public string Author
        {
            get => _author;
            set => SetProperty(ref _author, value);
        }

        private string _informationMD;
        public string InformationMD
        {
            get => _informationMD;
            set => SetProperty(ref _informationMD, value);
        }        
        public ObservableCollection<SummaryLicenceViewModel> LicenceTerms { get; } = new ObservableCollection<SummaryLicenceViewModel>();

        private Visibility _licencesVisibility = Visibility.Collapsed;
        public Visibility LicencesVisibility
        {
            get => _licencesVisibility;
            set => SetProperty(ref _licencesVisibility, value);
        }

        private ICommand _okCommand;
        public ICommand OkCommand => _okCommand ?? (_okCommand = new RelayCommand(OnOk));


        public InformationViewModel(InformationWindow infoWindow)
        {
            _infoWindow = infoWindow;
        }

        public void Iniatialize(TemplateInfoViewModel template)
        {
            Name = template.Name;
            InformationType = GetInformationType(template.TemplateType.ToString());
            Version = template.Version;
            Author = template.Author;
            if (template.LicenceTerms != null && template.LicenceTerms.Any())
            {
                LicenceTerms.AddRange(template.LicenceTerms.Select(l => new SummaryLicenceViewModel(l)));
                LicencesVisibility = Visibility.Visible;
            }
            else
            {
                LicencesVisibility = Visibility.Collapsed;
            }
            InformationMD = template.Description;
        }        

        public void Iniatialize(MetadataInfoViewModel metadataInfo)
        {
            Name = metadataInfo.Name;
            InformationType = GetInformationType(metadataInfo.MetadataType);
            Author = metadataInfo.Author;
            if(metadataInfo.LicenceTerms != null && metadataInfo.LicenceTerms.Any())
            {
                LicenceTerms.AddRange(metadataInfo.LicenceTerms.Select(l => new SummaryLicenceViewModel(l)));
                LicencesVisibility = Visibility.Visible;
            }
            else
            {
                LicencesVisibility = Visibility.Collapsed;
            }
            InformationMD = metadataInfo.Description;
        }

        public void UnsuscribeEventHandlers()
        {
        }

        private void OnOk()
        {
            _infoWindow.DialogResult = false;
            _infoWindow.Close();
        }

        private string GetInformationType(string name)
        {
            switch (name)
            {
                case "projectTypes":
                    return StringRes.TemplateTypeProjectType;
                case "frameworks":
                    return StringRes.TemplateTypeFramework;
                case "Page":
                    return StringRes.TemplateTypePage;
                case "Feature":
                    return StringRes.TemplateTypeFeature;
                default:
                    return String.Empty;
            }
        }
    }
}
