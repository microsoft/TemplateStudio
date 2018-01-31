// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows;

using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Mvvm;
using Microsoft.Templates.UI.ViewModels.Common;
using Microsoft.Templates.UI.Views.NewProject;

namespace Microsoft.Templates.UI.ViewModels.NewProject
{
    public class MetadataInfoViewModel : CommonInfoViewModel
    {
        private MetadataInfo _metadataInfo;

        private string _displayName;

        public string DisplayName
        {
            get => _displayName;
            set => SetProperty(ref _displayName, value);
        }

        private MetadataType _metadataType;

        public MetadataType MetadataType
        {
            get => _metadataType;
            set => SetProperty(ref _metadataType, value);
        }

        private RelayCommand _showInfoCommand;

        public RelayCommand ShowInfoCommand => _showInfoCommand ?? (_showInfoCommand = new RelayCommand(() => { OnShowInfo(); }));

        private void OnShowInfo()
        {
            MainViewModel.Current.WizardStatus.InfoShapeVisibility = Visibility.Visible;
            var infoView = new InformationWindow(this, MainViewModel.Current.MainView);

            infoView.ShowDialog();
            MainViewModel.Current.WizardStatus.InfoShapeVisibility = Visibility.Collapsed;
        }

        public MetadataInfoViewModel(MetadataInfo metadataInfo)
        {
            if (metadataInfo == null)
            {
                return;
            }

            _metadataInfo = metadataInfo;

            Author = metadataInfo.Author;
            Description = metadataInfo.Description;
            DisplayName = metadataInfo.DisplayName;
            Icon = metadataInfo.Icon;
            Order = metadataInfo.Order;
            LicenseTerms = metadataInfo.LicenseTerms;
            MetadataType = metadataInfo.MetadataType;
            Name = metadataInfo.Name;
            Summary = metadataInfo.Summary;
        }

        public override string ToString()
        {
            return $"{DisplayName} - {Summary}";
        }
    }
}
