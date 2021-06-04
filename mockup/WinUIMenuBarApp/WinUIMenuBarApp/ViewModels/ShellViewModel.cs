
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Navigation;

using WinUIMenuBarApp.Contracts.Services;

namespace WinUIMenuBarApp.ViewModels
{
    public class ShellViewModel : ObservableRecipient
    {
        private bool _isBackEnabled;
        private object _selected;
        private ICommand _menuViewsMainCommand;
        private ICommand _menuFilesSettingsCommand;
        private ICommand _menuFileExitCommand;

        public ICommand MenuViewsMainCommand => _menuViewsMainCommand ?? (_menuViewsMainCommand = new RelayCommand(OnMenuViewsMain));

        public ICommand MenuFileSettingsCommand => _menuFilesSettingsCommand ?? (_menuFilesSettingsCommand = new RelayCommand(OnMenuFileSettings));

        public ICommand MenuFileExitCommand => _menuFileExitCommand ?? (_menuFileExitCommand = new RelayCommand(OnMenuFileExit));

        public INavigationService NavigationService { get; }

        public IMenuBarService MenuBarService { get; }

        public bool IsBackEnabled
        {
            get { return _isBackEnabled; }
            set { SetProperty(ref _isBackEnabled, value); }
        }

        public object Selected
        {
            get { return _selected; }
            set { SetProperty(ref _selected, value); }
        }

        public ShellViewModel(INavigationService navigationService, IMenuBarService menuBarService)
        {
            NavigationService = navigationService;
            NavigationService.Navigated += OnNavigated;
            MenuBarService = menuBarService;
        }

        private void OnNavigated(object sender, NavigationEventArgs e)
        {
            IsBackEnabled = NavigationService.CanGoBack;
        }

        private void OnMenuViewsMain() => MenuBarService.UpdateView(typeof(MainViewModel).FullName);

        private void OnMenuFileSettings() => MenuBarService.OpenInRightPane(typeof(SettingsViewModel).FullName);

        private void OnMenuFileExit() => MenuBarService.Exit();
    }
}
