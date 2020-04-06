namespace Param_RootNamespace.Views
{
    public sealed partial class ShellPage : Page, INotifyPropertyChanged
    {
        private void Initialize()
        {
//^^
//{[{
            NavigationService.OnCurrentPageCanGoBackChanged += OnCurrentPageCanGoBackChanged;
//}]}
            navigationView.BackRequested += OnBackRequested;
        }
//^^
//{[{
        private void OnCurrentPageCanGoBackChanged(object sender, bool currentPageCanGoBack)
            => IsBackEnabled = NavigationService.CanGoBack || currentPageCanGoBack;

//}]}
        private void Frame_Navigated(object sender, NavigationEventArgs e)
        {
        }
    }
}