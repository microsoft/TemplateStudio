Namespace Views
    Public NotInheritable Partial Class ShellPage
        Inherits Page
        Implements INotifyPropertyChanged

'{[{

Private Sub ShellMenuItemClick_Views_wts.ItemName(sender As Object, e As RoutedEventArgs)
    MenuNavigationHelper.UpdateView(GetType(wts.ItemNamePage))
End Sub
'}]}
        End Sub
    End Class
End Namespace
