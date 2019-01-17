Namespace Views
    Public NotInheritable Partial Class wts.ItemNameDetailPage
        Inherits Page
        Implements INotifyPropertyChanged

        Public Sub New()
        End Sub

        '{[{
        Private Sub OnGoBack(sender As Object, e As RoutedEventArgs)
            If NavigationService.CanGoBack Then
                NavigationService.GoBack()
            End If
        End Sub
        '}]}
    End Class
End Namespace
