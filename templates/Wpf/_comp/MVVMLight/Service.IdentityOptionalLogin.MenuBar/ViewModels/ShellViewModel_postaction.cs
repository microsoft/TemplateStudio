//{[{
using System.Windows.Controls;
using Param_RootNamespace.Core.Contracts.Services;
using Param_RootNamespace.Core.Helpers;
//}]}
namespace Param_RootNamespace.ViewModels
{
    public class ShellViewModel : ViewModelBase
    {
//{[{
        private readonly IIdentityService _identityService;
//}]}
        private ICommand _unloadedCommand;
//^^
//{[{
        private bool _isLoggedIn;
        private bool _isAuthorized;
        private ICommand _loadedCommand;

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

        public ICommand LoadedCommand => _loadedCommand ?? (_loadedCommand = new RelayCommand(OnLoad));

//}]}
        public RelayCommand GoBackCommand => _goBackCommand ?? (_goBackCommand = new RelayCommand(OnGoBack, CanGoBack));

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
            _navigationService.Frame.CleanNavigation();
            _navigationService.NavigateTo(typeof(Param_HomeNameViewModel).FullName, null, true);
        }
//}]}
    }
}
