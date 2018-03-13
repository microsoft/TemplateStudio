Namespace Views
    Public NotInheritable Partial Class wts.ItemNamePage
        Inherits Page
        Implements INotifyPropertyChanged

        Public Sub New()
            '{[{
            AddHandler Loaded, AddressOf wts.ItemNamePage_Loaded
            AddHandler Unloaded, AddressOf wts.ItemNamePage_Unloaded
            '}]}
            InitializeComponent()
        End Sub

        '{[{
        Private Async Sub wts.ItemNamePage_Loaded(sender As Object, e As RoutedEventArgs)
            Await InitializeAsync()
        End Sub

        Private Sub wts.ItemNamePage_Unloaded(sender As Object, e As RoutedEventArgs)
            Cleanup()
        End Sub
        '}]}
    End Class
End Namespace
