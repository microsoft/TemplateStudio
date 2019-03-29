'{[{
Imports GalaSoft.MvvmLight.Command
'}]}
Namespace ViewModels
    Public Class wts.ItemNameViewModel
        Inherits ViewModelBase

        Private Sub NavCompleted(e As WebViewNavigationCompletedEventArgs)
            IsLoading = False
            '{[{
            RaisePropertyChanged(nameof(BrowserBackCommand))
            RaisePropertyChanged(nameof(BrowserForwardCommand))
            '}]}
        End Sub
    End Class
End Namespace
