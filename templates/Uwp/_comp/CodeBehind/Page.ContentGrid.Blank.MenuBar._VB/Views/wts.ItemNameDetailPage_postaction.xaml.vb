'{[{
Imports Windows.UI.Xaml
'}]}

Namespace Views
    Public NotInheritable Partial Class wts.ItemNameDetailPage
        Inherits Page
        Implements INotifyPropertyChanged
'^^
'{[{

        Private Sub OnGoBack(sender As Object, e As RoutedEventArgs)
            If NavigationService.CanGoBack Then
                NavigationService.GoBack()
            End If
        End Sub
'}]}
    End Class
End Namespace
