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

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Mvvm;
using Microsoft.Templates.UI.Resources;
using Microsoft.Templates.UI.Views;

namespace Microsoft.Templates.UI.ViewModels
{
    public class InformationViewModel : Observable
    {
        public static InformationViewModel Current;
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
        
        public ObservableCollection<SummaryLicenseViewModel> LicenseTerms { get; } = new ObservableCollection<SummaryLicenseViewModel>();

        private Visibility _licensesVisibility = Visibility.Collapsed;
        public Visibility LicensesVisibility
        {
            get => _licensesVisibility;
            set => SetProperty(ref _licensesVisibility, value);
        }

        private Visibility _dependenciesVisibility = Visibility.Collapsed;
        public Visibility DependenciesVisibility
        {
            get => _dependenciesVisibility;
            set => SetProperty(ref _dependenciesVisibility, value);
        }

        private ICommand _okCommand;
        public ICommand OkCommand => _okCommand ?? (_okCommand = new RelayCommand(OnOk));
        
        public InformationViewModel(InformationWindow infoWindow)
        {            
            _infoWindow = infoWindow;
            Current = this;
        }

        private IEnumerable<DependencyInfoViewModel> _dependencyItems;
        public IEnumerable<DependencyInfoViewModel> DependencyItems
        {
            get => _dependencyItems;
            set => SetProperty(ref _dependencyItems, value);
        }

        public void Initialize(TemplateInfoViewModel template)
        {
            Author = template.Author;
            DependencyItems = template.DependencyItems;
            InformationMD = template.Description;
            Name = template.Name;
            Version = template.Version;

            DependenciesVisibility = DependencyItems.Any() ? Visibility.Visible : Visibility.Collapsed;
            InformationType = GetInformationType(template.TemplateType.ToString());
            
            if (template.LicenseTerms != null && template.LicenseTerms.Any())
            {
                LicenseTerms.AddRange(template.LicenseTerms.Select(l => new SummaryLicenseViewModel(l)));

                LicensesVisibility = Visibility.Visible;
            }
            else
            {
                LicensesVisibility = Visibility.Collapsed;
            }
            
        }        

        public void Initialize(MetadataInfoViewModel metadataInfo)
        {
            Author = metadataInfo.Author;
            Name = metadataInfo.DisplayName;
            InformationMD = metadataInfo.Description;
            InformationType = GetInformationType(metadataInfo.MetadataType);
            
            if(metadataInfo.LicenseTerms != null && metadataInfo.LicenseTerms.Any())
            {
                LicenseTerms.AddRange(metadataInfo.LicenseTerms.Select(l => new SummaryLicenseViewModel(l)));

                LicensesVisibility = Visibility.Visible;
            }
            else
            {
                LicensesVisibility = Visibility.Collapsed;
            }

            
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
