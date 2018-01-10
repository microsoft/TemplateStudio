Namespace Views
    Public NotInheritable Partial Class wts.ItemNamePage
        Inherits Page
        Implements INotifyPropertyChanged

        Public Sub New()
        End Sub

        '{[{
        Protected Overrides Async Sub OnNavigatedTo(e As NavigationEventArgs)
            Await InitializeAsync()
        End Sub

        Protected Overrides Sub OnNavigatedFrom(e As NavigationEventArgs)
            Cleanup()
        End Sub
        '}]}
    End Class
End Namespace
