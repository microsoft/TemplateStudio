namespace Param_RootNamespace.Views
{
    public sealed partial class ShellPage : Page, INotifyPropertyChanged
    {
        public ShellPage()
        {
        }

//{[{

        private void OnUserProfile(object sender, RoutedEventArgs e)
        {
            MenuNavigationHelper.OpenInRightPane(typeof(SettingsPage));
        }
//}]}
    }
}
