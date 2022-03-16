//{[{
using Param_RootNamespace.Helpers;
using Param_RootNamespace.Models;
//}]}
namespace Param_RootNamespace.Views
{
    public partial class ShellWindow : MetroWindow, IShellWindow, INotifyPropertyChanged
    {
//^^
//{[{
        private readonly IUserDataService _userDataService;
//}]}
        private bool _canGoBack;

        public ShellWindow(/*{[{*/IUserDataService userDataService/*}]}*/)
        {
//^^
//{[{
            _userDataService = userDataService;
//}]}
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
//^^
//{[{
            _userDataService.UserDataUpdated += OnUserDataUpdated;
            var user = _userDataService.GetUser();
            var userMenuItem = new HamburgerMenuImageItem()
            {
                Thumbnail = user.Photo,
                Label = user.Name,
                Command = new RelayCommand(OnUserItemSelected)
            };

            OptionMenuItems.Insert(0, userMenuItem);
//}]}
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
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

        private void OnUserItemSelected()
            => NavigateTo(typeof(SettingsPage));

        private void OnUserDataUpdated(object sender, UserData user)
        {
            var userMenuItem = OptionMenuItems.OfType<HamburgerMenuImageItem>().FirstOrDefault();
            if (userMenuItem != null)
            {
                userMenuItem.Label = user.Name;
                userMenuItem.Thumbnail = user.Photo;
            }
        }
//}]}
        private void OnOptionsItemClick(object sender, ItemClickEventArgs args)
            => NavigateTo(SelectedOptionsMenuItem.TargetPageType);
    }
}
