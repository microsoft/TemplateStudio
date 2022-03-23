//{[{
using Windows.UI.Xaml;
using Param_RootNamespace.Services;
//}]}
namespace Param_RootNamespace.ViewModels
{
    public class ShellViewModel : ViewModelBase
    {
//{[{
        private IBackNavigationService _backNavigationService;
//}]}
        private WinUI.NavigationViewItem _selected;
//{[{
        private bool _currentPageCanGoBack;
//}]}
        public ShellViewModel(/*{[{*/IBackNavigationService backNavigationService/*}]}*/)
        {
            _navigationService = navigationServiceInstance;
//{[{
            _backNavigationService = backNavigationService;
//}]}
        }

        public void Initialize(Frame frame, WinUI.NavigationView navigationView)
        {
//^^
//{[{
            _backNavigationService.Initialize(frame);
            _backNavigationService.OnCurrentPageCanGoBackChanged += OnCurrentPageCanGoBackChanged;
//}]}
            _navigationView.BackRequested += OnBackRequested;
        }

        private void OnBackRequested(WinUI.NavigationView sender, WinUI.NavigationViewBackRequestedEventArgs args)
        {
//{[{
            if (_currentPageCanGoBack)
            {
                if (_frame.Content is FrameworkElement element && element.DataContext is IBackNavigationHandler navigationHandler)
                {
                    navigationHandler.GoBack();
                    return;
                }
            }

//}]}
            _navigationService.GoBack();
        }
//^^
//{[{

        private void OnCurrentPageCanGoBackChanged(object sender, bool currentPageCanGoBack)
        {
            _currentPageCanGoBack = currentPageCanGoBack;
            IsBackEnabled = _navigationService.CanGoBack() || currentPageCanGoBack;
        }
//}]}
    }
}