Namespace Views
    Public NotInheritable Partial Class ShellPage
        Inherits Page
        Implements INotifyPropertyChanged

        Public Sub New()
        End Sub
'^^
'{[{

        Private Sub ShellMenuItemClick_File_wts.ItemName(sender As Object, e As RoutedEventArgs)
            MenuNavigationHelper.OpenInRightPane(GetType(wts.ItemNamePage))
        End Sub

'}]}
    End Class
End Namespace
