using System.Collections.Generic;
using System.Windows.Input;
using Windows.ApplicationModel;
using WTSPrism.Services;
using Prism.Commands;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;

namespace WTSPrism.ViewModels
{
    public class SettingsPageViewModel : ViewModelBase
    {
        // TODO WTS: Add other settings as necessary. For help see https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/pages/settings.md
        private bool isLightThemeEnabled;
        public bool IsLightThemeEnabled
        {
            get { return isLightThemeEnabled; }
            set { SetProperty(ref isLightThemeEnabled, value); }
        }

        private string appDescription;
        public string AppDescription
        {
            get { return appDescription; }
            set { SetProperty(ref appDescription, value); }
        }

        public ICommand SwitchThemeCommand { get; }

        public SettingsPageViewModel()
        {
            SwitchThemeCommand = new DelegateCommand(async () => { await ThemeSelectorService.SwitchThemeAsync(); });
        }

        public override void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);
            Initialize();
        }

        public void Initialize()
        {
            IsLightThemeEnabled = ThemeSelectorService.IsLightThemeEnabled;
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
