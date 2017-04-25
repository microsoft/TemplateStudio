using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Mvvm;
using Microsoft.Templates.UI.Resources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Microsoft.Templates.UI.ViewModels
{
    public class ProjectSetupViewModel : Observable
    {

        private string _projectTypesHeader;
        public string ProjectTypesHeader
        {
            get { return _projectTypesHeader; }
            set { SetProperty(ref _projectTypesHeader, value); }
        }

        private string _frameworkHeader;
        public string FrameworkHeader
        {
            get { return _frameworkHeader; }
            set { SetProperty(ref _frameworkHeader, value); }
        }

        private MetadataInfoViewModel _selectedProjectType;
        public MetadataInfoViewModel SelectedProjectType
        {
            get { return _selectedProjectType; }
            set
            {
                var orgframework = _selectedFramework;

                SetProperty(ref _selectedProjectType, value);
                if (value != null)
                {
                    LoadFrameworks(value, orgframework);
                    MainViewModel.Current.AlertProjectSetupChanged();
                }

                MainViewModel.Current.RebuildLicenses();
            }
        }

        private MetadataInfoViewModel _selectedFramework;
        public MetadataInfoViewModel SelectedFramework
        {
            get { return _selectedFramework; }
            set
            {
                SetProperty(ref _selectedFramework, value);
                if (value != null)
                {
                    MainViewModel.Current.AlertProjectSetupChanged();
                }

                MainViewModel.Current.RebuildLicenses();
            }
        }

        public ObservableCollection<MetadataInfoViewModel> ProjectTypes { get; } = new ObservableCollection<MetadataInfoViewModel>();
        public ObservableCollection<MetadataInfoViewModel> Frameworks { get; } = new ObservableCollection<MetadataInfoViewModel>();

        public async Task IniatializeAsync()
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
                }
                else
                {
                    MainViewModel.Current.NoContentVisibility = Visibility.Visible;
                }

                ProjectTypesHeader = String.Format(StringRes.GroupProjectTypeHeader_SF, ProjectTypes.Count);

                await Task.CompletedTask;
            }
        }

        private void LoadFrameworks(MetadataInfoViewModel projectType, MetadataInfoViewModel orgFramework)
        {
            var projectFrameworks = GetSupportedFx(projectType.Name);

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

            FrameworkHeader = String.Format(StringRes.GroupFrameworkHeader_SF, Frameworks.Count);
        }

        private IEnumerable<string> GetSupportedFx(string projectType)
        {
            return GenContext.ToolBox.Repo.GetAll()
                .Where(t => t.GetProjectType() == projectType)
                .SelectMany(t => t.GetFrameworkList())
                .Distinct();
        }
    }
}
