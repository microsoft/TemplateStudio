using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Mvvm;
using System.Windows.Media.Imaging;

namespace Microsoft.Templates.Wizard.ViewModels
{
    public class ProjectInfoViewModel : Observable
    {
        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _description;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        private BitmapImage _icon;
        public BitmapImage Icon
        {
            get { return _icon; }
            set { SetProperty(ref _icon, value); }
        }

        public ProjectInfoViewModel(string name, ProjectInfo projectInfo)
        {
            this.Name = name;
            this.Description = projectInfo.Description;
            this.Icon = Extensions.CreateIcon(projectInfo.Icon);
        }
    }
}
