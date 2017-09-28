'{[{
Imports Windows.UI.Xaml.Navigation
'}]}
Namespace Views
    Public NotInheritable Partial Class wts.ItemNamePage
        Inherits Page
        Implements INotifyPropertyChanged
        Public Sub New()
        End Sub

        '{[{
        Protected Overrides Async Sub OnNavigatedTo(e As NavigationEventArgs)
            Await LoadDataAsync()
        End Sub
        '}]}
    End Class
End Namespace
