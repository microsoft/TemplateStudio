using System;
using System.Windows.Input;
using Windows.ApplicationModel;
using Param_RootNamespace.Services;

namespace Param_ItemNamespace.ViewModels
{
    public class SettingsPageViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        // TODO UWPTemplates: Add other settings as necessary. For help see https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/pages/settings.md
        private int _selectedTheme;
        public int SelectedTheme
        {
            get { return _selectedTheme; }
            set { Set(ref _selectedTheme, value); }
        }

        private string _appDescription;
        public string AppDescription
        {
            get { return _appDescription; }
            set { Set(ref _appDescription, value); }
        }

        public ICommand SelectionChangeCommand { get; private set; }

        public SettingsPageViewModel()
        {
            SelectionChangeCommand = new RelayCommand(async () => { await ThemeSelectorService.SetThemeAsync(SelectedTheme); });
        }

        public void Initialize()
        {
            SelectedTheme = ThemeSelectorService.GetTheme();
            AppDescription = GetAppDescription();
        }

        private string GetAppDescription()
        {
            var package = Package.Current;
            var packageId = package.Id;
            var version = packageId.Version;

            return $"{package.DisplayName} - {version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
        }
    }
}
