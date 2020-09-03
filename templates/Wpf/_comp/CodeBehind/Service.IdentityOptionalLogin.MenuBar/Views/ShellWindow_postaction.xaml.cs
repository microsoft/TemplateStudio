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
        private readonly IRightPaneService _rightPaneService;
//{[{
        private readonly IIdentityService _identityService;

        private bool _isLoggedIn;
        private bool _isAuthorized;
//}]}
//^^
//{[{

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
        public ShellWindow(/*{[{*/IIdentityService identityService/*}]}*/)
        {
//^^
//{[{
            _identityService = identityService;
            _identityService.LoggedIn += OnLoggedIn;
            _identityService.LoggedOut += OnLoggedOut;
//}]}
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
//^^
//{[{
            IsLoggedIn = _identityService.IsLoggedIn();
            IsAuthorized = IsLoggedIn && _identityService.IsAuthorized();
//}]}
        }
//{[{

        private void OnLoggedIn(object sender, EventArgs e)
        {
            IsLoggedIn = true;
            IsAuthorized = IsLoggedIn && _identityService.IsAuthorized();
        }

        private void OnLoggedOut(object sender, EventArgs e)
        {
            IsLoggedIn = false;
            IsAuthorized = false;
            _navigationService.CleanNavigation();
            _navigationService.NavigateTo(typeof(Param_HomeNamePage), null, true);
        }
//}]}
    }
}
