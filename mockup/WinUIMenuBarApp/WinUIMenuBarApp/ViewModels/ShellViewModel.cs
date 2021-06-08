
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Navigation;

using WinUIMenuBarApp.Contracts.Services;

namespace WinUIMenuBarApp.ViewModels
{
    // You can show pages in different ways (update main view, navigate, right pane, new windows or dialog)
    // using the NavigationService, RightPaneService and WindowManagerService.
    // Read more about MenuBar project type here:
    // https://github.com/Microsoft/WindowsTemplateStudio/blob/release/docs/WinUI/projectTypes/menubar.md
    public class ShellViewModel : ObservableRecipient
    {
        private bool _isBackEnabled;
        private object _selected;
        private ICommand _menuViewsMainCommand;
        private ICommand _menuViewsAppInfoCommand;
        private ICommand _menuViewsWebViewCommand;
        private ICommand _menuFilesSettingsCommand;
        private ICommand _menuFileExitCommand;

        public ICommand MenuViewsMainCommand => _menuViewsMainCommand ?? (_menuViewsMainCommand = new RelayCommand(OnMenuViewsMain));

        public ICommand MenuViewsAppInfoCommand => _menuViewsAppInfoCommand ?? (_menuViewsAppInfoCommand = new RelayCommand(OnMenuViewsAppInfo));

        public ICommand MenuViewsWebViewCommand => _menuViewsWebViewCommand ?? (_menuViewsWebViewCommand = new RelayCommand(OnMenuViewsWebView));

        public ICommand MenuFileSettingsCommand => _menuFilesSettingsCommand ?? (_menuFilesSettingsCommand = new RelayCommand(OnMenuFileSettings));

        public ICommand MenuFileExitCommand => _menuFileExitCommand ?? (_menuFileExitCommand = new RelayCommand(OnMenuFileExit));

        public INavigationService NavigationService { get; }

        public IRightPaneService RightPaneService { get; }

        public IWindowManagerService WindowManagerService { get; }

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

        public ShellViewModel(INavigationService navigationService, IRightPaneService rightPaneService, IWindowManagerService windowManagerService)
        {
            NavigationService = navigationService;
            NavigationService.Navigated += OnNavigated;
            RightPaneService = rightPaneService;
            WindowManagerService = windowManagerService;
        }

        private void OnNavigated(object sender, NavigationEventArgs e)
        {
            IsBackEnabled = NavigationService.CanGoBack;
        }

        private void OnMenuViewsMain() => NavigationService.NavigateTo(typeof(MainViewModel).FullName, null, true);
        
        private void OnMenuViewsAppInfo() => WindowManagerService.OpenInDialogAsync(typeof(AppInfoViewModel).FullName);

        private void OnMenuViewsWebView() => NavigationService.NavigateTo(typeof(WebViewViewModel).FullName, null, true);

        private void OnMenuFileSettings() => RightPaneService.OpenInRightPane(typeof(SettingsViewModel).FullName);

        private void OnMenuFileExit() => Application.Current.Exit();
    }
}
