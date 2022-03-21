//{[{
using Param_RootNamespace.Services;
using Param_RootNamespace.Core.Helpers;
using Param_RootNamespace.Core.Services;
//}]}

namespace Param_RootNamespace.ViewModels
{
    public class ShellViewModel : ViewModelBase
    {
//{[{
        private bool _isBusy;
        private bool _isLoggedIn;
        private string _statusMessage;
        private IIdentityService _identityService;
//}]}
        private IMenuNavigationService _menuNavigationService;
//^^
//{[{
        public DelegateCommand LoginCommand { get; }

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                SetProperty(ref _isBusy, value);
                LoginCommand.RaiseCanExecuteChanged();
            }
        }

        public bool IsLoggedIn
        {
            get { return _isLoggedIn; }
            set { SetProperty(ref _isLoggedIn, value); }
        }

        public string StatusMessage
        {
            get { return _statusMessage; }
            set { SetProperty(ref _statusMessage, value); }
        }
//}]}

        public ShellViewModel(/*{[{*/IIdentityService identityService/*}]}*/)
        {
//^^
//{[{
            _identityService = identityService;
            LoginCommand = new DelegateCommand(OnLogin, () => !IsBusy);
//}]}
            MenuFileExitCommand = new DelegateCommand(OnMenuFileExit);
        }
        public void Initialize(Frame frame, SplitView splitView, Frame rightFrame)
        {
//^^
//{[{
            _identityService.LoggedIn += OnLoggedIn;
            _identityService.LoggedOut += OnLoggedOut;
            IsLoggedIn = _identityService.IsLoggedIn();
//}]}
        }
//^^
//{[{

        private void OnLoggedIn(object sender, EventArgs e)
        {
            IsLoggedIn = true;
        }

        private void OnLoggedOut(object sender, EventArgs e)
        {
            IsLoggedIn = false;
            _menuNavigationService.CloseRightPane();
            _menuNavigationService.UpdateView(PageTokens.Param_HomeNamePage);
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
//}]}
    }
}
