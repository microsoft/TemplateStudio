namespace Param_RootNamespace.Views
{
    public sealed partial class ShellPage : Page, INotifyPropertyChanged
    {
        public ShellPage()
        {
        }
//^^
//{[{
        private void ShellMenuItemClick_Views_wts.ItemName(object sender, RoutedEventArgs e)
        {
            MenuNavigationHelper.UpdateView(typeof(wts.ItemNamePage));
        }

//}]}
        private void ShellMenuItemClick_File_Exit(object sender, RoutedEventArgs e)
        {
        }
    }
}
