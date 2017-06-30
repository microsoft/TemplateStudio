//{[{
Imports Windows.UI.Xaml.Navigation
//}]}
Namespace Views
    Public NotInheritable Partial Class wts.ItemNamePage
        Inherits Page
        Implements INotifyPropertyChanged
        Public Sub New()
        End Sub

//{[{
        Protected Overrides Sub OnNavigatedTo(e As NavigationEventArgs)
            AppDescription = GetAppDescription()
        End Sub
//}]}
    End Class
End Namespace
