using Microsoft.Templates.Core;
using Microsoft.Templates.Core.Mvvm;
using System.Windows.Media.Imaging;

namespace Microsoft.Templates.Wizard.ViewModels
{
    public class MetadataInfoViewModel : Observable
    {
        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _displayName;
        public string DisplayName
        {
            get => _displayName;
            set => SetProperty(ref _displayName, value);
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

        public MetadataInfoViewModel(string name, MetadataInfo metadataInfo)
        {
            this.Name = name;
            
            if (metadataInfo == null)
            {
                return;
            }

            this.DisplayName = metadataInfo.DisplayName;
            this.Description = metadataInfo.Description;
            this.Icon = Extensions.CreateIcon(metadataInfo.Icon);
        }
    }
}
