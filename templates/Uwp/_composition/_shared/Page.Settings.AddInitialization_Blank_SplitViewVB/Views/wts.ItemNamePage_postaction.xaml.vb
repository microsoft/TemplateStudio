Namespace Views
    Public NotInheritable Partial Class wts.ItemNamePage
        Inherits Page
        '^^

        '{[{
        Protected Overrides Sub OnNavigatedTo(e As NavigationEventArgs)
            ViewModel.Initialize()
        End Sub
        '}]}
    End Class
End Namespace
