namespace Param_RootNamespace.Views
{
    public partial class ShellWindow : MetroWindow, IShellWindow, INotifyPropertyChanged
    {
//^^
//{[{
        private void OnMenuViewswts.ItemName(object sender, RoutedEventArgs e)
            => _navigationService.NavigateTo(typeof(wts.ItemNamePage), null, true);
//}]}

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
