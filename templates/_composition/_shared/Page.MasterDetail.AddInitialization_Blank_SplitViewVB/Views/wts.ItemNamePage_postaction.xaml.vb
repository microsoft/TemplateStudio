'{[{
Imports Windows.UI.Xaml.Navigation
'}]}
Namespace Views
    Public NotInheritable Partial Class wts.ItemNamePage
        Inherits Page
        '^^

        '{[{
        Protected Overrides Async Sub OnNavigatedTo(e As NavigationEventArgs)
            Await ViewModel.LoadDataAsync(WindowStates.CurrentState)
        End Sub
        '}]}
    End Class
End Namespace
