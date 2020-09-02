//{[{
using System.Windows.Controls;
using Param_RootNamespace.Core.Contracts.Services;
using Param_RootNamespace.Core.Helpers;
//}]}
namespace Param_RootNamespace.ViewModels
{
    public class ShellViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        private readonly IRightPaneService _rightPaneService;
//{[{
        private readonly IIdentityService _identityService;

        private bool _isLoggedIn;
        private bool _isAuthorized;
//}]}
        private RelayCommand _goBackCommand;
        private ICommand _unloadedCommand;
//{[{

        public bool IsLoggedIn
        {
            get { return _isLoggedIn; }
            set { Param_Setter(ref _isLoggedIn, value); }
        }

        public bool IsAuthorized
        {
            get { return _isAuthorized; }
            set { Param_Setter(ref _isAuthorized, value); }
        }
//}]}
        public RelayCommand GoBackCommand => _goBackCommand ?? (_goBackCommand = new System.Windows.Input.ICommand(OnGoBack, CanGoBack));

        public ShellViewModel(/*{[{*/IIdentityService identityService/*}]}*/)
        {
//^^
//{[{
            _identityService = identityService;
            _identityService.LoggedIn += OnLoggedIn;
            _identityService.LoggedOut += OnLoggedOut;
//}]}
        }

        private void OnLoaded()
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
            _navigationService.NavigateTo(typeof(Param_HomeNameViewModel).FullName, null, true);
        }
//}]}
    }
}
