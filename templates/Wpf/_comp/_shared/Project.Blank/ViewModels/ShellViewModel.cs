using System;
using System.Windows.Input;
using Param_RootNamespace.Contracts.Services;

namespace Param_RootNamespace.ViewModels
{
    public class ShellViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        private readonly INavigationService _navigationService;
        private System.Windows.Input.ICommand _goBackCommand;
        private System.Windows.Input.ICommand _unloadedCommand;

        public System.Windows.Input.ICommand GoBackCommand => _goBackCommand ?? (_goBackCommand = new System.Windows.Input.ICommand(OnGoBack, CanGoBack));

        public ICommand UnloadedCommand => _unloadedCommand ?? (_unloadedCommand = new System.Windows.Input.ICommand(OnUnloaded));

        public ShellViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            _navigationService.Navigated += OnNavigated;
        }

        public void OnUnloaded()
        {
            _navigationService.Navigated -= OnNavigated;
        }

        private bool CanGoBack()
            => _navigationService.CanGoBack;

        private void OnGoBack()
            => _navigationService.GoBack();

        private void OnNavigated(object sender, string viewModelName)
        {
            GoBackCommand.Param_CanExecuteChangedMethodName();
        }
    }
}
