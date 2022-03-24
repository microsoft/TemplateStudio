//{[{
using System.Windows.Controls;
using Param_RootNamespace.Helpers;
using Param_RootNamespace.Models;
using Param_RootNamespace.Core.Contracts.Services;
using Param_RootNamespace.Core.Helpers;
//}]}
namespace Param_RootNamespace.Views
{
    public partial class ShellWindow : MetroWindow, IShellWindow, INotifyPropertyChanged
    {
//^^
//{[{
        private readonly IIdentityService _identityService;
        private readonly IUserDataService _userDataService;
        private bool _isBusy;
        private bool _isLoggedIn;
        private bool _isAuthorized;
//}]}
        private bool _canGoBack;
//^^
//{[{
        public Func<HamburgerMenuItem, bool> IsPageRestricted { get; } =
            (menuItem) => Attribute.IsDefined(menuItem.TargetPageType, typeof(Restricted));

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

//}]}
        // TODO WTS: Change the icons and titles for all HamburgerMenuItems here.
        public ShellWindow(/*{[{*/IIdentityService identityService, IUserDataService userDataService/*}]}*/)
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

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
//^^
//{[{
            IsLoggedIn = _identityService.IsLoggedIn();
            IsAuthorized = IsLoggedIn && _identityService.IsAuthorized();
            var userMenuItem = new HamburgerMenuImageItem()
            {
                Command = new RelayCommand(OnUserItemSelected, () => !IsBusy)
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
                userMenuItem.Label = Properties.Resources.Shell_LogIn;
            }

            OptionMenuItems.Insert(0, userMenuItem);
//}]}
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
//^^
//{[{
            _userDataService.UserDataUpdated -= OnUserDataUpdated;
//}]}
        }
//{[{

        private void OnUserDataUpdated(object sender, UserData user)
        {
            var userMenuItem = OptionMenuItems.OfType<HamburgerMenuImageItem>().FirstOrDefault();
            if (userMenuItem != null)
            {
                userMenuItem.Label = user.Name;
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
            RemoveUserInformation();
            _navigationService.CleanNavigation();
            CanGoBack = false;
        }

        private void RemoveUserInformation()
        {
            var userMenuItem = OptionMenuItems.OfType<HamburgerMenuImageItem>().FirstOrDefault();
            if (userMenuItem != null)
            {
                userMenuItem.Thumbnail = ImageHelper.ImageFromAssetsFile("DefaultIcon.png");
                userMenuItem.Label = Properties.Resources.Shell_LogIn;
            }
        }
//}]}
        private void OnOptionsItemClick(object sender, ItemClickEventArgs args)
            => NavigateTo(SelectedOptionsMenuItem.TargetPageType);
//{[{

        private async void OnUserItemSelected()
        {
            if (!IsLoggedIn)
            {
                IsBusy = true;
                var loginResult = await _identityService.LoginAsync();
                if (loginResult != LoginResultType.Success)
                {
                    await AuthenticationHelper.ShowLoginErrorAsync(loginResult);
                    IsBusy = false;
                }
            }

            NavigateTo(typeof(SettingsPage));
        }
//}]}
    }
}
