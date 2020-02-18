
public void OnNavigationCompleted(WebViewControlNavigationCompletedEventArgs e)
{
    IsLoading = false;
//^^
//{[{
    BrowserBackCommand.OnCanExecuteChanged();
    BrowserForwardCommand.OnCanExecuteChanged();
//}]}
}