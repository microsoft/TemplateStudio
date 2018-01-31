// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Mvvm;
using Microsoft.Templates.UI.Resources;
using Microsoft.Templates.UI.ViewModels.NewItem;

namespace Microsoft.Templates.UI.ViewModels.Common
{
    public class InformationViewModel : Observable
    {
        private static InformationViewModel _current;

        private Views.NewProject.InformationWindow _infoWindow;

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

        private string _helpText;

        public string HelpText
        {
            get => _helpText;
            set => SetProperty(ref _helpText, value);
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

        private IEnumerable<DependencyInfoViewModel> _dependencyItems;

        public IEnumerable<DependencyInfoViewModel> DependencyItems
        {
            get => _dependencyItems;
            set => SetProperty(ref _dependencyItems, value);
        }

        public InformationViewModel(Views.NewProject.InformationWindow infoWindow)
        {
            _infoWindow = infoWindow;
            _current = this;
        }

        public InformationViewModel(NewItem.TemplateInfoViewModel template)
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

        private void ComposeHelpText()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"{Name}");

            if (!string.IsNullOrEmpty(Version))
            {
                stringBuilder.AppendLine($"{StringRes.InfoModalVersion} {Version}");
            }

            if (!string.IsNullOrEmpty(Author))
            {
                stringBuilder.AppendLine($"{StringRes.InfoModalAuthor} {Author}");
            }

            if (LicenseTerms.Any())
            {
                stringBuilder.AppendLine($"{StringRes.InfoModalLicenses}");
                foreach (var license in LicenseTerms)
                {
                    stringBuilder.AppendLine($"{license.Text} {license.Url}");
                }
            }

            if (DependencyItems != null && DependencyItems.Any())
            {
                stringBuilder.AppendLine($"{StringRes.InfoModalDependencies}");
                foreach (var dependency in DependencyItems)
                {
                    stringBuilder.AppendLine($"{dependency.Name}");
                }
            }

            stringBuilder.AppendLine($"{InformationMD}");
            HelpText = stringBuilder.ToString();
        }

        internal void Initialize(TemplateInfoViewModel template)
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

            ComposeHelpText();
        }

        public void Initialize(NewProject.TemplateInfoViewModel template)
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

            ComposeHelpText();
        }

        public void Initialize(NewProject.MetadataInfoViewModel metadataInfo)
        {
            Author = metadataInfo.Author;
            Name = metadataInfo.DisplayName;
            InformationMD = metadataInfo.Description;
            InformationType = GetInformationType(metadataInfo.MetadataType.ToString());
            if (metadataInfo.LicenseTerms != null && metadataInfo.LicenseTerms.Any())
            {
                LicenseTerms.AddRange(metadataInfo.LicenseTerms.Select(l => new SummaryLicenseViewModel(l)));

                LicensesVisibility = Visibility.Visible;
            }
            else
            {
                LicensesVisibility = Visibility.Collapsed;
            }

            ComposeHelpText();
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
                case "ProjectType":
                    return StringRes.TemplateTypeProjectType;
                case "Framework":
                    return StringRes.TemplateTypeFramework;
                case "Page":
                    return StringRes.TemplateTypePage;
                case "Feature":
                    return StringRes.TemplateTypeFeature;
                default:
                    return string.Empty;
            }
        }
    }
}
