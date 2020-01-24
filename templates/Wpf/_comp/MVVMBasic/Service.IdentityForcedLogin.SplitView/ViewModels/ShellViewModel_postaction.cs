namespace Param_RootNamespace.ViewModels
{
    public class ShellViewModel : /*{[{*/IDisposable/*}]}*/
    {
//^^
//{[{
        private readonly IUserDataService _userDataService;
//}]}
        private HamburgerMenuItem _selectedMenuItem;
//^^
//{[{
        private ICommand _loadCommand;
//}]}
        private RelayCommand _goBackCommand;
//^^
//{[{
        public ICommand LoadCommand => _loadCommand ?? (_loadCommand = new RelayCommand(OnLoad));

//}]}
        public RelayCommand GoBackCommand => _goBackCommand ?? (_goBackCommand = new RelayCommand(OnGoBack, CanGoBack));

        public ShellViewModel(/*{[{*/IUserDataService userDataService/*}]}*/)
        {
//^^
//{[{
            _userDataService = userDataService;
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
            var user = _userDataService.GetUser();
            var userMenuItem = new HamburgerMenuImageItem()
            {
                Thumbnail = user.Photo,
                Label = user.Name,
                Command = new RelayCommand(OnUserItemSelected)
            };

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
//}]}
        private void OnOptionsMenuItemInvoked()
            => NavigateTo(SelectedOptionsMenuItem.TargetPageType);
//{[{

        private void OnUserItemSelected()
            => NavigateTo(typeof(SettingsViewModel));
//}]}
    }
}
