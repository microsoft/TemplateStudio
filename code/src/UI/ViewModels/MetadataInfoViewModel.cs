using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Mvvm;
using Microsoft.Templates.UI.Views;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Microsoft.Templates.UI.ViewModels
{
    public class MetadataInfoViewModel : Observable
    {
        private MetadataInfo _metadataInfo;

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _displayName;
        public string DisplayName
        {
            get => _displayName;
            set => SetProperty(ref _displayName, value);
        }

        private string _summary;
        public string Summary
        {
            get => _summary;
            set => SetProperty(ref _summary, value);
        }

        private string _description;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        private string _icon;
        public string Icon
        {
            get => _icon; 
            set => SetProperty(ref _icon, value);
        }

        private string _author;
        public string Author
        {
            get => _author;
            set => SetProperty(ref _author, value);
        }

        private IEnumerable<TemplateLicense> _licenceTerms;
        public IEnumerable<TemplateLicense> LicenceTerms
        {
            get => _licenceTerms;
            set => SetProperty(ref _licenceTerms, value);
        }

        private string _metadataType;
        public string MetadataType
        {
            get => _metadataType;
            set => SetProperty(ref _metadataType, value);
        }

        private RelayCommand _showInfoCommand;
        public RelayCommand ShowInfoCommand => _showInfoCommand ?? (_showInfoCommand = new RelayCommand(() => { OnShowInfo(); }));

        private void OnShowInfo()
        {
            MainViewModel.Current.InfoShapeVisibility = Visibility.Visible;
            var infoView = new InformationWindow(this, MainViewModel.Current.MainView);
            try
            {
                GenContext.ToolBox.Shell.ShowModal(infoView);
                MainViewModel.Current.InfoShapeVisibility = Visibility.Collapsed;

            }
            catch (Exception)
            {
            }
        }        

        public MetadataInfoViewModel(MetadataInfo metadataInfo)
        {
            if (metadataInfo == null)
            {
                return;
            }
            _metadataInfo = metadataInfo;
            Name = metadataInfo.Name;
            DisplayName = metadataInfo.DisplayName;
            Summary = metadataInfo.Summary;
            Description = metadataInfo.Description;
            Author = metadataInfo.Author;
            Icon = metadataInfo.Icon;
            MetadataType = metadataInfo.MetadataType;
            LicenceTerms = metadataInfo.LicenceTerms;
        }
    }
}
