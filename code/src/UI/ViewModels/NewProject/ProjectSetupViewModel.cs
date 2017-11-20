// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Templates.Core.Mvvm;
using Microsoft.Templates.UI.Resources;
using Microsoft.Templates.UI.Services;

namespace Microsoft.Templates.UI.ViewModels.NewProject
{
    public class ProjectSetupViewModel : Observable
    {
        private string _projectTypesHeader;

        public string ProjectTypesHeader
        {
            get => _projectTypesHeader;
            private set => SetProperty(ref _projectTypesHeader, value);
        }

        private string _frameworkHeader;

        public string FrameworkHeader
        {
            get => _frameworkHeader;
            private set => SetProperty(ref _frameworkHeader, value);
        }

        public string Platform { get; private set; }

        private MetadataInfoViewModel _selectedProjectType;

        public MetadataInfoViewModel SelectedProjectType
        {
            get => _selectedProjectType;
            set
            {
                if (value != null)
                {
                    DataService.LoadFrameworks(Frameworks, value.Name, Platform);
                    FrameworkHeader = string.Format(StringRes.GroupFrameworkHeader_SF, Frameworks.Count);
                    if (_selectedFramework != null)
                    {
                        SelectedFramework = Frameworks.FirstOrDefault(f => f.Name == _selectedFramework.Name);
                    }
                    else
                    {
                        SelectedFramework = Frameworks.FirstOrDefault();
                    }

                    var hasChanged = _selectedProjectType != null && _selectedProjectType.Name != value.Name;
                    SetProperty(ref _selectedProjectType, value);
                    UserSelectionService.SelectedProjectType = value;
                    if (hasChanged)
                    {
                        MainViewModel.Current.AlertProjectSetupChanged();
                    }

                    MainViewModel.Current.UpdateCanGoForward(true);
                    MainViewModel.Current.RebuildLicenses();
                }
            }
        }

        private MetadataInfoViewModel _selectedFramework;

        public MetadataInfoViewModel SelectedFramework
        {
            get => _selectedFramework;
            set
            {
                if (value != null)
                {
                    bool hasChanged = _selectedFramework != null && _selectedFramework.Name != value.Name;
                    SetProperty(ref _selectedFramework, value);
                    UserSelectionService.SelectedFramework = value;
                    if (hasChanged)
                    {
                        MainViewModel.Current.AlertProjectSetupChanged();
                    }

                    MainViewModel.Current.RebuildLicenses();
                }
            }
        }

        public ObservableCollection<MetadataInfoViewModel> ProjectTypes { get; } = new ObservableCollection<MetadataInfoViewModel>();

        public ObservableCollection<MetadataInfoViewModel> Frameworks { get; } = new ObservableCollection<MetadataInfoViewModel>();

        public async Task InitializeAsync(string platform, bool force = false)
        {
            Platform = platform;
            if (SelectedProjectType == null || force)
            {
                if (DataService.LoadProjectTypes(ProjectTypes, platform))
                {
                    SelectedProjectType = ProjectTypes.First();
                    MainViewModel.Current.WizardStatus.HasContent = true;
                }
                else
                {
                    MainViewModel.Current.WizardStatus.HasContent = false;
                }

                ProjectTypesHeader = string.Format(StringRes.GroupProjectTypeHeader_SF, ProjectTypes.Count);
                await Task.CompletedTask;
            }
        }
    }
}
