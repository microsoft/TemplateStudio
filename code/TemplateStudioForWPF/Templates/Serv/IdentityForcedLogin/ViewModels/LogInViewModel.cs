using System;
using Param_RootNamespace.Core.Contracts.Services;
using Param_RootNamespace.Core.Helpers;
using Param_RootNamespace.Properties;

namespace Param_RootNamespace.ViewModels;

public class LogInViewModel : System.ComponentModel.INotifyPropertyChanged
{
    private readonly IIdentityService _identityService;
    private string _statusMessage;
    private bool _isBusy;
    private System.Windows.Input.ICommand _loginCommand;

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
            LoginCommand.Param_CanExecuteChangedMethodName();
        }
    }

    public System.Windows.Input.ICommand LoginCommand => _loginCommand ?? (_loginCommand = new System.Windows.Input.ICommand(OnLogin, () => !IsBusy));

    public LogInViewModel(IIdentityService identityService)
    {
        _identityService = identityService;
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
                return Resources.StatusUnauthorized;
            case LoginResultType.NoNetworkAvailable:
                return Resources.StatusNoNetworkAvailable;
            case LoginResultType.UnknownError:
                return Resources.StatusLoginFails;
            case LoginResultType.Success:
            case LoginResultType.CancelledByUser:
                return string.Empty;
            default:
                return string.Empty;
        }
    }
}
