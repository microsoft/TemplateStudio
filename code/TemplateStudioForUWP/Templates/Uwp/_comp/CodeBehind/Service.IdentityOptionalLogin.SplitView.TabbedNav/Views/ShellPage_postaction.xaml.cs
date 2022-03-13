//{[{
using System.Linq;
using Param_RootNamespace.Models;
using Param_RootNamespace.Core.Services;
using Param_RootNamespace.Core.Helpers;
//}]}

namespace Param_RootNamespace.Views
{
    public sealed partial class ShellPage : Page, INotifyPropertyChanged
    {
        private WinUI.NavigationViewItem _selected;
//{[{
        private UserData _user;
        private bool _isBusy;
        private bool _isLoggedIn;
        private bool _isAuthorized;

        private IdentityService IdentityService => Singleton<IdentityService>.Instance;

        private UserDataService UserDataService => Singleton<UserDataService>.Instance;
//}]}
        public WinUI.NavigationViewItem Selected
        {
        }
//^^
//{[{
        public UserData User
        {
            get { return _user; }
            set { Set(ref _user, value); }
        }

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

        public ShellPage()
        {
        }

        private void Initialize()
        {
//^^
//{[{
            IdentityService.LoggedIn += OnLoggedIn;
            IdentityService.LoggedOut += OnLoggedOut;
            UserDataService.UserDataUpdated += OnUserDataUpdated;
//}]}
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
//^^
//{[{
            IsLoggedIn = IdentityService.IsLoggedIn();
            IsAuthorized = IsLoggedIn && IdentityService.IsAuthorized();
            User = await UserDataService.GetUserAsync();
//}]}
        }
//{[{

        private void OnUserDataUpdated(object sender, UserData userData)
        {
            User = userData;
        }

        private void OnLoggedIn(object sender, EventArgs e)
        {
            IsLoggedIn = true;
            IsAuthorized = IsLoggedIn && IdentityService.IsAuthorized();
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
            NavigationService.Frame.BackStack
                .Where(b => Attribute.IsDefined(b.SourcePageType, typeof(Restricted)))
                .ToList()
                .ForEach(page => NavigationService.Frame.BackStack.Remove(page));
        }

        private void GoBackToLastUnrestrictedPage()
        {
            var currentPage = NavigationService.Frame.Content as Page;
            var isCurrentPageRestricted = Attribute.IsDefined(currentPage.GetType(), typeof(Restricted));
            if (isCurrentPageRestricted)
            {
                NavigationService.GoBack();
            }
        }

        private async void OnUserProfile(object sender, RoutedEventArgs e)
        {
            if (IsLoggedIn)
            {
                NavigationService.Navigate<SettingsPage>();
            }
            else
            {
                IsBusy = true;
                var loginResult = await IdentityService.LoginAsync();
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
