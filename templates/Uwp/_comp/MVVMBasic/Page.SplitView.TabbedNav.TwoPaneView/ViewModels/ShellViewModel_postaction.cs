namespace Param_RootNamespace.ViewModels
{
    public class ShellViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        public void Initialize(Frame frame, WinUI.NavigationView navigationView, IList<KeyboardAccelerator> keyboardAccelerators)
        {
//^^
//{[{
            NavigationService.OnCurrentPageCanGoBackChanged += OnCurrentPageCanGoBackChanged;
//}]}
            _navigationView.BackRequested += OnBackRequested;
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