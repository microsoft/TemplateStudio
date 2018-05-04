Namespace Views
    Public NotInheritable Partial Class wts.ItemNamePage
        Inherits Page
        '^^

        '{[{
        Protected Overrides Async Sub OnNavigatedTo(e As NavigationEventArgs)
            Await ViewModel.InitializeAsync(mapControl)
        End Sub

        Protected Overrides Sub OnNavigatedFrom(e As NavigationEventArgs)
            ViewModel.Cleanup()
        End Sub
        '}]}
    End Class
End Namespace
