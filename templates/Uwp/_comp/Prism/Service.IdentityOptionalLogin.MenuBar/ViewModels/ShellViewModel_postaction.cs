//{[{
using System.Linq;
using Prism.Windows.Navigation;
using Param_RootNamespace.Core.Services;
using Param_RootNamespace.Core.Helpers;
//}]}

namespace Param_RootNamespace.ViewModels
{
    public class ShellViewModel : ViewModelBase
    {
//{[{
        private bool _isLoggedIn;
        private bool _isAuthorized;
        private INavigationService _navigationService;
        private IIdentityService _identityService;
//}]}
        private Frame _frame;
//^^
//{[{
        public bool IsLoggedIn
        {
            get { return _isLoggedIn; }
            set { SetProperty(ref _isLoggedIn, value); }
        }

        public bool IsAuthorized
        {
            get { return _isAuthorized; }
            set { SetProperty(ref _isAuthorized, value); }
        }
//}]}

        public ICommand MenuViewsParam_HomeNameCommand { get; }

        public ShellViewModel(/*{[{*/INavigationService navigationService, IIdentityService identityService/*}]}*/)
        {
            _menuNavigationService = menuNavigationService;
//^^
//{[{
            _navigationService = navigationService;
            _identityService = identityService;
//}]}
            MenuFileExitCommand = new DelegateCommand(OnMenuFileExit);
        }

        public void Initialize(Frame frame, SplitView splitView, Frame rightFrame)
        {
//^^
//{[{
            _identityService.LoggedIn += OnLoggedIn;
            _identityService.LoggedOut += OnLoggedOut;
            IsLoggedIn = _identityService.IsLoggedIn();
            IsAuthorized = IsLoggedIn && _identityService.IsAuthorized();
//}]}
        }

        private void OnMenuFileExit()
        {
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
            CleanRestrictedPagesFromNavigationHistory();
            GoBackToLastUnrestrictedPage();
        }

        private void CleanRestrictedPagesFromNavigationHistory()
        {
            var app = App.Current as App;
            foreach (var pageToken in PageTokens.GetAll())
            {
                var page = app.GetPage(pageToken);
                var isRestricted = Attribute.IsDefined(page, typeof(Restricted));
                if (isRestricted)
                {
                    _navigationService.RemoveAllPages(pageToken);
                }
            }
        }

        private void GoBackToLastUnrestrictedPage()
        {
            var currentPage = _frame.Content as Page;
            var isCurrentPageRestricted = Attribute.IsDefined(currentPage.GetType(), typeof(Restricted));
            if (isCurrentPageRestricted)
            {
                if (_navigationService.CanGoBack())
                {
                    _navigationService.GoBack();
                }
                else
                {
                    _menuNavigationService.UpdateView(PageTokens.Param_HomeNamePage);
                }
            }
        }
//}]}
    }
}
