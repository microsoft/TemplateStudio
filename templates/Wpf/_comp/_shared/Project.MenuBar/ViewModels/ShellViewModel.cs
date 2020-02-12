using System;
using System.Windows;
using System.Windows.Input;
using Param_RootNamespace.Contracts.Services;
using Param_RootNamespace.Strings;

namespace Param_RootNamespace.ViewModels
{
    // You can show pages in different ways (update main view, navigate, right pane, new windows or dialog)
    // using the NavigationService, RightPaneService and WindowManagerService.
    // Read more about MenuBar project type here:
    // https://github.com/Microsoft/WindowsTemplateStudio/blob/master/docs/WPF/projectTypes/menubar.md
    public class ShellViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        private readonly INavigationService _navigationService;

        private System.Windows.Input.ICommand _goBackCommand;
        private ICommand _menuFileExitCommand;
        private ICommand _loadedCommand;
        private ICommand _unloadedCommand;

        public System.Windows.Input.ICommand GoBackCommand => _goBackCommand ?? (_goBackCommand = new System.Windows.Input.ICommand(OnGoBack, CanGoBack));

        public ICommand MenuFileExitCommand => _menuFileExitCommand ?? (_menuFileExitCommand = new System.Windows.Input.ICommand(OnMenuFileExit));

        public ICommand LoadedCommand => _loadedCommand ?? (_loadedCommand = new System.Windows.Input.ICommand(OnLoaded));

        public ICommand UnloadedCommand => _unloadedCommand ?? (_unloadedCommand = new System.Windows.Input.ICommand(OnUnloaded));

        public ShellViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            _navigationService.Navigated += OnNavigated;
        }

        private void OnLoaded()
        {
        }

        private void OnUnloaded()
        {
            _navigationService.Navigated -= OnNavigated;
        }

        private bool CanGoBack()
            => _navigationService.CanGoBack;

        private void OnGoBack()
            => _navigationService.GoBack();

        private void OnNavigated(object sender, string e)
            => GoBackCommand.Param_CanExecuteChangedMethodName();

        private void OnMenuFileExit()
            => Application.Current.Shutdown();
    }
}
