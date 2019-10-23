using System;
using System.Windows;
using System.Windows.Input;
using Param_RootNamespace.Contracts.Services;
using Param_RootNamespace.Strings;

namespace Param_RootNamespace.ViewModels
{
    public class ShellViewModel : System.ComponentModel.INotifyPropertyChanged, IDisposable
    {
        private readonly INavigationService _navigationService;

        private System.Windows.Input.ICommand _goBackCommand;
        private ICommand _menuFileExitCommand;

        public System.Windows.Input.ICommand GoBackCommand => _goBackCommand ?? (_goBackCommand = new System.Windows.Input.ICommand(OnGoBack, CanGoBack));

        public ICommand MenuFileExitCommand => _menuFileExitCommand ?? (_menuFileExitCommand = new System.Windows.Input.ICommand(OnMenuFileExit));

        public ShellViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            _navigationService.Navigated += OnNavigated;
        }

        public void Dispose()
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
