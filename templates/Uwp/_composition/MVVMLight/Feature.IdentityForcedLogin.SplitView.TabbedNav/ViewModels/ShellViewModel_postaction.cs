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
        private ICommand _userProfileCommand;
        private UserViewModel _user;

        private IdentityService IdentityService => Singleton<IdentityService>.Instance;

        private UserDataService UserDataService => Singleton<UserDataService>.Instance;
//}]}
        public ICommand ItemInvokedCommand => _itemInvokedCommand ?? (_itemInvokedCommand = new RelayCommand<WinUI.NavigationViewItemInvokedEventArgs>(OnItemInvoked));
//^^
//{[{
        public ICommand UserProfileCommand => _userProfileCommand ?? (_userProfileCommand = new RelayCommand(OnUserProfile));

        public UserViewModel User
        {
            get { return _user; }
            set { Set(ref _user, value); }
        }
//}]}

        public ShellViewModel()
        {
        }

        public void Initialize(Frame frame, WinUI.NavigationView navigationView, IList<KeyboardAccelerator> keyboardAccelerators)
        {
//^^
//{[{
            IdentityService.LoggedOut += OnLoggedOut;
//}]}
        }

        private async void OnLoadedAsync()
        {
//^^
//{[{
            User = await UserDataService.GetUserFromCacheAsync();
            User = await UserDataService.GetUserFromGraphApiAsync();
            if (User == null)
            {
                User = UserDataService.GetDefaultUserData();
            }
//}]}
        }
//{[{

        private void OnLoggedOut(object sender, EventArgs e)
        {
            NavigationService.Navigated -= Frame_Navigated;
            _navigationView.BackRequested -= OnBackRequested;
            IdentityService.LoggedOut -= OnLoggedOut;
        }

        private void OnUserProfile()
        {
            NavigationService.Navigate(typeof(SettingsViewModel).FullName);
        }
//}]}
    }
}
