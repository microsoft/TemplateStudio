
public void OnNavigationCompleted(object sender, CoreWebView2NavigationCompletedEventArgs e)
{
    IsLoading = false;
//^^
//{[{
    BrowserBackCommand.OnCanExecuteChanged();
    BrowserForwardCommand.OnCanExecuteChanged();
//}]}
}