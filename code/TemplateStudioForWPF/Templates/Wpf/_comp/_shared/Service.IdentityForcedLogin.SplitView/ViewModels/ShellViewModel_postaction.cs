namespace Param_RootNamespace.ViewModels
{
    public class ShellViewModel : System.ComponentModel.INotifyPropertyChanged
    {
//^^
//{[{
        private readonly IUserDataService _userDataService;
//}]}
        private HamburgerMenuItem _selectedMenuItem;

        public ShellViewModel(/*{[{*/IUserDataService userDataService/*}]}*/)
        {
//^^
//{[{
            _userDataService = userDataService;
//}]}
        }

        private void OnLoaded()
        {
//^^
//{[{
            _userDataService.UserDataUpdated += OnUserDataUpdated;
            var user = _userDataService.GetUser();
            var userMenuItem = new HamburgerMenuImageItem()
            {
                Thumbnail = user.Photo,
                Label = user.Name,
                Command = new System.Windows.Input.ICommand(OnUserItemSelected)
            };

            OptionMenuItems.Insert(0, userMenuItem);
//}]}
        }

        private void OnUnloaded()
        {
//^^
//{[{
            _userDataService.UserDataUpdated -= OnUserDataUpdated;
            var userMenuItem = OptionMenuItems.OfType<HamburgerMenuImageItem>().FirstOrDefault();
            if (userMenuItem != null)
            {
                OptionMenuItems.Remove(userMenuItem);
            }
//}]}
        }
//{[{

        private void OnUserDataUpdated(object sender, UserViewModel user)
        {
            var userMenuItem = OptionMenuItems.OfType<HamburgerMenuImageItem>().FirstOrDefault();
            if (userMenuItem != null)
            {
                userMenuItem.Label = user.Name;
                userMenuItem.Thumbnail = user.Photo;
            }
        }
//}]}
        private void OnOptionsMenuItemInvoked()
            => NavigateTo(SelectedOptionsMenuItem.TargetPageType);
//{[{

        private void OnUserItemSelected()
            => NavigateTo(typeof(SettingsViewModel));
//}]}
    }
}
