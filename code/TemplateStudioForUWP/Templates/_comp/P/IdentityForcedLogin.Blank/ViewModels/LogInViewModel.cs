using System;
using Param_RootNamespace.Core.Helpers;
using Param_RootNamespace.Core.Services;
using Param_RootNamespace.Helpers;
using Prism.Commands;
using Prism.Windows.Mvvm;

namespace Param_RootNamespace.ViewModels
{
    public class LogInViewModel : ViewModelBase
    {
        private IIdentityService _identityService;
        private string _statusMessage;
        private bool _isBusy;

        public string StatusMessage
        {
            get => _statusMessage;
            set => SetProperty(ref _statusMessage, value);
        }

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                SetProperty(ref _isBusy, value);
                LoginCommand.RaiseCanExecuteChanged();
            }
        }

        public DelegateCommand LoginCommand { get; }

        public LogInViewModel(IIdentityService identityService)
        {
            _identityService = identityService;
            LoginCommand = new DelegateCommand(OnLogin, () => !IsBusy);
        }

        private async void OnLogin()
        {
            IsBusy = true;
            StatusMessage = string.Empty;
            var loginResult = await _identityService.LoginAsync();
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
