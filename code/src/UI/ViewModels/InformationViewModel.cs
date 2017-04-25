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

namespace Microsoft.Templates.UI.ViewModels
{
    public class InformationViewModel : Observable
    {
        private InformationWindow _infoWindow;

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

        private IEnumerable<(string text, string url)> _licenceTerms;
        public IEnumerable<(string text, string url)> LicenceTerms
        {
            get => _licenceTerms;
            set => SetProperty(ref _licenceTerms, value);
        }

        private ICommand _okCommand;
        public ICommand OkCommand => _okCommand ?? (_okCommand = new RelayCommand(OnOk));


        public InformationViewModel(InformationWindow infoWindow)
        {
            _infoWindow = infoWindow;
        }

        public async Task IniatializeAsync(TemplateInfoViewModel template)
        {
            Name = template.Name;
            InformationType = GetInformationType(template.TemplateType.ToString());
            Version = template.Version;
            Author = template.Author;
            LicenceTerms = template.LicenceTerms;
        }

        public void UnsuscribeEventHandlers()
        {
        }

        public async Task IniatializeAsync(MetadataInfoViewModel metadataInfo)
        {
            Name = metadataInfo.Name;
            InformationType = GetInformationType(metadataInfo.MetadataType);
            Author = metadataInfo.Author;
            LicenceTerms = metadataInfo.LicenceTerms;
            InformationMD = metadataInfo.Description;
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
