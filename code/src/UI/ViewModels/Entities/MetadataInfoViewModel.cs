// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System.Windows;

using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Mvvm;
using Microsoft.Templates.UI.Views;

namespace Microsoft.Templates.UI.ViewModels
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

            infoView.ShowDialog();
            MainViewModel.Current.InfoShapeVisibility = Visibility.Collapsed;
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
            LicenseTerms = metadataInfo.LicenseTerms;
            MetadataType = metadataInfo.MetadataType;
            Name = metadataInfo.Name;
            Summary = metadataInfo.Summary;
        }
    }
}
