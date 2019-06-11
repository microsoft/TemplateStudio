using System;
using Param_RootNamespace.Core.Helpers;
using Param_RootNamespace.Core.Services;
using Param_RootNamespace.Helpers;

namespace Param_RootNamespace.ViewModels
{
    public class LogInViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        private string _statusMessage;
        private bool _isBusy;
        private RelayCommand _loginCommand;

        private IdentityService IdentityService => Singleton<IdentityService>.Instance;

        public string StatusMessage
        {
            get => _statusMessage;
            set => Param_Setter(ref _statusMessage, value);
        }

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                Param_Setter(ref _isBusy, value);
            }
        }

        public RelayCommand LoginCommand => _loginCommand ?? (_loginCommand = new RelayCommand(OnLogin, () => !IsBusy));

        public LogInViewModel()
        {
        }

        private async void OnLogin()
        {
            IsBusy = true;
            StatusMessage = string.Empty;
            var loginResult = await IdentityService.LoginAsync();
            StatusMessage = GetStatusMessage(loginResult);
            IsBusy = false;
        }

        private string GetStatusMessage(LoginResultType loginResult)
        {
            switch (loginResult)
            {
                case LoginResultType.Unauthorized:
                    return "StatusUnauthorized".GetLocalized();
                case LoginResultType.NoNetworkAvailable:
                    return "StatusNoNetworkAvailable".GetLocalized();
                case LoginResultType.UnknownError:
                    return "StatusLoginFails".GetLocalized();
                default:
                    return string.Empty;
            }
        }
    }
}
