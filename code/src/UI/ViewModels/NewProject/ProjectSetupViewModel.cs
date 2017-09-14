// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Mvvm;
using Microsoft.Templates.UI.Resources;

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
                var orgFramework = _selectedFramework;
                var orgProjectType = _selectedProjectType;

                SetProperty(ref _selectedProjectType, value);

                if (value != null)
                {
                    LoadFrameworks(value, orgFramework);

                    if (orgProjectType != null && orgProjectType != value)
                    {
                        MainViewModel.Current.AlertProjectSetupChanged();
                    }
                }

                MainViewModel.Current.Licenses.RebuildLicenses(MainViewModel.Current.CreateUserSelection());
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

                MainViewModel.Current.Licenses.RebuildLicenses(MainViewModel.Current.CreateUserSelection());
            }
        }

        public ObservableCollection<MetadataInfoViewModel> ProjectTypes { get; } = new ObservableCollection<MetadataInfoViewModel>();
        public ObservableCollection<MetadataInfoViewModel> Frameworks { get; } = new ObservableCollection<MetadataInfoViewModel>();

        public async Task InitializeAsync(bool force = false)
        {
            if (SelectedProjectType == null || force)
            {
                ProjectTypes.Clear();

                var projectTypes = GenContext.ToolBox.Repo.GetProjectTypes();

                if (projectTypes.Any())
                {
                    var data = projectTypes.Select(m => new MetadataInfoViewModel(m)).ToList();

                    foreach (var projectType in data.Where(p => !string.IsNullOrEmpty(p.Description)))
                    {
                        ProjectTypes.Add(projectType);
                    }

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

        private void LoadFrameworks(MetadataInfoViewModel projectType, MetadataInfoViewModel orgFramework)
        {
            var projectFrameworks = GenComposer.GetSupportedFx(projectType.Name);
            var targetFrameworks = GenContext.ToolBox.Repo.GetFrameworks()
                                                                .Where(m => projectFrameworks.Contains(m.Name))
                                                                .Select(m => new MetadataInfoViewModel(m))
                                                                .ToList();

            Frameworks.Clear();

            foreach (var framework in targetFrameworks)
            {
                Frameworks.Add(framework);
            }

            SelectedFramework = Frameworks.FirstOrDefault(f => f.Name == orgFramework?.Name);

            if (SelectedFramework == null)
            {
                SelectedFramework = Frameworks.FirstOrDefault();
            }

            FrameworkHeader = string.Format(StringRes.GroupFrameworkHeader_SF, Frameworks.Count);
            MainViewModel.Current.UpdateCanGoForward(true);
        }
    }
}
