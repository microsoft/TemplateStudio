//{[{
using System.Collections.Generic;
using Param_RootNamespace.Services;
using Param_RootNamespace.Core.Services;
using Param_RootNamespace.Core.Helpers;
//}]}

namespace Param_RootNamespace.ViewModels
{
    public class ShellViewModel : ViewModelBase
    {
        private WinUI.NavigationViewItem _selected;
//{[{
        private UserViewModel _user;
        private bool _isBusy;
        private bool _isLoggedIn;
        private bool _isAuthorized;
        private IIdentityService _identityService;
        private IUserDataService _userDataService;
//}]}
        public ICommand ItemInvokedCommand { get; }
//{[{
        public DelegateCommand UserProfileCommand { get; }

        public UserViewModel User
        {
            get { return _user; }
            set { SetProperty(ref _user, value); }
        }

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                SetProperty(ref _isBusy, value);
                UserProfileCommand.RaiseCanExecuteChanged();
            }
        }

        public bool IsLoggedIn
        {
            get { return _isLoggedIn; }
            set { SetProperty(ref _isLoggedIn, value); }
        }

        public bool IsAuthorized
        {
            get { return _isAuthorized; }
            set { SetProperty(ref _isAuthorized, value); }
        }

//}]}
//{--{
        public ShellViewModel(INavigationService navigationServiceInstance)
        {
            _navigationService = navigationServiceInstance;
            ItemInvokedCommand = new DelegateCommand<WinUI.NavigationViewItemInvokedEventArgs>(OnItemInvoked);
        }
//}--}
//{[{
        public ShellViewModel(INavigationService navigationServiceInstance, IIdentityService identityService, IUserDataService userDataService)
        {
            _navigationService = navigationServiceInstance;
            _identityService = identityService;
            _userDataService = userDataService;
            ItemInvokedCommand = new DelegateCommand<WinUI.NavigationViewItemInvokedEventArgs>(OnItemInvoked);
            UserProfileCommand = new DelegateCommand(OnUserProfile);
        }
//}]}
        public void Initialize(Frame frame, WinUI.NavigationView navigationView)
        {
//^^
//{[{
            _identityService.LoggedIn += OnLoggedIn;
            _identityService.LoggedOut += OnLoggedOut;
            _userDataService.UserDataUpdated += OnUserDataUpdated;
//}]}
        }
//{[{

        public override async void OnNavigatedTo(NavigatedToEventArgs e, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(e, viewModelState);
            IsLoggedIn = _identityService.IsLoggedIn();
            IsAuthorized = IsLoggedIn && _identityService.IsAuthorized();
            User = await _userDataService.GetUserAsync();
        }

        private void OnUserDataUpdated(object sender, UserViewModel user)
        {
            User = user;
        }

        private void OnLoggedIn(object sender, EventArgs e)
        {
            IsLoggedIn = true;
            IsAuthorized = IsLoggedIn && _identityService.IsAuthorized();
            IsBusy = false;
        }

        private void OnLoggedOut(object sender, EventArgs e)
        {
            User = null;
            IsLoggedIn = false;
            IsAuthorized = false;
            CleanRestrictedPagesFromNavigationHistory();
            GoBackToLastUnrestrictedPage();
        }

        private void CleanRestrictedPagesFromNavigationHistory()
        {
            var app = App.Current as App;
            foreach (var pageToken in PageTokens.GetAll())
            {
                var page = app.GetPage(pageToken);
                var isRestricted = Attribute.IsDefined(page, typeof(Restricted));
                if (isRestricted)
                {
                    _navigationService.RemoveAllPages(pageToken);
                }
            }
        }

        private void GoBackToLastUnrestrictedPage()
        {

            var currentPage = _frame.Content as Page;
            var isCurrentPageRestricted = Attribute.IsDefined(currentPage.GetType(), typeof(Restricted));
            if (isCurrentPageRestricted)
            {
                _navigationService.GoBack();
            }
        }

        private async void OnUserProfile()
        {
            if (IsLoggedIn)
            {
                _navigationService.Navigate(PageTokens.SettingsPage, null);
            }
            else
            {
                IsBusy = true;
                var loginResult = await _identityService.LoginAsync();
                if (loginResult != LoginResultType.Success)
                {
                    await AuthenticationHelper.ShowLoginErrorAsync(loginResult);
                    IsBusy = false;
                }
            }
        }
//}]}
    }
}
