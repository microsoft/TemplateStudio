Namespace Views
    Public NotInheritable Partial Class wts.ItemNamePage
        Inherits Page
        Protected Overrides Async Sub OnNavigatedTo(e As NavigationEventArgs)
            MyBase.OnNavigatedTo(e)
            Await ViewModel.LoadDataAsync()
        End Sub
    End Class
End Namespace