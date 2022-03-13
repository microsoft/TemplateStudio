//{[{
using Param_RootNamespace.Core.Services;
using Param_RootNamespace.Core.Helpers;
using Microsoft.Toolkit.Mvvm.Input;
//}]}

namespace Param_RootNamespace.ViewModels
{
    public class ShellViewModel : ObservableObject
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
            set { SetProperty(ref _user, value); }
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
            UserDataService.UserDataUpdated += OnUserDataUpdated;
//}]}
        }

        private async void OnLoaded()
        {
//^^
//{[{
            User = await UserDataService.GetUserAsync();
//}]}
        }
//{[{

        private void OnUserDataUpdated(object sender, UserViewModel userData)
        {
            User = userData;
        }

        private void OnLoggedOut(object sender, EventArgs e)
        {
            NavigationService.NavigationFailed -= Frame_NavigationFailed;
            NavigationService.Navigated -= Frame_Navigated;
            _navigationView.BackRequested -= OnBackRequested;
            UserDataService.UserDataUpdated -= OnUserDataUpdated;
            IdentityService.LoggedOut -= OnLoggedOut;
        }

        private void OnUserProfile()
        {
            NavigationService.Navigate<SettingsPage>();
        }
//}]}
    }
}
