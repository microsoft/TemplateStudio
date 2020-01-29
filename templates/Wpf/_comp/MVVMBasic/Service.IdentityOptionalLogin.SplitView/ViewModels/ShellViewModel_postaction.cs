//{[{
using System.Windows.Controls;
using Param_RootNamespace.Core.Contracts.Services;
using Param_RootNamespace.Core.Helpers;
//}]}
namespace Param_RootNamespace.ViewModels
{
    public class ShellViewModel : /*{[{*/IDisposable/*}]}*/
    {
//^^
//{[{
        private readonly IIdentityService _identityService;
        private readonly IUserDataService _userDataService;
//}]}
        private HamburgerMenuItem _selectedMenuItem;
//^^
//{[{
        private bool _isBusy;
        private bool _isLoggedIn;
        private bool _isAuthorized;
        private ICommand _loadCommand;
//}]}
        private RelayCommand _goBackCommand;
//^^
//{[{
        public bool IsBusy
        {
            get { return _isBusy; }
            set { Set(ref _isBusy, value); }
        }

        public bool IsLoggedIn
        {
            get { return _isLoggedIn; }
            set { Set(ref _isLoggedIn, value); }
        }

        public bool IsAuthorized
        {
            get { return _isAuthorized; }
            set { Set(ref _isAuthorized, value); }
        }

        public ICommand LoadCommand => _loadCommand ?? (_loadCommand = new RelayCommand(OnLoad));

//}]}
        public RelayCommand GoBackCommand => _goBackCommand ?? (_goBackCommand = new RelayCommand(OnGoBack, CanGoBack));

        public ShellViewModel(/*{[{*/IIdentityService identityService, IUserDataService userDataService/*}]}*/)
        {
//^^
//{[{
            _identityService = identityService;
            _userDataService = userDataService;
            _identityService.LoggedIn += OnLoggedIn;
            _identityService.LoggedOut += OnLoggedOut;
            _userDataService.UserDataUpdated += OnUserDataUpdated;
//}]}
        }
//{[{

        public void Dispose()
        {
            _userDataService.UserDataUpdated -= OnUserDataUpdated;
        }

        private void OnLoad()
        {
            IsLoggedIn = _identityService.IsLoggedIn();
            IsAuthorized = IsLoggedIn && _identityService.IsAuthorized();
            var userMenuItem = new HamburgerMenuImageItem()
            {
                Command = new RelayCommand(OnUserItemSelected)
            };
            if (IsAuthorized)
            {
                var user = _userDataService.GetUser();
                userMenuItem.Thumbnail = user.Photo;
                userMenuItem.Label = user.Name;
            }
            else
            {
                userMenuItem.Thumbnail = ImageHelper.ImageFromAssetsFile("DefaultIcon.png");
                userMenuItem.Label = Resources.Shell_LogIn;
            }

            OptionMenuItems.Insert(0, userMenuItem);
        }

        private void OnUserDataUpdated(object sender, UserViewModel user)
        {
            var userMenuItem = OptionMenuItems.OfType<HamburgerMenuImageItem>().FirstOrDefault();
            if (userMenuItem != null)
            {
                userMenuItem.Thumbnail = user.Photo;
            }
        }

        private void OnLoggedIn(object sender, EventArgs e)
        {
            IsLoggedIn = true;
            IsAuthorized = IsLoggedIn && _identityService.IsAuthorized();
            IsBusy = false;
        }

        private void OnLoggedOut(object sender, EventArgs e)
        {
            IsLoggedIn = false;
            IsAuthorized = false;
            var userMenuItem = OptionMenuItems.OfType<HamburgerMenuImageItem>().FirstOrDefault();
            if (userMenuItem != null)
            {
                userMenuItem.Thumbnail = ImageHelper.ImageFromAssetsFile("DefaultIcon.png");
                userMenuItem.Label = Resources.Shell_LogIn;
            }

            CleanRestrictedPagesFromNavigationHistory();
            GoBackToLastUnrestrictedPage();
        }

        private void CleanRestrictedPagesFromNavigationHistory()
        {
            //_navigationService.Frame.BackStack
            //    .Where(b => Attribute.IsDefined(b.SourcePageType, typeof(Restricted)))
            //    .ToList()
            //    .ForEach(page => NavigationService.Frame.BackStack.Remove(page));
        }

        private void GoBackToLastUnrestrictedPage()
        {
            var currentPage = _navigationService.Frame.Content as Page;
            var isCurrentPageRestricted = Attribute.IsDefined(currentPage.GetType(), typeof(Restricted));
            if (isCurrentPageRestricted)
            {
                _navigationService.GoBack();
            }
        }
//}]}
        private void OnOptionsMenuItemInvoked()
            => NavigateTo(SelectedOptionsMenuItem.TargetPageType);
//{[{

        private async void OnUserItemSelected()
        {
            if (IsLoggedIn)
            {
                NavigateTo(typeof(SettingsViewModel));
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
