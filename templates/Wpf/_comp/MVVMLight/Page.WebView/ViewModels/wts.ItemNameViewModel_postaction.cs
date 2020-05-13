
public void OnNavigationCompleted(WebViewControlNavigationCompletedEventArgs e)
{
    IsLoading = false;
//^^
//{[{
    BrowserBackCommand.RaiseCanExecuteChanged();
    BrowserForwardCommand.RaiseCanExecuteChanged();
//}]}
}