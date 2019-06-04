using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Templates.Core;
using Microsoft.Templates.UI.Mvvm;
using Microsoft.Templates.UI.Services;
using Microsoft.Templates.UI.ViewModels.Common;

namespace Microsoft.Templates.UI.ViewModels.NewProject
{
    public class TemplatesStepViewModel : Observable
    {
        private string _platform;
        private string _projectTypeName;
        private string _frameworkName;
        private TemplateType _templateType;
        private string _title;

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public ObservableCollection<TemplateGroupViewModel> Groups { get; } = new ObservableCollection<TemplateGroupViewModel>();

        public TemplatesStepViewModel(TemplateType templateType, string platform, string projectTypeName, string frameworkName, string title)
        {
            _templateType = templateType;
            _platform = platform;
            _projectTypeName = projectTypeName;
            _frameworkName = frameworkName;
            Title = title;
        }

        public void LoadData()
        {
            Groups.Clear();
            DataService.LoadTemplatesGroups(Groups, _templateType, _platform, _projectTypeName, _frameworkName);
        }

        public void ResetData(string projectTypeName, string frameworkName)
        {
            _projectTypeName = projectTypeName;
            _frameworkName = frameworkName;
            Groups.Clear();
            DataService.LoadTemplatesGroups(Groups, _templateType, _platform, _projectTypeName, _frameworkName);
        }

        public void ResetTemplatesCount()
        {
            foreach (var group in Groups)
            {
                foreach (var template in group.Items)
                {
                    template.ResetTemplateCount();
                }
            }
        }
    }
}
