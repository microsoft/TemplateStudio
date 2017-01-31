using Microsoft.Templates.Core;
using Microsoft.TemplateEngine.Abstractions;

namespace Microsoft.Templates.Wizard.ViewModels
{
    public class ProjectTypeViewModel : ObservableBase
    {
        private string _projectType;
        public string ProjectType
        {
            get { return _projectType; }
            set { SetProperty(ref _projectType, value); }
        }

        public ProjectTypeViewModel(string projectType)
        {
            //TODO: Recover info
            this.ProjectType = projectType;
        }
    }
}
