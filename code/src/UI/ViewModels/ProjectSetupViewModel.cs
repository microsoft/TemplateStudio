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

using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Mvvm;
using Microsoft.Templates.UI.Resources;

namespace Microsoft.Templates.UI.ViewModels
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

                MainViewModel.Current.RebuildLicenses();
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

        public async Task InitializeAsync()
        {
            MainViewModel.Current.Title = StringRes.ProjectSetupTitle;

            if (SelectedProjectType == null)
            {
                ProjectTypes.Clear();

                var projectTypes = GenContext.ToolBox.Repo.GetProjectTypes();

                if (projectTypes.Count() > 0)
                {
                    var data = projectTypes.Select(m => new MetadataInfoViewModel(m)).ToList();

                    foreach (var projectType in data.Where(p => !string.IsNullOrEmpty(p.Description)))
                    {
                        ProjectTypes.Add(projectType);
                    }

                    SelectedProjectType = ProjectTypes.First();
                    MainViewModel.Current.LoadedContentVisibility = Visibility.Visible;
                    MainViewModel.Current.LoadingContentVisibility = Visibility.Collapsed;
                    MainViewModel.Current.NextCommand.OnCanExecuteChanged();
                }
                else
                {
                    MainViewModel.Current.NoContentVisibility = Visibility.Visible;
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
            MainViewModel.Current.EnableGoForward();
        }
    }
}
