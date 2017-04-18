using Microsoft.Templates.Core.Gen;
using Microsoft.Templates.Core.Mvvm;
using Microsoft.Templates.UI.Resources;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        private MetadataInfoViewModel _selectedProjectType;
        public MetadataInfoViewModel SelectedProjectType
        {
            get { return _selectedProjectType; }
            set { SetProperty(ref _selectedProjectType, value); }
        }

        public ObservableCollection<MetadataInfoViewModel> ProjectTypes { get; } = new ObservableCollection<MetadataInfoViewModel>();

        private ICommand _itemClickCommand;
        public ICommand ItemClickCommand => _itemClickCommand ?? new RelayCommand(OnItemClickCommand);

        private void OnItemClickCommand()
        {
        }

        public async Task IniatializeAsync()
        {
            ProjectTypes.Clear();
            var projectTypes = GenContext.ToolBox.Repo.GetProjectTypes().Select(m => new MetadataInfoViewModel(m)).ToList();            
            foreach (var projectType in projectTypes.Where(p => !string.IsNullOrEmpty(p.Description)))
            {
                ProjectTypes.Add(projectType);
            }
            SelectedProjectType = ProjectTypes.First();
            ProjectTypesHeader = String.Format(StringRes.GroupProjectTypeHeader_SF, ProjectTypes.Count);
            await Task.CompletedTask;
        }
    }
}
