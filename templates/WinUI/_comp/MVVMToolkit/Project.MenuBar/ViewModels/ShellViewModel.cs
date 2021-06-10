using System;
using System.Windows;
using System.Windows.Input;
using Param_RootNamespace.Contracts.Services;
using Param_RootNamespace.Properties;

namespace Param_RootNamespace.ViewModels
{
    // You can show pages in different ways (update main view, navigate, right pane, new windows or dialog)
    // using the NavigationService, RightPaneService and WindowManagerService.
    // Read more about MenuBar project type here:
    // https://github.com/Microsoft/WindowsTemplateStudio/blob/release/docs/WinUI/projectTypes/menubar.md
    public class ShellViewModel : ObservableRecipient
    {
        private bool _isBackEnabled;
        private object _selected;
        private ICommand _menuFileExitCommand;

        public ICommand MenuFileExitCommand => _menuFileExitCommand ?? (_menuFileExitCommand = new RelayCommand(OnMenuFileExit));

        public INavigationService NavigationService { get; }

        public IRightPaneService RightPaneService { get; }

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

        public ShellViewModel(INavigationService navigationService)
        {
            NavigationService = navigationService;
            NavigationService.Navigated += OnNavigated;
        }

        private void OnNavigated(object sender, NavigationEventArgs e)
        {
            IsBackEnabled = NavigationService.CanGoBack;
        }

        private void OnMenuViewsMain() => NavigationService.NavigateTo(typeof(MainViewModel).FullName, null, true);

        private void OnMenuFileExit() => Application.Current.Exit();
    }
}
