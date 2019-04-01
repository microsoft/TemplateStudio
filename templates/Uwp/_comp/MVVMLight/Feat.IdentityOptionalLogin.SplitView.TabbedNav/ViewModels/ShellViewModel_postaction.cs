//{[{
using Param_RootNamespace.Core.Services;
using Param_RootNamespace.Core.Helpers;
//}]}

namespace Param_RootNamespace.ViewModels
{
    public class ShellViewModel : ViewModelBase
    {
        private ICommand _itemInvokedCommand;
//{[{
        private RelayCommand _userProfileCommand;
        private UserViewModel _user;
        private bool _isBusy;
        private bool _isLoggedIn;
        private bool _isAuthorized;

        private IdentityService IdentityService => Singleton<IdentityService>.Instance;

        private UserDataService UserDataService => Singleton<UserDataService>.Instance;
//}]}
        public ICommand ItemInvokedCommand => _itemInvokedCommand ?? (_itemInvokedCommand = new RelayCommand<WinUI.NavigationViewItemInvokedEventArgs>(OnItemInvoked));
//^^
//{[{
        public RelayCommand UserProfileCommand => _userProfileCommand ?? (_userProfileCommand = new RelayCommand(OnUserProfile, () => !IsBusy));

        public UserViewModel User
        {
            get { return _user; }
            set { Set(ref _user, value); }
        }

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                Set(ref _isBusy, value);
                UserProfileCommand.RaiseCanExecuteChanged();
            }
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

        public ShellViewModel()
        {
        }

        public void Initialize(Frame frame, WinUI.NavigationView navigationView, IList<KeyboardAccelerator> keyboardAccelerators)
        {
//^^
//{[{
            IdentityService.LoggedIn += OnLoggedIn;
            IdentityService.LoggedOut += OnLoggedOut;
            UserDataService.UserDataUpdated += OnUserDataUpdated;
//}]}
        }

        private async void OnLoaded()
        {
//^^
//{[{
            IsLoggedIn = IdentityService.IsLoggedIn();
            IsAuthorized = IsLoggedIn && IdentityService.IsAuthorized();
            User = await UserDataService.GetUserAsync();
//}]}
        }
//{[{

        private void OnUserDataUpdated(object sender, UserViewModel user)
        {
            User = user;
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
            foreach (var backStack in NavigationService.Frame.BackStack)
            {
                var isRestricted = Attribute.IsDefined(backStack.SourcePageType, typeof(Restricted));
                if (isRestricted)
                {
                    NavigationService.Frame.BackStack.Remove(backStack);
                }
            }

            var currentPage = NavigationService.Frame.Content as Page;
            var isCurrentPageRestricted = Attribute.IsDefined(currentPage.GetType(), typeof(Restricted));
            if (isCurrentPageRestricted)
            {
                NavigationService.GoBack();
            }
        }

        private async void OnUserProfile()
        {
            if (IsLoggedIn)
            {
                NavigationService.Navigate(typeof(SettingsViewModel).FullName);
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
