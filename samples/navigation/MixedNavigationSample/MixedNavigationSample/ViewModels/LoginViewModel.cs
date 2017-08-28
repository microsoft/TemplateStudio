using System;

using MixedNavigationSample.Helpers;
using System.Windows.Input;
using MixedNavigationSample.Services;
using Windows.UI.Xaml;

namespace MixedNavigationSample.ViewModels
{
    public class LoginViewModel : Observable
    {
        public ICommand LoginCommand { get; set; }

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(OnLogin);    
        }

        private void OnLogin()
        {
            NavigationService.Navigate<Views.ShellPage>();
            NavigationService.Navigate<Views.HomePage>();
        }
    }
}
