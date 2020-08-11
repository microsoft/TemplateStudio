'{[{
Imports Microsoft.Toolkit.Mvvm.Input
'}]}

        Private Sub NavCompleted(e As WebViewNavigationCompletedEventArgs)
            IsLoading = False
            '{[{
            OnPropertyChanged(nameof(BrowserBackCommand))
            OnPropertyChanged(nameof(BrowserForwardCommand))
            '}]}
        End Sub
