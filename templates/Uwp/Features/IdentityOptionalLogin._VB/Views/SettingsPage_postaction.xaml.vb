Namespace Views
    Public NotInheritable Partial Class SettingsPage
        Inherits Page

        Protected Overrides Async Sub OnNavigatedTo(e As NavigationEventArgs)
        End Sub
'^^
'{[{

        Protected Overrides Sub OnNavigatingFrom(e As NavigatingCancelEventArgs)
            MyBase.OnNavigatingFrom(e)
            ViewModel.UnregisterEvents()
        End Sub
'}]}
    End Class
End Namespace
