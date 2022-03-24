//{[{
using System.Collections.Generic;
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
        private UserViewModel _user;
        private IUserDataService _userDataService;
        private IIdentityService _identityService;
//}]}

        public ICommand ItemInvokedCommand { get; }
//{[{

        public ICommand LoadedCommand { get; }

        public DelegateCommand LoginCommand { get; }

        public ICommand UserProfileCommand { get; }

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

        public UserViewModel User
        {
            get { return _user; }
            set { SetProperty(ref _user, value); }
        }
//}]}
        public ShellViewModel(/*{[{*/IUserDataService userDataService, IIdentityService identityService/*}]}*/)
        {
            _navigationService = navigationServiceInstance;
//^^
//{[{
            _userDataService = userDataService;
            _identityService = identityService;
            LoadedCommand = new DelegateCommand(OnLoaded);
            LoginCommand = new DelegateCommand(OnLogin, () => !IsBusy);
            UserProfileCommand = new DelegateCommand(OnUserProfile);
//}]}
            ItemInvokedCommand = new DelegateCommand<WinUI.NavigationViewItemInvokedEventArgs>(OnItemInvoked);
        }

        public void Initialize(Frame frame, WinUI.NavigationView navigationView)
        {
//^^
//{[{
            _identityService.LoggedIn += OnLoggedIn;
            _identityService.LoggedOut += OnLoggedOut;
            _userDataService.UserDataUpdated += OnUserDataUpdated;
            IsLoggedIn = _identityService.IsLoggedIn();
//}]}
        }
//{[{

        private async void OnLoaded()
        {
            User = await _userDataService.GetUserAsync();
        }

        private void OnUserDataUpdated(object sender, UserViewModel userData)
        {
            User = userData;
        }

        private void OnLoggedIn(object sender, EventArgs e)
        {
            IsLoggedIn = true;
        }

        private void OnLoggedOut(object sender, EventArgs e)
        {
            IsLoggedIn = false;
            _navigationService.Navigate(PageTokens.Param_HomeNamePage, null);
            _navigationService.ClearHistory();
            IsBackEnabled = false;
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

        private void OnUserProfile()
        {
            _navigationService.Navigate(PageTokens.SettingsPage, null);
        }
//}]}
    }
}
