using AzureMobileAuthPoc.Helpers;
using AzureMobileAuthPoc.Services;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Security.Credentials;
using Windows.UI.Xaml;

namespace AzureMobileAuthPoc.ViewModels
{
    public class Page1ViewModel : Observable
    {
        private AuthenticationService _authService;

        public Page1ViewModel()
        {
            _authService = new AuthenticationService();

            LoginCommand = new RelayCommand(async () => await LoginAsync());
            LogoutCommand = new RelayCommand(async () => await LogoutAsync());
        }

        public ICommand LoginCommand;

        public ICommand LogoutCommand;

        private string _message;

        public string Message
        {
            get => _message;
            set => Set(ref _message, value);
        }
        
        private async Task LoginAsync()
        {
            Message = await _authService.LoginAsync();
        }

        private async Task LogoutAsync()
        {
            Message = await _authService.LogoutAsync();
        }
    }
}
