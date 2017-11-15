        Private Sub NavCompleted(e As WebViewNavigationCompletedEventArgs)
            IsLoading = False
            '{[{
            OnPropertyChanged(nameof(BrowserBackCommand))
            OnPropertyChanged(nameof(BrowserForwardCommand))
            '}]}
        End Sub
