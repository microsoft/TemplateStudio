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
            set => SetProperty(ref _projectTypesHeader, value);
        }

        private string _frameworkHeader;
        public string FrameworkHeader
        {
            get => _frameworkHeader;
            set => SetProperty(ref _frameworkHeader, value);
        }

        private MetadataInfoViewModel _selectedProjectType;
        public MetadataInfoViewModel SelectedProjectType
        {
            get => _selectedProjectType;
            set
            {
                if (value != null)
                {
                    DataService.LoadFrameworks(Frameworks, _selectedFramework.Name);
                    SelectedFramework = Frameworks.FirstOrDefault(f => f.Name == _selectedFramework?.Name);

                    if (SelectedFramework == null)
                    {
                        SelectedFramework = Frameworks.FirstOrDefault();
                    }

                    if (_selectedProjectType != null && _selectedProjectType != value)
                    {
                        MainViewModel.Current.AlertProjectSetupChanged();
                    }
                    SetProperty(ref _selectedProjectType, value);
                    FrameworkHeader = string.Format(StringRes.GroupFrameworkHeader_SF, Frameworks.Count);
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
                var orgframework = _selectedFramework;

                SetProperty(ref _selectedFramework, value);

                if (value != null && orgframework != null && orgframework != _selectedFramework)
                {
                    MainViewModel.Current.AlertProjectSetupChanged();
                }

                MainViewModel.Current.RebuildLicenses();
            }
        }

        public ObservableCollection<MetadataInfoViewModel> ProjectTypes { get; } = new ObservableCollection<MetadataInfoViewModel>();
        public ObservableCollection<MetadataInfoViewModel> Frameworks { get; } = new ObservableCollection<MetadataInfoViewModel>();

        public async Task InitializeAsync(bool force = false)
        {
            if (SelectedProjectType == null || force)
            {
                if (DataService.LoadProjectTypes(ProjectTypes))
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
