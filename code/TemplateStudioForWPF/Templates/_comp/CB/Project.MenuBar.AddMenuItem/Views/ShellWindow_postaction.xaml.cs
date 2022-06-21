namespace Param_RootNamespace.Views;

public partial class ShellWindow : MetroWindow, IShellWindow, INotifyPropertyChanged
{
    //^^
    //{[{
    private void OnMenuViewsts.ItemName(object sender, RoutedEventArgs e)
        => _navigationService.NavigateTo(typeof(ts.ItemNamePage), null, true);
    //}]}

    public event PropertyChangedEventHandler PropertyChanged;
}
