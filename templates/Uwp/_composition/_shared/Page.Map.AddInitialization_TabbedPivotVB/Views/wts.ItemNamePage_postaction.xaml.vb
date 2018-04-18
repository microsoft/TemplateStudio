Namespace Views
    Public NotInheritable Partial Class wts.ItemNamePage
        Inherits Page

        Public Sub New()
            InitializeComponent()
            '{[{
            AddHandler Loaded, AddressOf wts.ItemNamePage_Loaded
            AddHandler Unloaded, AddressOf wts.ItemNamePage_Unloaded
            '}]}
        End Sub

        '{[{
        Private Async Sub wts.ItemNamePage_Loaded(sender As Object, e As RoutedEventArgs)
            Await ViewModel.InitializeAsync(mapControl)
        End Sub

        Private Sub wts.ItemNamePage_Unloaded(sender As Object, e As RoutedEventArgs)
            ViewModel.Cleanup()
        End Sub
        '}]}
    End Class
End Namespace
